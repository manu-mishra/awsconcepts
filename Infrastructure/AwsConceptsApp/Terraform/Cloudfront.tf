resource "aws_cloudfront_distribution" "cf" {
  enabled             = true
  aliases             = ["${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}","${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}"]
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
  origin {
    domain_name = aws_apigatewayv2_domain_name.api_domain_name.domain_name
    origin_id   = "api-gateway"
    custom_origin_config {
      http_port = 80
      https_port = 443
      origin_protocol_policy = "https-only"
      origin_ssl_protocols= ["TLSv1.2"]
    }
  }

  ordered_cache_behavior {
    path_pattern = "api/*"
    target_origin_id = "api-gateway"
    viewer_protocol_policy = "redirect-to-https"
    allowed_methods  = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods   = ["GET", "HEAD", "OPTIONS"]

    forwarded_values {
      headers      = ["Origin", "Authorization"]
      query_string = true
      cookies {
        forward = "none"
      }
    }
    min_ttl                = 0
    default_ttl            = 1
    max_ttl                = 1
    compress               = true
  }


  default_cache_behavior {
    allowed_methods        = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods         = ["GET", "HEAD", "OPTIONS"]
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
  comment = "OAI for ${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}"
}

resource "aws_route53_record" "cloudfron_A_record" {
  zone_id = aws_route53_zone.zone.zone_id
  name    = "${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}"
  type    = "A"

  alias {
    name                   = aws_cloudfront_distribution.cf.domain_name
    zone_id                = aws_cloudfront_distribution.cf.hosted_zone_id
    evaluate_target_health = true
  }
}