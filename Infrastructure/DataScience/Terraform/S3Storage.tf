resource "aws_s3_bucket" "object_store" {
  bucket = "awsc.datascience.objecjstore"
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

resource "aws_iam_policy_attachment" "s3_full_access_attach" {
  name       = "s3-full-access-attachment"
  roles      = [aws_iam_role.sm_domain_iam_role.name]
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3FullAccess"
}