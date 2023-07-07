resource "aws_ecs_cluster" "main" {
  name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-cluster"
}
resource "aws_cloudwatch_log_group" "app1_log_group" {
  name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-app1-log-group"
  retention_in_days = 14
}
resource "aws_ecs_task_definition" "appserver" {
  family                   = "appserver"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "1024"
  memory                   = "2048"
  execution_role_arn = aws_iam_role.app_server_workload_iam_role.arn

  container_definitions = jsonencode([
    {
      name  = "${var.APPLICATION_NAME}-appserver1",
      image = var.APP_SERVER_HAS_IMAGE ? "${aws_ecr_repository.app1.repository_url}:${var.APP_SERVER_IMAGE_TAG}" : "nginxdemos/hello",
      portMappings = [
      {
        containerPort = var.APP_SERVER_HAS_IMAGE ? 3000 : 80,
        hostPort      = var.APP_SERVER_HAS_IMAGE ? 3000 : 80,
        protocol      = "tcp"
      }
      ],
      essential = true,
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-region        = var.REGION
          awslogs-group         = aws_cloudwatch_log_group.app1_log_group.name
          awslogs-stream-prefix = "app1_stream_prefix"
      }
    }
    }
  ])
}

resource "aws_ecs_service" "appserver" {
  name            = "${var.APPLICATION_NAME}-appserver"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.appserver.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    assign_public_ip = false
    security_groups  = [aws_security_group.ecs_tasks.id]
    subnets = [
      aws_subnet.container_workloads_AZ_A.id, 
      aws_subnet.container_workloads_AZ_B.id, 
      aws_subnet.container_workloads_AZ_C.id
    ]
  }

  load_balancer {
    target_group_arn =  var.APP_SERVER_HAS_IMAGE ? aws_lb_target_group.appserver_port_3000[0].arn : aws_lb_target_group.appserver_port_80[0].arn
    container_name   = "${var.APPLICATION_NAME}-appserver1"
    container_port   = var.APP_SERVER_HAS_IMAGE ? 3000 : 80
  }

  depends_on = [
    aws_lb_listener.http,
  ]
}

