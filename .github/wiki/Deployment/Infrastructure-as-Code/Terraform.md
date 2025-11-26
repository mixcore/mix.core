# Deploying with Terraform

Terraform is an infrastructure as code tool that allows you to define and provision infrastructure across multiple cloud providers. This guide will help you deploy Mixcore using Terraform.

## Prerequisites

- [Terraform](https://www.terraform.io/downloads.html) installed
- Cloud provider CLI tools installed:
  - [AWS CLI](https://aws.amazon.com/cli/) for AWS
  - [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) for Azure
  - [gcloud CLI](https://cloud.google.com/sdk/docs/install) for GCP
- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed
- [Helm](https://helm.sh/docs/intro/install/) installed

## Configuration

### Cloud Provider Setup

#### AWS
```sh
# Configure AWS credentials
export AWS_ACCESS_KEY_ID=your_access_key
export AWS_SECRET_ACCESS_KEY=your_secret_key
```

#### Azure
```sh
# Login to Azure
az login

# Set environment variables
export ARM_SUBSCRIPTION_ID=your_subscription_id
export ARM_TENANT_ID=your_tenant_id
export ARM_CLIENT_ID=your_client_id
export ARM_CLIENT_SECRET=your_client_secret
```

#### GCP
```sh
# Login to GCP
gcloud auth login
gcloud auth application-default login

# Set project
export GOOGLE_PROJECT=your_project_id
```

### Terraform Configuration

1. Create a `terraform.tfvars` file:
```hcl
cloud_provider = "aws"  # or "azure" or "gcp"
cluster_name = "mixcore-cluster"
sqlserver_password = "your_password"
mysql_password = "your_password"
redis_password = "your_password"
```

2. Review and customize `values.yaml` for Helm configuration:
```yaml
# See cloud/opentofu/values.yaml for configuration options
```

## Deployment

1. Initialize Terraform:
```sh
cd cloud/opentofu
terraform init
```

2. Plan the deployment:
```sh
terraform plan
```

3. Apply the deployment:
```sh
terraform apply
```

## Accessing the Application

1. Get the cluster endpoint:
```sh
terraform output cluster_endpoint
```

2. Get the kubeconfig:
```sh
terraform output kubeconfig > kubeconfig.yaml
export KUBECONFIG=kubeconfig.yaml
```

3. Access services:
- Main application: http://<load-balancer-ip>
- SQL Server: <load-balancer-ip>:1433
- MySQL: <load-balancer-ip>:3306
- Redis: <load-balancer-ip>:6379

## Updating Configuration

1. Edit `values.yaml` for Helm configuration:
```sh
nano cloud/opentofu/values.yaml
```

2. Apply changes:
```sh
terraform apply
```

## Destroying Resources

To destroy all resources:
```sh
terraform destroy
```

## CI/CD Integration

### GitHub Actions
```yaml
deploy-infra:
  needs: build-and-push
  runs-on: ubuntu-latest
  steps:
    - uses: actions/checkout@v3
    
    - name: Setup Terraform
      uses: hashicorp/setup-terraform@v2
      with:
        terraform_version: "1.5.0"
        
    - name: Deploy Infrastructure
      run: |
        cd cloud/opentofu
        terraform init
        terraform apply -auto-approve
```

### GitLab CI/CD
```yaml
deploy-infra:
  stage: deploy
  image:
    name: hashicorp/terraform:light
    entrypoint: [""]
  script:
    - cd cloud/opentofu
    - terraform init
    - terraform apply -auto-approve
  only:
    - main
```

## Troubleshooting

### Common Issues

1. **Authentication Errors**
   - Verify cloud provider credentials
   - Check environment variables
   - Ensure CLI tools are properly configured

2. **Resource Creation Failures**
   - Check resource quotas
   - Verify network configurations
   - Review cloud provider logs

3. **Kubernetes Connection Issues**
   - Verify kubeconfig
   - Check cluster status
   - Ensure proper RBAC permissions

### Logs and Monitoring

1. **Terraform Logs**
```sh
# Enable debug logging
export TF_LOG=DEBUG
terraform apply
```

2. **Kubernetes Logs**
```sh
# View pod logs
kubectl logs -f deployment/mixcore -n mixcore

# View resource usage
kubectl top pods -n mixcore
```

## Best Practices

1. **State Management**
   - Use remote state storage
   - Enable state locking
   - Regular state backups

2. **Security**
   - Use secrets management
   - Implement least privilege
   - Regular security audits

3. **Maintenance**
   - Regular updates
   - Backup procedures
   - Disaster recovery planning

## Additional Resources

- [Terraform Documentation](https://www.terraform.io/docs)
- [Kubernetes Documentation](https://kubernetes.io/docs)
- [Helm Documentation](https://helm.sh/docs)
- [Cloud Provider Documentation](https://docs.aws.amazon.com/index.html) 