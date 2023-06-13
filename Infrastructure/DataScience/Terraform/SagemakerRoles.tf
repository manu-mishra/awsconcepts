
# create a sagemaker policy
data "aws_iam_policy_document" "sm_assume_role_policy" {
  statement {
    actions = ["sts:AssumeRole"]
    
    principals {
      type = "Service"
      identifiers = ["sagemaker.amazonaws.com"]
    }
  }
}
#############SM Roles######################
# create a role
resource "aws_iam_role" "sm_domain_iam_role" {
  name = "sm_notebook_role"
  assume_role_policy = data.aws_iam_policy_document.sm_assume_role_policy.json
}

# Attaching the AWS default policy, "AmazonSageMakerFullAccess" 
resource "aws_iam_policy_attachment" "sm_full_access_attach" {
  name = "sm-domain-full-access-attachment"
  roles = [aws_iam_role.sm_domain_iam_role.name]
  policy_arn = "arn:aws:iam::aws:policy/AmazonSageMakerFullAccess"
}

resource "aws_iam_policy_attachment" "s3_full_access_attach" {
  name       = "s3-full-access-attachment"
  roles      = [aws_iam_role.sm_domain_iam_role.name]
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3FullAccess"
}

