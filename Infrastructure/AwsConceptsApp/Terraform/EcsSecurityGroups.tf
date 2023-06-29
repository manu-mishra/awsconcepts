resource "aws_security_group" "alb" {
  name   = "${var.APPLICATION_NAME}-${var.ENV_NAME}-alb"
  vpc_id = aws_vpc.main.id

  # Define ingress, egress rules here...

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-alb"
  }
}

resource "aws_security_group" "ecs_tasks" {
  name   = "${var.APPLICATION_NAME}-${var.ENV_NAME}-ecs_tasks"
  vpc_id = aws_vpc.main.id

  # Define ingress, egress rules here...

  tags = {
    Name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-ecs_tasks"
  }
}
