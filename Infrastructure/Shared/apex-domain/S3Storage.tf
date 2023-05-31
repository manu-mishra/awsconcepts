resource "aws_s3_bucket" "website_bucket" {
  bucket = "${var.domain_name}"
  force_destroy = true
  tags= local.tags
}
resource "aws_s3_bucket_website_configuration" "website-config" {
  bucket = aws_s3_bucket.website_bucket.id
  redirect_all_requests_to {
    host_name = "www.${var.domain_name}"
    protocol = "https"
  }
}
resource "aws_s3_bucket_server_side_encryption_configuration" "encryption_at_rest" {
  bucket = aws_s3_bucket.website_bucket.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}
resource "aws_s3_object" "redirectpage" {
  bucket = aws_s3_bucket.website_bucket.id
  key    = "index.html"
  content = "<html><head><meta http-equiv='refresh' content='0; url=https://www.${var.domain_name}/'></head></html>"
  content_type = "text/html"
}
resource "aws_s3_bucket_public_access_block" "website_s3block" {
  bucket                  = aws_s3_bucket.website_bucket.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
resource "aws_s3_bucket_policy" "s3_cf_policy" {
  bucket = aws_s3_bucket.website_bucket.id
  policy = data.aws_iam_policy_document.s3_cf_policy.json
}

