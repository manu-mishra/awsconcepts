data "aws_iam_policy_document" "ecr_consumer_policy" {
  statement {
    actions = [
      "ecr:GetDownloadUrlForLayer",
      "ecr:BatchGetImage",
      "ecr:BatchCheckLayerAvailability",
      "ecr:PutImage",
      "ecr:InitiateLayerUpload",
      "ecr:UploadLayerPart",
      "ecr:CompleteLayerUpload",
    ]

    resources = [
      aws_ecr_repository.embeddings_container_repository.arn
    ]
  }
}

resource "aws_iam_policy" "ecr_consumer_policy" {
  name        = "ecr_consumer_policy"
  policy      = data.aws_iam_policy_document.ecr_consumer_policy.json
}
resource "aws_iam_role_policy_attachment" "ecr_consumer_policy_attach" {
  role       = aws_iam_role.sm_domain_iam_role.name
  policy_arn = aws_iam_policy.ecr_consumer_policy.arn
}