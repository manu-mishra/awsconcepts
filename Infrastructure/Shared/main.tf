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
        ns_records = ["ns-332.awsdns-41.com.", "ns-1387.awsdns-45.org.", "ns-2044.awsdns-63.co.uk.", "ns-878.awsdns-45.net."]
      }
    },
    "dev" = {
      name = "dev",
      subdomain = "dev",
      dns_settings = {
        ns_records = ["ns-1094.awsdns-08.org.", "ns-68.awsdns-08.com.", "ns-863.awsdns-43.net.", "ns-1980.awsdns-55.co.uk."]
      }
    }
  }
}