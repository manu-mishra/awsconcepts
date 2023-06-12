resource "aws_sagemaker_notebook_instance" "notebook_tokenization" {
  name          = "awsc-custom-tokenization"
  role_arn      = aws_iam_role.sm_domain_iam_role.arn
  instance_type = "ml.t2.medium"
  default_code_repository = aws_sagemaker_code_repository.awsconcepts.code_repository_name
  tags = {
    Name = "notebook-tokenization"
  }
}