resource "aws_lambda_function" "lambda_function_pretoken" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  function_name = "${var.ENV_NAME}-lambda-pretoken"
  description = "API Lambda function for token generation"
  runtime = "dotnet6"
  handler = "LambdaApi::LambdaApi.LambdaEntryPoint::PreTokenGenerationHandler"
  memory_size = 1024
  timeout = 30
  tracing_config {
    mode = "Active"
  }
  environment {
    variables = {
      appBuildId = "${var.LAMBDA_API_BUILD_ID}"
      globalAdmins ="${var.GLOBALADMINS}"
    }
  }
  role = aws_iam_role.lambdaIAMRole.arn
  s3_bucket = var.STATEBUCKET
  s3_key    = "backend/lambdapackages/${var.LAMBDA_API_BUILD_ID}/lambda-api-package.zip"
  lifecycle {
    create_before_destroy = true
  }
  depends_on = [aws_iam_role.lambdaIAMRole]
  
}

resource "aws_lambda_permission" "get_pretoken" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  statement_id  = "AllowExecutionFromCognito"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.lambda_function_pretoken[0].function_name
  principal     = "cognito-idp.amazonaws.com"
  source_arn    = aws_cognito_user_pool.pool.arn
  depends_on = [ aws_lambda_function.lambda_function_pretoken ]
}