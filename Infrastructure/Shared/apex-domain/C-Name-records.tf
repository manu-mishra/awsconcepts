resource "aws_route53_record" "cname_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = "autodiscover.${var.domain_name}"
  type    = "CNAME"
  ttl     = "3600"

  records = [
    "autodiscover.outlook.com."
  ]
}
