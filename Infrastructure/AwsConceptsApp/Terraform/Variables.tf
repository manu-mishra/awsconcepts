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
