locals {
  db_name    = "mysqldb-mixcore-${var.project_name}-${var.environment}-${var.location_code}"
  dbsrv_name = "mysql-mixcore-${var.project_name}-${var.environment}-${var.location_code}"
}

resource "azurerm_mysql_server" "dbsrv" {
  name                = local.dbsrv_name
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name

  administrator_login          = var.db_user
  administrator_login_password = var.db_pwd

  sku_name   = "B_Gen5_2"
  storage_mb = 5120
  version    = "8.0"

  auto_grow_enabled                 = true
  backup_retention_days             = 7
  geo_redundant_backup_enabled      = false
  infrastructure_encryption_enabled = false
  public_network_access_enabled     = true
  ssl_enforcement_enabled           = true
  ssl_minimal_tls_version_enforced  = "TLS1_2"
}

resource "azurerm_mysql_database" "db" {
  name                = local.db_name
  resource_group_name = var.resource_group.name
  server_name         = azurerm_mysql_server.dbsrv.name
  charset             = "utf8"
  collation           = "utf8_unicode_ci"
}