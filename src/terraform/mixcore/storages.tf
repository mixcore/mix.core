locals {
  storage_account_name = "st${var.project_name}${var.environment}${var.location_code}"
}

resource "azurerm_storage_account" "st" {
  name                          = local.storage_account_name
  resource_group_name           = var.resource_group.name
  location                      = var.resource_group.location
  account_tier                  = "Standard"
  account_replication_type      = "LRS"
  account_kind                  = "StorageV2"
  enable_https_traffic_only     = true
  public_network_access_enabled = false
  network_rules {
    default_action = "Allow"
    bypass         = ["AzureServices"]
  }
  tags = var.tags
}
