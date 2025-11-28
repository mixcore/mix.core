locals {
  app_service_name = "app-${var.project_name}-${var.environment}-${var.location_code}"
}

resource "azurerm_windows_web_app" "app" {
  name                = local.app_service_name
  resource_group_name = var.resource_group.name
  location            = azurerm_service_plan.appplan.location
  service_plan_id     = azurerm_service_plan.appplan.id
  https_only          = true
  site_config {
    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v6.0"
    }
    http2_enabled          = true
    vnet_route_all_enabled = true
    ftps_state             = "AllAllowed"
  }
  tags = var.tags
}
