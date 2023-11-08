resource "aws_route53_record" "txt_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = "${var.domain_name}"
  type    = "TXT"
  ttl     = "3600"
  
  records = [
    "MS=ms50990525",#Text verification record for Microsoft Entra Domain Verification
    "v=spf1 include:spf.protection.outlook.com -all"
    ]
}

resource "aws_route53_record" "mx_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = var.domain_name
  type    = "MX"
  ttl     = "3600"

  records = [
    "10 awsconcepts-com.mail.protection.outlook.com."
  ]
}
resource "aws_route53_record" "cname_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = "autodiscover.${var.domain_name}"
  type    = "CNAME"
  ttl     = "3600"

  records = [
    "autodiscover.outlook.com."
  ]
}
