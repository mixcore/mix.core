@description('The name of the Managed Cluster resource.')
param clusterName string = 'mixcore-cluster'

@description('The location for all resources.')
param location string = resourceGroup().location

@description('The name of the resource group containing the VNet.')
param vnetResourceGroupName string = resourceGroup().name

@description('The name of the VNet.')
param vnetName string = 'mixcore-vnet'

@description('The name of the subnet.')
param subnetName string = 'mixcore-subnet'

@description('The CIDR prefix for the VNet.')
param vnetPrefix string = '10.0.0.0/16'

@description('The CIDR prefix for the subnet.')
param subnetPrefix string = '10.0.0.0/22'

@description('The size of the VM.')
param vmSize string = 'Standard_D2s_v3'

@description('The number of nodes in the node pool.')
param nodeCount int = 3

@description('The version of Kubernetes.')
param kubernetesVersion string = '1.28.0'

resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetPrefix
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: subnetPrefix
        }
      }
    ]
  }
}

resource aks 'Microsoft.ContainerService/managedClusters@2021-05-01' = {
  name: clusterName
  location: location
  properties: {
    kubernetesVersion: kubernetesVersion
    dnsPrefix: clusterName
    agentPoolProfiles: [
      {
        name: 'agentpool'
        count: nodeCount
        vmSize: vmSize
        osType: 'Linux'
        mode: 'System'
        vnetSubnetID: vnet.properties.subnets[0].id
      }
    ]
    networkProfile: {
      networkPlugin: 'azure'
      serviceCidr: '10.2.0.0/16'
      dnsServiceIP: '10.2.0.10'
      dockerBridgeCidr: '172.17.0.1/16'
    }
    servicePrincipalProfile: {
      clientId: subscription().servicePrincipalProfile.clientId
      secret: subscription().servicePrincipalProfile.secret
    }
  }
}

output clusterName string = aks.name
output clusterFqdn string = aks.properties.fqdn
output clusterNodeResourceGroup string = aks.properties.nodeResourceGroup 