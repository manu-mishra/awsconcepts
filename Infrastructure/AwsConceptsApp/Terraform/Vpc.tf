resource "aws_vpc" "main" {
  cidr_block = var.VPC_CIDR_BLOCK
  enable_dns_support   = true
  enable_dns_hostnames = true
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-vpc"
  }
}

resource "aws_eip" "nat" {
  vpc = true
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-eip"
  }
}

resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-igw"
  }
}

resource "aws_nat_gateway" "main" {
  allocation_id = aws_eip.nat.id
  subnet_id = aws_subnet.public_AZ_A.id

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-nat"
  }
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-rt"
  }
}
//aws_subnet.public_AZ_A.id, aws_subnet.public_AZ_B.id, aws_subnet.public_AZ_C.id
resource "aws_route_table_association" "public_AZ_A" {
  subnet_id      = aws_subnet.public_AZ_A.id
  route_table_id = aws_route_table.public.id
}
resource "aws_route_table_association" "public_AZ_B" {
  subnet_id      = aws_subnet.public_AZ_B.id
  route_table_id = aws_route_table.public.id
}
resource "aws_route_table_association" "public_AZ_C" {
  subnet_id      = aws_subnet.public_AZ_C.id
  route_table_id = aws_route_table.public.id
}

resource "aws_route_table" "private" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.main.id
  }
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-private-rt"
  }
}

resource "aws_route_table_association" "private_subnet_AZ_A" {
  subnet_id      = aws_subnet.container_workloads_AZ_A.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "private_subnet_AZ_B" {
  subnet_id      = aws_subnet.container_workloads_AZ_B.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "private_subnet_AZ_C" {
  subnet_id      = aws_subnet.container_workloads_AZ_C.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "db_subnet_AZ_A" {
  subnet_id      = aws_subnet.databases_AZ_A.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "db_subnet_AZ_B" {
  subnet_id      = aws_subnet.databases_AZ_B.id
  route_table_id = aws_route_table.private.id
}

resource "aws_route_table_association" "db_subnet_AZ_C" {
  subnet_id      = aws_subnet.databases_AZ_C.id
  route_table_id = aws_route_table.private.id
}
