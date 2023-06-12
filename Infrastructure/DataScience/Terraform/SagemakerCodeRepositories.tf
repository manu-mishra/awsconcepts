resource "aws_sagemaker_code_repository" "awsconcepts" {
  code_repository_name = "awsconcepts"

  git_config {
    repository_url = var.GIT_Hub_REPO_URL
  }
}