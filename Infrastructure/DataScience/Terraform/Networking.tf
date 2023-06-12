# Create VPC
resource "aws_vpc" "datascience_vpc" {
  cidr_block = "10.2.0.0/16"

  tags = {
    Name = "aws_datascience"
  }
}

# Create Subnet
resource "aws_subnet" "sagemakerdomain_subnet" {
  vpc_id                  = aws_vpc.datascience_vpc.id
  cidr_block              = "10.2.0.0/24" 

  tags = {
    Name = "sagemaker-domain-subnet"
  }
}
