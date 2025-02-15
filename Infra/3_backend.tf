terraform {
  backend "s3" {
    bucket = "tc-tf-pedidos"
    key    = "backend/terraform.tfstate"
    region = "us-east-1"
  }
}