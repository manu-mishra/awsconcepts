resource "aws_iam_openid_connect_provider" "github_actions" {
  url             = "https://token.actions.githubusercontent.com"
  client_id_list  = ["sts.amazonaws.com"]
  thumbprint_list = var.Git_Hub_Thumbprint_List
}

data "aws_iam_policy_document" "GitHubWritePolicy" {
  statement {
    effect  = "Allow"
    actions = ["sts:AssumeRoleWithWebIdentity"]

    principals {
      type        = "Federated"
      identifiers = [aws_iam_openid_connect_provider.github_actions.arn]
    }

    condition {
      test     = "StringEquals"
      variable = "token.actions.github.com:aud"
      values   = ["sts.amazonaws.com"]
    }

    condition {
      test     = "StringEquals"
      variable = "token.actions.githubusercontent.com:sub"
      values   = ["job_workflow_ref:manu-mishra/awsconcepts/.github/workflows/ds-embeddings-container-build.yml@refs/heads/main"]
    }
  }
}

resource "aws_iam_role" "GitHubWriteRole" {
  path                 = "/"
  name                 = "github-deploy-role"
  assume_role_policy   = data.aws_iam_policy_document.GitHubWritePolicy.json
  max_session_duration = 3600
  tags = {}
}

resource "aws_iam_role_policy_attachment" "GitHubWriteRole_AdminAccess" {
  role       = aws_iam_role.GitHubWriteRole.name
  policy_arn = "arn:aws:iam::aws:policy/AdministratorAccess"
}