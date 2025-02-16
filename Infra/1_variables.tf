variable "defaultRegion" {
  default = "us-east-1"
}

variable "rdsHost" {
  description = "Secret passed from GitHub Actions"
  type        = string
}

variable "brokerpassword" {
  description = "Secret passed from GitHub Actions"
  type        = string
}