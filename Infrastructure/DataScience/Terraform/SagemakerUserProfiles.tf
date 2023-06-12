resource "aws_sagemaker_user_profile" "manu" {
  domain_id         = aws_sagemaker_domain.sm-domain-1.id
  user_profile_name = "manu-mishra"
  user_settings {
    execution_role = aws_iam_role.sm_domain_iam_role.arn
    jupyter_server_app_settings {
      
      code_repository {
        repository_url = var.GIT_Hub_REPO_URL
      }
    }
  }
}