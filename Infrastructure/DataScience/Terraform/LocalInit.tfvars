
bucket = "a-us1-tf-ds-mm"
key = "infrastructure/terraform.tfstate"
region = "us-east-1"
#use this command to set up local
# terraform init -backend-config="LocalInit.tfvars" -reconfigure

# terraform plan -var-file="../Environments/dev/Static.tfvars" -var-file="../Environments/dev/Variables.tfvars"
