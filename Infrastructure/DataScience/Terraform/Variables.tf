variable "APPLICATION_NAME" {
  description = "name of application"
  type        = string
  default = "awsconcepts"
}
variable "REGION" {
  description = "aws region"
  type        = string
  default = "us-east-1"
} 
variable "STATEBUCKET" {
  description = "terraform state bucket"
  type        = string
  default = "a-us1-tf-ds-mm"
}

variable "GIT_Hub_REPO_URL" {
  description = "URL of git hub repo"
  type        = string
  default = "https://github.com/manu-mishra/awsconcepts.git"
}

