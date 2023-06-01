resource "aws_dynamodb_table" "dynamoDBTable" {
  name           = "${var.APPLICATION_NAME}"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "ek"
  range_key      = "sk"

  attribute {
    name = "ek"
    type = "S"
  }

  attribute {
    name = "sk"
    type = "S"
  }

  # attribute {
  #   name = "CreatedAt"
  #   type = "S"
  # }

  # attribute {
  #   name = "LastUpdatedAt"
  #   type = "S"
  # }

  # attribute {
  #   name = "CreatedBy"
  #   type = "S"
  # }

  # attribute {
  #   name = "LastUpdatedBy"
  #   type = "S"
  # }
  # attribute {
  #   name = "etype"
  #   type = "S"
  # }

  # local_secondary_index {
  #   name               = "CreatedAt-index"
  #   range_key          = "CreatedAt"
  #   non_key_attributes = ["CreatedBy"]
  # }

  # local_secondary_index {
  #   name               = "LastUpdatedAt-index"
  #   range_key          = "LastUpdatedAt"
  #   non_key_attributes = ["LastUpdatedBy"]
  # }

  #   local_secondary_index {
  #   name               = "etype-index"
  #   range_key          = "etype"
  # }

  global_secondary_index {
    name               = "sk-pk-index"
    hash_key           = "sk"
    range_key          = "ek"
    projection_type    = "ALL"
    non_key_attributes = []
  }
}
