variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "location_code" {
  type = string
}

variable "db_user" {
  type = string
}

variable "db_pwd" {
  type = string
}

variable "resource_group" {
  type = map(any)
}

variable "tags" {
  type = map(any)
}
