terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
    google = {
      source  = "hashicorp/google"
      version = "~> 5.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.0"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.0"
    }
  }
}

# AWS Provider
provider "aws" {
  region = var.aws_region
}

# Azure Provider
provider "azurerm" {
  features {}
}

# GCP Provider
provider "google" {
  project = var.gcp_project_id
  region  = var.gcp_region
}

# Kubernetes Provider
provider "kubernetes" {
  config_path = local.kubeconfig_path
}

# Helm Provider
provider "helm" {
  kubernetes {
    config_path = local.kubeconfig_path
  }
}

# Variables
variable "aws_region" {
  description = "AWS region"
  default     = "ap-southeast-1"
}

variable "gcp_project_id" {
  description = "GCP project ID"
}

variable "gcp_region" {
  description = "GCP region"
  default     = "asia-southeast1"
}

variable "cluster_name" {
  description = "Kubernetes cluster name"
  default     = "mixcore-cluster"
}

# Locals
locals {
  kubeconfig_path = var.cloud_provider == "aws" ? "~/.kube/config-eks" : 
                   var.cloud_provider == "azure" ? "~/.kube/config-aks" : 
                   "~/.kube/config-gke"
}

# AWS EKS Cluster
module "aws_eks" {
  count   = var.cloud_provider == "aws" ? 1 : 0
  source  = "terraform-aws-modules/eks/aws"
  version = "~> 19.0"

  cluster_name    = var.cluster_name
  cluster_version = "1.28"

  vpc_id     = module.vpc[0].vpc_id
  subnet_ids = module.vpc[0].private_subnets

  eks_managed_node_groups = {
    mixcore = {
      min_size     = 3
      max_size     = 5
      desired_size = 3

      instance_types = ["t3.medium"]
    }
  }
}

# Azure AKS Cluster
module "azure_aks" {
  count   = var.cloud_provider == "azure" ? 1 : 0
  source  = "Azure/aks/azurerm"
  version = "~> 7.0"

  resource_group_name = "mixcore-rg"
  cluster_name       = var.cluster_name
  kubernetes_version = "1.28.0"

  default_node_pool = {
    name       = "mixcore"
    node_count = 3
    vm_size    = "Standard_D2s_v3"
  }

  network_plugin = "azure"
  network_policy = "azure"
}

# GCP GKE Cluster
module "gcp_gke" {
  count   = var.cloud_provider == "gcp" ? 1 : 0
  source  = "terraform-google-modules/kubernetes-engine/google"
  version = "~> 30.0"

  project_id        = var.gcp_project_id
  name             = var.cluster_name
  region           = var.gcp_region
  zones            = ["${var.gcp_region}-a", "${var.gcp_region}-b"]
  network          = "mixcore-vpc"
  subnetwork       = "mixcore-subnet"
  ip_range_pods    = "mixcore-pods"
  ip_range_services = "mixcore-services"

  node_pools = [
    {
      name         = "mixcore"
      machine_type = "e2-medium"
      node_count   = 3
    }
  ]
}

# Kubernetes Resources
resource "kubernetes_namespace" "mixcore" {
  metadata {
    name = "mixcore"
  }
}

resource "kubernetes_secret" "mixcore_secrets" {
  metadata {
    name      = "mixcore-secrets"
    namespace = kubernetes_namespace.mixcore.metadata[0].name
  }

  data = {
    sqlserver-password = base64encode(var.sqlserver_password)
    mysql-password     = base64encode(var.mysql_password)
    redis-password     = base64encode(var.redis_password)
  }
}

resource "kubernetes_config_map" "mixcore_config" {
  metadata {
    name      = "mixcore-config"
    namespace = kubernetes_namespace.mixcore.metadata[0].name
  }

  data = {
    ASPNETCORE_ENVIRONMENT           = "Production"
    ASPNETCORE_URLS                 = "http://+:80;https://+:443"
    ConnectionStrings__DefaultConnection = "Server=mixcore-sqlserver;Database=mixcore;User=sa;Password=${var.sqlserver_password};"
    ConnectionStrings__MySqlConnection   = "Server=mixcore-mysql;Database=mixcore;User=root;Password=${var.mysql_password};"
    Redis__ConnectionString         = "mixcore-redis:6379"
  }
}

# Helm Releases
resource "helm_release" "mixcore" {
  name       = "mixcore"
  namespace  = kubernetes_namespace.mixcore.metadata[0].name
  repository = "https://charts.mixcore.org"
  chart      = "mixcore"
  version    = "1.0.0"

  values = [
    file("${path.module}/values.yaml")
  ]
}

# Outputs
output "cluster_endpoint" {
  value = var.cloud_provider == "aws" ? module.aws_eks[0].cluster_endpoint :
         var.cloud_provider == "azure" ? module.azure_aks[0].cluster_endpoint :
         module.gcp_gke[0].endpoint
}

output "kubeconfig" {
  value = var.cloud_provider == "aws" ? module.aws_eks[0].kubeconfig :
         var.cloud_provider == "azure" ? module.azure_aks[0].kubeconfig :
         module.gcp_gke[0].kubeconfig
} 