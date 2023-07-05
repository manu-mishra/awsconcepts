# Create Public Subnet
resource "aws_subnet" "public_AZ_A" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = var.PUBLIC_SUBNET_CIDR_BLOCK_AZ_A
  map_public_ip_on_launch = true
  availability_zone       = var.AVAILABILITY_ZONE_A
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-subnet-AZ-A"
  }
}
# Create Public Subnet
resource "aws_subnet" "public_AZ_B" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = var.PUBLIC_SUBNET_CIDR_BLOCK_AZ_B
  map_public_ip_on_launch = true
  availability_zone       = var.AVAILABILITY_ZONE_B
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-subnet-AZ-B"
  }
}
# Create Public Subnet
resource "aws_subnet" "public_AZ_C" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = var.PUBLIC_SUBNET_CIDR_BLOCK_AZ_C
  map_public_ip_on_launch = true
  availability_zone       = var.AVAILABILITY_ZONE_C
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-subnet-AZ-C"
  }
}

# Create Container-Workload Subnets
resource "aws_subnet" "container_workloads_AZ_A" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_A
  availability_zone = var.AVAILABILITY_ZONE_A
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-workload-subnet-AZ-A"
  }
}

resource "aws_subnet" "container_workloads_AZ_B" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_B
  availability_zone = var.AVAILABILITY_ZONE_B
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-workload-subnet-AZ-B"
  }
}

resource "aws_subnet" "container_workloads_AZ_C" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_C
  availability_zone = var.AVAILABILITY_ZONE_C
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-workload-subnet-AZ-C"
  }
}

# Create Databases Subnets
resource "aws_subnet" "databases_AZ_A" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.DB_SUBNET_CIDR_BLOCK_AZ_A
  availability_zone = var.AVAILABILITY_ZONE_A
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db-subnet-AZ-A"
  }
}

resource "aws_subnet" "databases_AZ_B" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.DB_SUBNET_CIDR_BLOCK_AZ_B
  availability_zone = var.AVAILABILITY_ZONE_B
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db-subnet-AZ-B"
  }
}

resource "aws_subnet" "databases_AZ_C" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.DB_SUBNET_CIDR_BLOCK_AZ_C
  availability_zone = var.AVAILABILITY_ZONE_C
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db-subnet-AZ-C"
  }
}
