resource "aws_sagemaker_model" "embeddings_model" {
  count              = var.Is_Ecr_Embeddings_Container_Image_Ready ? 1 : 0
  name               = "awsc-custom-embeddings-model"
  execution_role_arn = aws_iam_role.sm_domain_iam_role.arn
  primary_container {
    image            = "${aws_ecr_repository.embeddings_container_repository.repository_url}:latest"
  }
}

resource "aws_sagemaker_endpoint_configuration" "embeddings_end_point_config" {
  count              = var.Is_Ecr_Embeddings_Container_Image_Ready ? 1 : 0
  name               = "awsc-dev-embeddings-end-point-config"
  production_variants {
    variant_name           = "prod"
    model_name             = aws_sagemaker_model.embeddings_model[count.index].name
    serverless_config {
      max_concurrency = 10
      memory_size_in_mb = 6144       
    }
  }
}

resource "aws_sagemaker_endpoint" "embeddings_end_point" {
  count              = var.Is_Ecr_Embeddings_Container_Image_Ready ? 1 : 0
  name                 = "awsc-dev-embeddings-end-point"
  endpoint_config_name = aws_sagemaker_endpoint_configuration.embeddings_end_point_config[count.index].name
}
