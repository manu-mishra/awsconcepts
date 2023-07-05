resource "aws_ecr_repository" "app1" {
  name                 = "${var.ENV_NAME}-${var.APPLICATION_NAME}-repository"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}
data "aws_iam_policy_document" "ecr_permissions" {
  statement {
    effect = "Allow"
    actions = [
      "ecr:GetDownloadUrlForLayer",
      "ecr:BatchGetImage",
      "ecr:BatchCheckLayerAvailability",
      "ecr:GetAuthorizationToken",
      "ecr:PutImage",
      "ecr:InitiateLayerUpload",
      "ecr:UploadLayerPart",
      "ecr:CompleteLayerUpload"
    ]

    resources = ["*"]
  }
}

resource "aws_iam_policy" "ecr_policy" {
  name        = "ecr_policy"
  path        = "/"
  description = "ECR permissions policy"
  policy      = data.aws_iam_policy_document.ecr_permissions.json
}

resource "aws_iam_role_policy_attachment" "ecs_ecr_policy_attach" {
  role       = aws_iam_role.app_server_workload_iam_role.name
  policy_arn = aws_iam_policy.ecr_policy.arn
}

 resource "aws_vpc_endpoint" "ecr_api" {
  vpc_id            = aws_vpc.main.id
  service_name      = "com.amazonaws.${var.REGION}.ecr.api"
  vpc_endpoint_type = "Interface"
  subnet_ids        = [aws_subnet.container_workloads_AZ_A.id, aws_subnet.container_workloads_AZ_B.id, aws_subnet.container_workloads_AZ_C.id]

  security_group_ids = [aws_security_group.ecs_tasks.id]

  private_dns_enabled = true
}

resource "aws_vpc_endpoint" "ecr_dkr" {
  vpc_id            = aws_vpc.main.id
  service_name      = "com.amazonaws.${var.REGION}.ecr.dkr"
  vpc_endpoint_type = "Interface"
  subnet_ids        = [aws_subnet.container_workloads_AZ_A.id, aws_subnet.container_workloads_AZ_B.id, aws_subnet.container_workloads_AZ_C.id]

  security_group_ids = [aws_security_group.ecs_tasks.id]

  private_dns_enabled = true
}
resource "aws_vpc_endpoint" "s3" {
  vpc_id            = aws_vpc.main.id
  service_name      = "com.amazonaws.${var.REGION}.s3"
  vpc_endpoint_type = "Gateway"
  route_table_ids   = [aws_route_table.private.id,aws_route_table.public.id]

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-s3-endpoint"
  }
}

