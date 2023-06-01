resource "aws_cloudwatch_log_group" "api_log_group" {
name = "/aws/lambda/${var.ENV_NAME}-lambda-api"
retention_in_days = 7
}
resource "aws_cloudwatch_log_group" "feedprocessor_log_group" {
name = "/aws/lambda/${var.ENV_NAME}-change-feed-processor"
retention_in_days = 7
}