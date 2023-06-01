resource "aws_s3_bucket" "website_bucket" {
  bucket = "${var.ENV_NAME}.${var.APEX_DOMAIN}"
  force_destroy = true
  tags= local.tags
}
resource "aws_s3_bucket_server_side_encryption_configuration" "encryption_at_rest" {
  bucket = aws_s3_bucket.website_bucket.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}
resource "aws_s3_bucket_public_access_block" "website_s3block" {
  bucket                  = aws_s3_bucket.website_bucket.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
resource "aws_s3_bucket_acl" "website-bucket-acl" {
  bucket = aws_s3_bucket.website_bucket.id
  acl    = "private"
}
resource "aws_s3_bucket_policy" "s3_cf_policy" {
  bucket = aws_s3_bucket.website_bucket.id
  policy = data.aws_iam_policy_document.s3_cf_policy.json
}

##########################objectstore#######################################

resource "aws_s3_bucket" "object_store" {
  bucket = "${var.APPLICATION_NAME}${var.ENV_NAME}.objecjstore"
  force_destroy = true
  tags= local.tags
}
resource "aws_s3_bucket_server_side_encryption_configuration" "object_store_encryption_at_rest" {
  bucket = aws_s3_bucket.object_store.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_public_access_block" "object_store_s3block" {
  bucket                  = aws_s3_bucket.object_store.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
resource "aws_s3_bucket_acl" "object_store-acl" {
  bucket = aws_s3_bucket.object_store.id
  acl    = "private"
}

##############################dataLake##########################################
resource "aws_s3_bucket" "data_lake" {
  bucket = "${var.APPLICATION_NAME}.${var.ENV_NAME}.datalake"
  force_destroy = true
  tags= local.tags
}
resource "aws_s3_bucket_server_side_encryption_configuration" "data_lake_encryption_at_rest" {
  bucket = aws_s3_bucket.data_lake.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}
resource "aws_s3_bucket_public_access_block" "data_lake_s3block" {
  bucket                  = aws_s3_bucket.data_lake.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
resource "aws_s3_bucket_acl" "data_lake-bucket-acl" {
  bucket = aws_s3_bucket.data_lake.id
  acl    = "private"
}
