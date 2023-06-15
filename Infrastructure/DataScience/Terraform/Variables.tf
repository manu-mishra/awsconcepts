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

variable "Is_Ecr_Embeddings_Container_Image_Ready" {
  description = "if the ecr image is available for endpoints"
  type        = bool
  default = true
}

variable "Git_Hub_Thumbprint_List" {
  description = "(Optional) A list of server certificate thumbprints for the OpenID Connect (OIDC) identity provider's server certificate(s)."
  type        = list(string)
  default     = ["6938fd4d98bab03faadb97b34396831e3780aea1"]
}