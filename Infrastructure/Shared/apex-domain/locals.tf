locals {
  tags = {
    created_by = "terraform",
    owner=var.owner,
    Purpose=var.purpose,
    name=var.domain_name
  }
}