resource "aws_cloudfront_distribution" "cf" {
  enabled             = true
  aliases             = ["${var.domain_name}"]
  default_root_object = "index.html"
  custom_error_response {
    error_code    = 403
    response_code = 200
    response_page_path = "/index.html"
  }

  custom_error_response {
    error_code    = 404
    response_code = 200
    response_page_path = "/index.html"
  }
  origin {
    domain_name = aws_s3_bucket.website_bucket.bucket_regional_domain_name
    origin_id   = aws_s3_bucket.website_bucket.bucket_regional_domain_name
    s3_origin_config {
      origin_access_identity = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
    }
  }
  default_cache_behavior {
    allowed_methods        = ["GET", "HEAD"]
    cached_methods         = ["GET", "HEAD"]
    target_origin_id       = aws_s3_bucket.website_bucket.bucket_regional_domain_name
    viewer_protocol_policy = "redirect-to-https"


    forwarded_values {
      headers      = []
      query_string = true
      cookies {
        forward = "all"
      }
    }
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }
  viewer_certificate {
    acm_certificate_arn      = aws_acm_certificate.certificate.arn
    ssl_support_method       = "sni-only"
    minimum_protocol_version = "TLSv1.2_2021"
  }
  tags = local.tags
}
resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "OAI for ${var.domain_name}"
}

resource "aws_route53_record" "cloudfront_A_record" {
  zone_id = aws_route53_zone.apex_domain_zone.zone_id
  name    = "${var.domain_name}"
  type    = "A"

  alias {
    name                   = aws_cloudfront_distribution.cf.domain_name
    zone_id                = aws_cloudfront_distribution.cf.hosted_zone_id
    evaluate_target_health = true
  }
}