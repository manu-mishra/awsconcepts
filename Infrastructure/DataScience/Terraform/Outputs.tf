# Output the number of available IP addresses
output "available_ip_count" {
  value = data.aws_subnet.sagemakerdomain_subnet_data.available_ip_address_count
}