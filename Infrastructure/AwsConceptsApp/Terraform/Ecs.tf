resource "aws_ecs_cluster" "main" {
  name = "${var.APPLICATION_NAME}-${var.ENV_NAME}-cluster"
}

resource "aws_ecs_task_definition" "appserver" {
  family                   = "appserver"
  network_mode             = "bridge"
  requires_compatibilities = ["EC2"]
  cpu                      = "256"
  memory                   = "512"

  container_definitions = jsonencode([
    {
      name  = "appserver1",
      image = "httpd:2.4",
      portMappings = [
        {
          containerPort = 80,
          hostPort      = 80,
          protocol      = "tcp"
        }
      ],
      essential = true
    }
  ])
}

resource "aws_ecs_service" "appserver" {
  name            = "appserver1"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.appserver.arn
  desired_count   = 1
  launch_type     = "EC2"

  load_balancer {
    target_group_arn = aws_lb_target_group.appserver.arn
    container_name   = "appserver1"
    container_port   = 80
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    subnets = [aws_subnet.container_workloads_AZ_A.id, aws_subnet.container_workloads_AZ_B.id, aws_subnet.container_workloads_AZ_C.id]
    assign_public_ip = false
  }

  depends_on = [
    aws_lb_listener.http
  ]
}

resource "aws_appautoscaling_target" "appserver" {
  max_capacity       = 100
  min_capacity       = 1
  resource_id        = "service/${aws_ecs_cluster.main.name}/${aws_ecs_service.appserver.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}

resource "aws_appautoscaling_policy" "cpu_scale_up" {
  name               = "${var.APPLICATION_NAME}-${var.ENV_NAME}-cpu-scale-up"
  service_namespace  = aws_appautoscaling_target.appserver.service_namespace
  scalable_dimension = aws_appautoscaling_target.appserver.scalable_dimension
  resource_id        = aws_appautoscaling_target.appserver.resource_id
  policy_type        = "TargetTrackingScaling"
  
  target_tracking_scaling_policy_configuration {
    target_value       = 80.0  # Target CPU utilization
    scale_in_cooldown  = 300  # The amount of time, in seconds, after a scale in activity completes before another can start.
    scale_out_cooldown = 60   # The amount of time, in seconds, after a scale out activity completes before another can start.
    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
  }
}
