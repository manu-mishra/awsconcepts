resource "aws_security_group" "alb" {
  name   = "${var.APPLICATION_NAME}-${var.ENV_NAME}-alb"
  vpc_id = aws_vpc.main.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
    self        = true
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-alb"
  }
}

resource "aws_security_group" "ecs_tasks" {
  name   = "${var.APPLICATION_NAME}-${var.ENV_NAME}-ecs_tasks"
  vpc_id = aws_vpc.main.id

  ingress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    security_groups = [aws_security_group.alb.id]
    self        = true
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-ecs_tasks"
  }
}

resource "aws_security_group" "db" {
  name   = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db"
  vpc_id = aws_vpc.main.id

  ingress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    security_groups = [aws_security_group.ecs_tasks.id]
    self        = true
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-db"
  }
}
