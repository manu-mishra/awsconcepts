resource "aws_route53_record" "txt_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = "${var.domain_name}"
  type    = "TXT"
  ttl     = "3600"
  
  # Text verification record for Microsoft Entra DOmain Verification
  records = ["\"MS=ms50990525\""]
}