resource "aws_lb" "application" {
  name               = "${var.APPLICATION_NAME}-${var.ENV_NAME}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb.id]
  subnets            = [aws_subnet.public_AZ_A.id, aws_subnet.public_AZ_B.id, aws_subnet.public_AZ_C.id]
  depends_on = [aws_nat_gateway.main]
  # Include other configurations...
}

resource "aws_lb_target_group" "appserver_port_80" {
  count            = var.APP_SERVER_HAS_IMAGE ? 0 : 1
  name             = "${var.APPLICATION_NAME}-${var.ENV_NAME}-appserver"
  port             = 80
  protocol         = "HTTP"
  vpc_id           = aws_vpc.main.id
  target_type      = "ip"  

  health_check {
    enabled             = true
    interval            = 30
    path                = "/"
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
  }
}

resource "aws_lb_target_group" "appserver_port_3000" {
  count            = var.APP_SERVER_HAS_IMAGE ? 1 : 0
  name             = "${var.APPLICATION_NAME}-${var.ENV_NAME}-appserver"
  port             = 3000
  protocol         = "HTTP"
  vpc_id           = aws_vpc.main.id
  target_type      = "ip"  

  health_check {
    enabled             = true
    interval            = 30
    path                = "/"
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
  }
}

resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.application.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = var.APP_SERVER_HAS_IMAGE ? aws_lb_target_group.appserver_port_3000[0].arn : aws_lb_target_group.appserver_port_80[0].arn
  }
}
