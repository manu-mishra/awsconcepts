APPLICATION_NAME                    = "awsconcepts"
ENV_NAME                            = "dev"
ENV_SUBDOMAIN                       = "dev"
APEX_DOMAIN                         = "awsconcepts.com"
REGION                              = "us-east-1"
STATEBUCKET                         = "a-us1-tf-dev-mm"
AVAILABILITY_ZONE_A                 = "us-east-1a"
AVAILABILITY_ZONE_B                 = "us-east-1b"
AVAILABILITY_ZONE_C                 = "us-east-1c"

VPC_CIDR_BLOCK                      = "10.5.0.0/16"

PUBLIC_SUBNET_CIDR_BLOCK_AZ_A       = "10.5.0.0/22"    # 1024 IPs, 1021 usable
PUBLIC_SUBNET_CIDR_BLOCK_AZ_B       = "10.5.4.0/22"    # 1024 IPs, 1021 usable
PUBLIC_SUBNET_CIDR_BLOCK_AZ_C       = "10.5.8.0/22"    # 1024 IPs, 1021 usable

CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_A = "10.5.12.0/20"   # 4096 IPs, 4093 usable
CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_B = "10.5.28.0/20"   # 4096 IPs, 4093 usable
CONTAINER_WORKLOADS_CIDR_BLOCK_AZ_C = "10.5.44.0/20"   # 4096 IPs, 4093 usable

DB_SUBNET_CIDR_BLOCK_AZ_A           = "10.5.60.0/22"   # 1024 IPs, 1021 usable
DB_SUBNET_CIDR_BLOCK_AZ_B           = "10.5.64.0/22"   # 1024 IPs, 1021 usable
DB_SUBNET_CIDR_BLOCK_AZ_C           = "10.5.68.0/22"   # 1024 IPs, 1021 usable

