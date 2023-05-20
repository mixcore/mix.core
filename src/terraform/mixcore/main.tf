# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.29.1"
    }
  }

  # backend "azurerm" {
  # }

  required_version = ">= 1.3.0"
}

provider "azurerm" {
  features {}
}
