resource "aws_cognito_user_pool" "pool" {
  name = "${var.ENV_NAME}-pool"
  deletion_protection = "ACTIVE"  
  auto_verified_attributes = ["email"]
  username_attributes      = ["email"]
  
  device_configuration {
    device_only_remembered_on_user_prompt = true
  }
  username_configuration {
    case_sensitive = false
  }
  schema {
     attribute_data_type      = "String"
      developer_only_attribute = false 
      mutable                  = true 
      name                     = "app_claims" 
      required                 = false 
      string_attribute_constraints {}
  }
  account_recovery_setting {
    recovery_mechanism {
      name     = "verified_email"
      priority = 1
    }
  }
  admin_create_user_config {
    allow_admin_create_user_only = false
  }
  verification_message_template {
    email_subject = "Verify your ${var.APPLICATION_NAME} account"
  }
  password_policy {
    minimum_length = 8
    require_lowercase = true
    require_numbers = true 
    require_symbols = true
    require_uppercase = true 
    temporary_password_validity_days = 1 
  }
  lambda_config {
    pre_token_generation = var.LAMBDA_HASCODE_API ? "${aws_lambda_function.lambda_function_pretoken[0].arn}" : ""
  }
  
}
resource "aws_cognito_resource_server" "resource" {
  identifier = "https://${var.ENV_SUBDOMAIN}${var.APEX_DOMAIN}"
  name       = "api"

  user_pool_id = aws_cognito_user_pool.pool.id
}
resource "aws_cognito_user_pool_client" "api_client" {
  name                = "api_client"
  user_pool_id        = aws_cognito_user_pool.pool.id
  generate_secret     = false
  explicit_auth_flows = ["ALLOW_CUSTOM_AUTH","ALLOW_USER_SRP_AUTH","ALLOW_REFRESH_TOKEN_AUTH"]
  refresh_token_validity = 30
  id_token_validity = 1
  access_token_validity = 1
  read_attributes = [ "custom:app_claims", "email", "email_verified", "family_name", "given_name", "nickname"]
  write_attributes = [ "email","family_name", "given_name", "nickname"]
}
