# DigitalOcean App Platform Deployment

This directory contains deployment configurations and documentation for deploying Mixcore CMS on DigitalOcean App Platform.

## Quick Deploy

[![Deploy to DO](https://www.deploytodo.com/do-btn-blue.svg)](https://cloud.digitalocean.com/apps/new?repo=https://github.com/mixcore/mix.core/tree/develop)

## What's Included

The one-click deployment includes:

- **Mixcore CMS Web Application** (Docker-based .NET 9.0)
- **Managed MySQL Database** (Version 8.0)
- **Managed Redis Cache** (Version 7.0)
- **Automatic Health Checks**
- **Load Balancing & Auto-scaling**

## Configuration

The deployment is configured via the `/.do/deploy.template.yaml` file in the repository root.

### Resources Provisioned

- **Web Service**: 1 vCPU, 1GB RAM (scalable)
- **MySQL Database**: 1 vCPU, 1GB RAM
- **Redis Cache**: 1 vCPU, 1GB RAM

### Environment Variables

The following environment variables are automatically configured:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://+:80`
- `ConnectionStrings__MixDbContext` (MySQL connection)
- `Redis__ConnectionString` (Redis connection)
- `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`

## Cost Estimation

Based on DigitalOcean's current pricing (as of 2024):

- Web Service (1 vCPU, 1GB): ~$12/month
- MySQL Database (1 vCPU, 1GB): ~$15/month  
- Redis Cache (1 vCPU, 1GB): ~$15/month

**Total**: ~$42/month for production deployment

## Scaling

The application can be scaled by:

1. Increasing instance count (horizontal scaling)
2. Upgrading instance size (vertical scaling)
3. Upgrading database resources

## Custom Deployment

For custom deployments, you can:

1. Fork the repository
2. Modify the `.do/deploy.template.yaml` file
3. Commit your changes
4. Deploy using your forked repository URL

## Support

For deployment issues:
- Check DigitalOcean App Platform [documentation](https://docs.digitalocean.com/products/app-platform/)
- Review application logs in the DigitalOcean dashboard
- Contact Mixcore support at [enterprise@mixcore.org](mailto:enterprise@mixcore.org)