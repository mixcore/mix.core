locals {
  cdn_name      = "CDN${var.project_name}"
  cdn_host_name = "cdn-${var.project_name}.azureedge.net"
}
resource "azurerm_cdn_profile" "example" {
  name                = local.cdn_name
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  sku                 = "Standard_Verizon"
}

resource "azurerm_cdn_endpoint" "example" {
  name                = var.project_name
  profile_name        = azurerm_cdn_profile.example.name
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name

  origin {
    name      = "${azurerm_storage_account.st.name}"
    host_name = "${azurerm_storage_account.st.name}.blob.core.windows.net"
  }
}