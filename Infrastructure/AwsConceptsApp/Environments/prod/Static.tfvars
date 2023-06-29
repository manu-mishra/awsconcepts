APPLICATION_NAME                    = "awsconcepts"
ENV_NAME                            = "prod"
ENV_SUBDOMAIN                       = "www"
APEX_DOMAIN                         = "awsconcepts.com"
REGION                              = "us-east-1"
STATEBUCKET                         = "a-us1-tf-prod-mm"
AVAILABILITY_ZONE_A                 = "us-east-1a"
AVAILABILITY_ZONE_B                 = "us-east-1b"
VPC_CIDR_BLOCK                      = "10.6.0.0/16"
CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_A = "10.6.0.0/23"    # Allows IP range from 10.6.0.0 to 10.6.1.255 (512 IP addresses, 508 usable)
CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_B = "10.6.2.0/23"    # Allows IP range from 10.6.2.0 to 10.6.3.255 (512 IP addresses, 508 usable)
DB_SUBNET_CIDR_BLOCK_AZ_A           = "10.6.4.0/27"    # Allows IP range from 10.6.4.0 to 10.6.4.31 (32 IP addresses, 27 usable)
DB_SUBNET_CIDR_BLOCK_AZ_B           = "10.6.4.32/27"   # Allows IP range from 10.6.4.32 to 10.6.4.63 (32 IP addresses, 27 usable)


