variable "APPLICATION_NAME" {
  description = "name of application"
  type        = string
  default = "awsconcepts"
}
variable "ENV_NAME" {
  description = "name of environment"
  type        = string
}
variable "ENV_SUBDOMAIN" {
  description = "subdomain appended before apex domain to access ui"
  type        = string
}

variable "APEX_DOMAIN" {
  description = "Domain Name"
  type        = string
}
variable "REGION" {
  description = "aws region"
  type        = string
} 
variable "STATEBUCKET" {
  description = "terraform state bucket"
  type        = string
}
variable "LAMBDA_HASCODE_API" {
  type = bool
  default = false
}
variable "LAMBDA_API_BUILD_ID" {
  type = string
  default = "nothing here"
}
variable "GLOBALADMINS" {
  type = string
  default = "nothing here"
}
variable "VPC_CIDR_BLOCK" {
  description = "CIDR block for the VPC"
  type        = string
}
variable "PUBLIC_SUBNET_CIDR_BLOCK_AZ_A" {
  description = "CIDR block for the public subnet"
  type        = string
}
variable "PUBLIC_SUBNET_CIDR_BLOCK_AZ_B" {
  description = "CIDR block for the public subnet"
  type        = string
}
variable "PUBLIC_SUBNET_CIDR_BLOCK_AZ_C" {
  description = "CIDR block for the public subnet"
  type        = string
}
variable "CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_A" {
  description = "CIDR block for the container workloads subnet in AZ A"
  type        = string
}

variable "CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_B" {
  description = "CIDR block for the container workloads subnet in AZ B"
  type        = string
}

variable "CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_C" {
  description = "CIDR block for the container workloads subnet in AZ B"
  type        = string
}

variable "DB_SUBNET_CIDR_BLOCK_AZ_A" {
  description = "CIDR block for the DB subnet in AZ A"
  type        = string
}

variable "DB_SUBNET_CIDR_BLOCK_AZ_B" {
  description = "CIDR block for the DB subnet in AZ B"
  type        = string
}

variable "DB_SUBNET_CIDR_BLOCK_AZ_C" {
  description = "CIDR block for the DB subnet in AZ B"
  type        = string
}

variable "AVAILABILITY_ZONE_A" {
  description = "Availability Zone A"
  type        = string
}

variable "AVAILABILITY_ZONE_B" {
  description = "Availability Zone B"
  type        = string
}
variable "AVAILABILITY_ZONE_C" {
  description = "Availability Zone B"
  type        = string
}
