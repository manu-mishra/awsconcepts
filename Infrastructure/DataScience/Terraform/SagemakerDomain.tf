# create sage maker domain
resource "aws_sagemaker_domain" "sm-domain-1" {
  domain_name = "awsc-smdomain-1"
  auth_mode   = "IAM"
  vpc_id      = aws_vpc.datascience_vpc.id
  subnet_ids  = [aws_subnet.sagemakerdomain_subnet.id]
  retention_policy {
    home_efs_file_system = "Delete"
  }

  default_user_settings {
    execution_role = aws_iam_role.sm_domain_iam_role.arn
  }
}

# Attaching the AWS default policy, "AmazonSageMakerFullAccess" 
resource "aws_iam_policy_attachment" "sm_full_access_attach" {
  name = "sm-domain-full-access-attachment"
  roles = [aws_iam_role.sm_domain_iam_role.name]
  policy_arn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess"
}