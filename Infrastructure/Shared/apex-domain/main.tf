resource "aws_route53_zone" "apex_domain_zone" {
  name = var.domain_name
  tags = local.tags
}


resource "aws_route53_record" "ns_records" {
  for_each = var.environments
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name = "${each.value.subdomain}.${var.domain_name}."
  type = "NS"
  ttl = "3600"
  records = each.value.dns_settings.ns_records
}

resource "aws_acm_certificate" "certificate" {
  domain_name = "${var.domain_name}"
  subject_alternative_names = ["*.${var.domain_name}"]
  validation_method         = "DNS"
  tags                      = local.tags
}

resource "aws_route53_record" "cert_validation" {
  for_each = {
    for d in aws_acm_certificate.certificate.domain_validation_options : d.domain_name => {
      name   = d.resource_record_name
      record = d.resource_record_value
      type   = d.resource_record_type
    }
  }
  allow_overwrite = true
  name            = each.value.name
  records         = [each.value.record]
  ttl             = 30
  type            = each.value.type
  zone_id         = aws_route53_zone.apex_domain_zone.zone_id
}

resource "aws_acm_certificate_validation" "cert_validation" {
  depends_on = [aws_route53_zone.apex_domain_zone,aws_acm_certificate.certificate]
  certificate_arn = aws_acm_certificate.certificate.arn
  validation_record_fqdns = [for r in aws_route53_record.cert_validation : r.fqdn]
}