module "apex-domain" {
  source = "./apex-domain"
  domain_name = "awsconcepts.com"
  purpose="apex-domain-hosting"
  owner="shared infra"
  environments = {
    "prod" = {
      name = "prod",
      subdomain = "www",
      dns_settings = {
        ns_records = ["ns-561.awsdns-06.net.", "ns-2022.awsdns-60.co.uk.", "ns-420.awsdns-52.com.", "ns-1086.awsdns-07.org."]
      }
    },
    "dev" = {
      name = "dev",
      subdomain = "dev",
      dns_settings = {
        ns_records = ["ns-71.awsdns-08.com.", "ns-993.awsdns-60.net.", "ns-1614.awsdns-09.co.uk.", "ns-1105.awsdns-10.org."]
      }
    }
  }
}