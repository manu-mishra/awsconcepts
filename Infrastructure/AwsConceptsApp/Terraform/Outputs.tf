output "cognito-api-client-id" {
  value = aws_cognito_user_pool_client.api_client.id
}
output "cognito-pool-endpoint" {
  value = aws_cognito_user_pool.pool.endpoint
}
output "cognito-pool-id" {
  value = aws_cognito_user_pool.pool.id
}
output "env-ui-url" {
  value = "${var.ENV_SUBDOMAIN}.${var.APEX_DOMAIN}"
}
