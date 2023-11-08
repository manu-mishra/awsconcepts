resource "aws_route53_record" "mx_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = var.domain_name
  type    = "MX"
  ttl     = "3600"

  records = [
    "0 awsconcepts-com.mail.protection.outlook.com."
  ]
}
