# Create NACL for Public Subnet
resource "aws_network_acl" "public_AZ_A" {
  vpc_id = aws_vpc.main.id

  egress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  ingress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-nacl"
  }
}
resource "aws_network_acl" "public_AZ_B" {
  vpc_id = aws_vpc.main.id

  egress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  ingress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-nacl"
  }
}
resource "aws_network_acl" "public_AZ_C" {
  vpc_id = aws_vpc.main.id

  egress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  ingress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-public-nacl"
  }
}

# Associate the Public NACL with the Public Subnet
resource "aws_network_acl_association" "public_AZ_A" {
  network_acl_id = aws_network_acl.public_AZ_A.id
  subnet_id      = aws_subnet.public_AZ_A.id
}

# Associate the Public NACL with the Public Subnet
resource "aws_network_acl_association" "public_AZ_B" {
  network_acl_id = aws_network_acl.public_AZ_B.id
  subnet_id      = aws_subnet.public_AZ_B.id
}

# Associate the Public NACL with the Public Subnet
resource "aws_network_acl_association" "public_AZ_C" {
  network_acl_id = aws_network_acl.public_AZ_C.id
  subnet_id      = aws_subnet.public_AZ_C.id
}

# Create NACLs for each Container-Workload Subnet
resource "aws_network_acl" "container_workloads" {
  vpc_id = aws_vpc.main.id

  egress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  ingress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-workload-nacl"
  }
}

# Associate the Container-Workloads NACL with the corresponding Subnets
resource "aws_network_acl_association" "container_workloads_AZ_A" {
  network_acl_id = aws_network_acl.container_workloads.id
  subnet_id      = aws_subnet.container_workloads_AZ_A.id
}

resource "aws_network_acl_association" "container_workloads_AZ_B" {
  network_acl_id = aws_network_acl.container_workloads.id
  subnet_id      = aws_subnet.container_workloads_AZ_B.id
}

resource "aws_network_acl_association" "container_workloads_AZ_C" {
  network_acl_id = aws_network_acl.container_workloads.id
  subnet_id      = aws_subnet.container_workloads_AZ_C.id
}

# Create NACLs for each Databases Subnet
resource "aws_network_acl" "databases" {
  vpc_id = aws_vpc.main.id

  egress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 0
  }

  ingress {
    protocol   = "-1"
    rule_no    = 100
    action     = "allow"
    cidr_block = aws_vpc.main.cidr_block
    from_port  = 0
    to_port    = 0
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db-nacl"
  }
}

# Associate the Databases NACL with the corresponding Subnets
resource "aws_network_acl_association" "databases_AZ_A" {
  network_acl_id = aws_network_acl.databases.id
  subnet_id      = aws_subnet.databases_AZ_A.id
}

resource "aws_network_acl_association" "databases_AZ_B" {
  network_acl_id = aws_network_acl.databases.id
  subnet_id      = aws_subnet.databases_AZ_B.id
}

resource "aws_network_acl_association" "databases_AZ_C" {
  network_acl_id = aws_network_acl.databases.id
  subnet_id      = aws_subnet.databases_AZ_C.id
}

