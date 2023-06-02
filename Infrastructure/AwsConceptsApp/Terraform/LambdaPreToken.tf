# resource "aws_lambda_function" "api_lambda_function_pretoken" {
#   count = var.LAMBDA_HASCODE_API ? 1 : 0
#   function_name = "${var.ENV_NAME}-lambda-pretoken"
#   description = "API Lambda function for token generation"
#   runtime = "dotnet6"
#   handler = "LambdaApi::LambdaApi.LambdaEntryPoint::PreTokenGenerationHandler"
#   memory_size = 1024
#   timeout = 30
#   tracing_config {
#     mode = "Active"
#   }
#   environment {
#     variables = {
#       elasticUserName = "dummy"
#       elasticPassword = "dummy"
#       oAuthAudiance = "${aws_cognito_user_pool_client.api_client.id}"
#       oAuthAuthority = "https://${aws_cognito_user_pool.pool.endpoint}"
#       appBuildId = "${var.LAMBDA_API_BUILD_ID}"
#       globalAdmins ="${var.GLOBALADMINS}"
#     }
#   }
#   role = aws_iam_role.lambdaIAMRole.arn
#   s3_bucket = var.STATEBUCKET
#   s3_key    = "backend/lambdapackages/${var.LAMBDA_API_BUILD_ID}/lambda-api-package.zip"
#   lifecycle {
#     create_before_destroy = true
#   }
#   depends_on = [aws_iam_role.lambdaIAMRole]
  
# }

