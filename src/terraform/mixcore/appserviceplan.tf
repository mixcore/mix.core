locals {
  app_service_plan_name = "plan-${var.environment}-${var.environment}-${var.location_code}"
}

resource "azurerm_service_plan" "appplan" {
  name                = local.app_service_plan_name
  resource_group_name = var.resource_group.name
  location            = var.resource_group.location
  os_type             = "Windows"
  sku_name            = "B1"
  tags                = var.tags
}