# Calculate the number of available IP addresses
data "aws_subnet" "sagemakerdomain_subnet_data" {
  id = aws_subnet.sagemakerdomain_subnet.id
}

