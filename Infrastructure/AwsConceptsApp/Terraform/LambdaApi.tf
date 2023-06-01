resource "aws_lambda_function" "api_lambda_function" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  function_name = "${var.ENV_NAME}-lambda-api"
  description = "API Lambda function"
  runtime = "dotnet6"
  handler = "LambdaApi::LambdaApi.LambdaEntryPoint::TracingFunctionHandlerAsync"
  memory_size = 1024
  timeout = 30
  tracing_config {
    mode = "Active"
  }
  environment {
    variables = {
      elasticUserName = "dummy"
      elasticPassword = "dummy"
      oAuthAudiance = "${aws_cognito_user_pool_client.api_client.id}"
      oAuthAuthority = "https://${aws_cognito_user_pool.pool.endpoint}"
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
  #https://github.com/aws-observability/aws-otel-lambda/blob/main/dotnet/sample-apps/aws-sdk/deploy/wrapper/layer_amd64.tf
  layers = ["arn:aws:lambda:us-east-1:901920570463:layer:aws-otel-collector-amd64-ver-0-68-0:1"]
  depends_on = [aws_iam_role.lambdaIAMRole]
  
}

resource "aws_lambda_permission" "lambda_permission" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  action = "lambda:InvokeFunction"
  function_name = aws_lambda_function.api_lambda_function[0].arn
  principal = "apigateway.amazonaws.com"
  source_arn = aws_apigatewayv2_api.api_gateway.arn
  depends_on = [aws_apigatewayv2_api.api_gateway]
}

resource "aws_apigatewayv2_integration" "api_lambda_integration" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  api_id = aws_apigatewayv2_api.api_gateway.id
  connection_type = "INTERNET"
  integration_method = "POST"
  integration_type = "AWS_PROXY"
  integration_uri = aws_lambda_function.api_lambda_function[0].arn
  timeout_milliseconds = 30000
  payload_format_version = "2.0"
  depends_on = [ aws_apigatewayv2_api.api_gateway]
}
resource "aws_apigatewayv2_route" "apiGatewayV2Route" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  api_id = aws_apigatewayv2_api.api_gateway.id
  route_key = "ANY /{proxy+}"
  target = "integrations/${aws_apigatewayv2_integration.api_lambda_integration[0].id}"
  api_key_required = false
  authorization_type = "NONE"
  depends_on = [ aws_apigatewayv2_integration.api_lambda_integration[0]]
}

resource "aws_lambda_permission" "lambda_api_gateway_invoke" {
  count = var.LAMBDA_HASCODE_API ? 1 : 0
  action = "lambda:InvokeFunction"
  function_name = aws_lambda_function.api_lambda_function[0].arn
  principal = "apigateway.amazonaws.com"
  source_arn = "${aws_apigatewayv2_api.api_gateway.execution_arn}/*/*/{proxy+}"
  depends_on = [ aws_apigatewayv2_route.apiGatewayV2Route[0]]
}