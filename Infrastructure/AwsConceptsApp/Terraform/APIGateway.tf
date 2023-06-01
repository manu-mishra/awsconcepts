resource "aws_apigatewayv2_domain_name" "api_domain_name" {
  domain_name = "api.${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}"
  domain_name_configuration {
    endpoint_type = "REGIONAL"
    certificate_arn = aws_acm_certificate.certificate.arn
    security_policy = "TLS_1_2"
  }
}

resource "aws_apigatewayv2_api" "api_gateway" {
  name = "${var.ENV_NAME}-api-gtw"
  protocol_type = "HTTP"
  description = "${var.APPLICATION_NAME} ${var.ENV_NAME} API Gateway"
  cors_configuration {
    allow_headers = [
      "authorization",
      "*"
    ]
    allow_origins = [
      "https://${aws_apigatewayv2_domain_name.api_domain_name.domain_name}",
      "https://${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}",
      "http://localhost:3000"
    ]
    allow_methods = [
      "GET",
      "HEAD",
      "OPTIONS",
      "POST",
      "PUT",
      "DELETE",
      "PATCH"
    ]
  }
}

resource "aws_apigatewayv2_stage" "api_gateway_stage" {
  api_id = aws_apigatewayv2_api.api_gateway.id
  name = "$default"
  auto_deploy = true
}

resource "aws_apigatewayv2_api_mapping" "api_base_path_mapping" {
  api_id = aws_apigatewayv2_api.api_gateway.id
  domain_name = aws_apigatewayv2_domain_name.api_domain_name.id
  stage = aws_apigatewayv2_stage.api_gateway_stage.id
}

resource "aws_route53_record" "apigateway_A_record" {
  zone_id = aws_route53_zone.zone.zone_id
  name    = aws_apigatewayv2_domain_name.api_domain_name.domain_name
  type    = "A"

  alias {
    name                   = aws_apigatewayv2_domain_name.api_domain_name.domain_name_configuration[0].target_domain_name
    zone_id                = aws_apigatewayv2_domain_name.api_domain_name.domain_name_configuration[0].hosted_zone_id
    evaluate_target_health = true
  }
}

