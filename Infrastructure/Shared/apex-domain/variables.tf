variable "domain_name" {
  type = string
}

variable "purpose" {
  type = string
}
variable "owner" {
  type = string
}

variable "environments" {
  type = map(object({
    name = string
    subdomain = string
    dns_settings = object({
      ns_records = list(string)
    })
  }))
  default = {}
}