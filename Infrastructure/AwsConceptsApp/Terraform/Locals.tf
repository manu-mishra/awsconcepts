locals {
  tags = {
    created_by = "terraform",
    name=var.ENV_NAME
  }
  aws_account_id = data.aws_caller_identity.current.account_id
}