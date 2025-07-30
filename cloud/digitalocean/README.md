# DigitalOcean App Platform Deployment

This directory contains deployment configurations and documentation for deploying Mixcore CMS on DigitalOcean App Platform with multiple configuration options.

## 🚀 Quick Deploy Options

Choose the configuration that best fits your needs:

### Standard Configuration (~$42/month) - **Recommended**
[![Deploy Standard](https://www.deploytodo.com/do-btn-blue.svg)](https://cloud.digitalocean.com/apps/new?repo=https://github.com/mixcore/mix.core&refcode=4d26c2aaade2)
- **Database**: MySQL 8.0
- **Cache**: Redis 7.0
- **Best for**: Production websites, e-commerce
- **Performance**: Excellent for most use cases

### Alternative Configurations

For other configurations, use the manual deployment method:

#### Quick Deploy Instructions for Alternative Configurations

**For Basic Configuration (~$27/month):**
1. Fork this repository: [Fork mixcore/mix.core](https://github.com/mixcore/mix.core/fork)
2. In your fork, replace `.do/app.yaml` with `.do/deploy-basic.yaml` content
3. Deploy: [![Deploy](https://www.deploytodo.com/do-btn-blue-ghost.svg)](https://cloud.digitalocean.com/apps/new?refcode=4d26c2aaade2)

**For Development Configuration (~$32/month):**
1. Fork this repository: [Fork mixcore/mix.core](https://github.com/mixcore/mix.core/fork)
2. In your fork, replace `.do/app.yaml` with `.do/deploy-dev.yaml` content
3. Deploy: [![Deploy](https://www.deploytodo.com/do-btn-blue-ghost.svg)](https://cloud.digitalocean.com/apps/new?refcode=4d26c2aaade2)

**For PostgreSQL Configuration (~$42/month):**
1. Fork this repository: [Fork mixcore/mix.core](https://github.com/mixcore/mix.core/fork)
2. In your fork, replace `.do/app.yaml` with `.do/deploy-postgresql.yaml` content
3. Deploy: [![Deploy](https://www.deploytodo.com/do-btn-blue-ghost.svg)](https://cloud.digitalocean.com/apps/new?refcode=4d26c2aaade2)

**For Production Configuration (~$84/month):**
1. Fork this repository: [Fork mixcore/mix.core](https://github.com/mixcore/mix.core/fork)
2. In your fork, replace `.do/app.yaml` with `.do/deploy-production.yaml` content
3. Deploy: [![Deploy](https://www.deploytodo.com/do-btn-blue-ghost.svg)](https://cloud.digitalocean.com/apps/new?refcode=4d26c2aaade2)

### Configuration Details

#### Basic Configuration (~$27/month)
- **Database**: MySQL 8.0
- **Cache**: None
- **Best for**: Small websites, blogs, testing
- **Performance**: Good for low-medium traffic

#### Development Configuration (~$32/month)
- **Database**: MySQL 8.0
- **Cache**: Redis 7.0
- **Best for**: Development, staging environments
- **Performance**: Optimized for development workflows

#### PostgreSQL Configuration (~$42/month)
- **Database**: PostgreSQL 15
- **Cache**: Redis 7.0
- **Best for**: Advanced SQL features, analytics
- **Performance**: Great for complex queries

#### Production Configuration (~$84/month)
- **Database**: PostgreSQL 15 (2 vCPU, 4GB RAM)
- **Cache**: Redis 7.0 (1 vCPU, 2GB RAM)
- **Best for**: High-traffic websites, enterprise
- **Performance**: Maximum performance with auto-scaling

## 📊 Configuration Comparison

| Feature | Basic | Development | Standard | PostgreSQL | Production |
|---------|-------|-------------|----------|------------|------------|
| **Web App** | 1 vCPU, 1GB | 1 vCPU, 512MB | 1 vCPU, 1GB | 1 vCPU, 1GB | 2×2 vCPU, 4GB |
| **Database** | MySQL 8.0 | MySQL 8.0 | MySQL 8.0 | PostgreSQL 15 | PostgreSQL 15 |
| **Database Size** | 1 vCPU, 1GB | 1 vCPU, 1GB | 1 vCPU, 1GB | 1 vCPU, 1GB | 2 vCPU, 4GB |
| **Cache** | ❌ None | ✅ Redis 7.0 | ✅ Redis 7.0 | ✅ Redis 7.0 | ✅ Redis 7.0 |
| **Cache Size** | - | 1 vCPU, 1GB | 1 vCPU, 1GB | 1 vCPU, 1GB | 1 vCPU, 2GB |
| **Auto-scaling** | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Load Balancing** | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Health Checks** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Est. Cost/Month** | ~$27 | ~$32 | ~$42 | ~$42 | ~$84 |

## 🎯 Database Choice: MySQL vs PostgreSQL

### MySQL 8.0 (Basic, Development, Standard)
**Pros:**
- ✅ Excellent performance for web applications
- ✅ Mature ecosystem and tooling
- ✅ Great for content management systems
- ✅ Simpler administration
- ✅ Better compatibility with many CMSs

**Cons:**
- ❌ Limited advanced SQL features
- ❌ Less flexible with complex data types
- ❌ Fewer analytical capabilities

**Best for:** Most websites, blogs, e-commerce sites, traditional CMS usage

### PostgreSQL 15 (PostgreSQL, Production)
**Pros:**
- ✅ Advanced SQL features (JSON, arrays, custom types)
- ✅ Better performance for complex queries
- ✅ Excellent for analytics and reporting
- ✅ More flexible data modeling
- ✅ Better concurrent write performance

**Cons:**
- ❌ Slightly more complex to administer
- ❌ Larger memory footprint
- ❌ May be overkill for simple websites

**Best for:** Enterprise applications, analytics, complex data relationships, APIs

## 🧠 Redis Cache Benefits

**With Redis (Development, Standard, PostgreSQL, Production):**
- ⚡ 10-100x faster data access
- 📈 Reduced database load
- 🚀 Better user experience
- 🔄 Session management
- 📊 Real-time features support

**Without Redis (Basic):**
- 💰 Lower cost
- 🔧 Simpler setup
- ⚠️ All data queries hit the database
- ⚠️ May be slower under load

## ⚙️ Configuration Details

All deployment templates are configured via YAML files in the `/.do/` directory:

- **`.do/deploy-basic.yaml`** - Basic configuration
- **`.do/deploy-dev.yaml`** - Development configuration  
- **`.do/deploy-standard.yaml`** - Standard configuration (recommended)
- **`.do/deploy-postgresql.yaml`** - PostgreSQL configuration
- **`.do/deploy-production.yaml`** - Production configuration

### Common Environment Variables

All configurations include these environment variables:

- `ASPNETCORE_ENVIRONMENT` (Development/Production)
- `ASPNETCORE_URLS=http://+:80`
- `ConnectionStrings__MixDbContext` (Database connection)
- `Redis__ConnectionString` (Redis connection, where applicable)
- `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`
- `DatabaseProvider` (MYSQL/POSTGRESQL, where applicable)

## 💰 Detailed Cost Breakdown

### Basic Configuration (~$27/month)
- Web Service (1 vCPU, 1GB): ~$12/month
- MySQL Database (1 vCPU, 1GB): ~$15/month
- **Total: ~$27/month**

### Development Configuration (~$32/month)
- Web Service (1 vCPU, 512MB): ~$5/month
- MySQL Database (1 vCPU, 1GB): ~$15/month
- Redis Cache (1 vCPU, 1GB): ~$12/month
- **Total: ~$32/month**

### Standard Configuration (~$42/month)
- Web Service (1 vCPU, 1GB): ~$12/month
- MySQL Database (1 vCPU, 1GB): ~$15/month
- Redis Cache (1 vCPU, 1GB): ~$15/month
- **Total: ~$42/month**

### PostgreSQL Configuration (~$42/month)
- Web Service (1 vCPU, 1GB): ~$12/month
- PostgreSQL Database (1 vCPU, 1GB): ~$15/month
- Redis Cache (1 vCPU, 1GB): ~$15/month
- **Total: ~$42/month**

### Production Configuration (~$84/month)
- Web Service (2×2 vCPU, 4GB): ~$48/month
- PostgreSQL Database (2 vCPU, 4GB): ~$24/month
- Redis Cache (1 vCPU, 2GB): ~$12/month
- **Total: ~$84/month**

*Prices based on DigitalOcean's current pricing as of 2024 and may vary*

## 📈 Scaling & Performance

### Automatic Scaling (Standard, PostgreSQL, Production)

The application can automatically scale based on:
- CPU usage
- Memory usage  
- Request volume
- Response times

### Manual Scaling Options

You can scale by:

1. **Horizontal Scaling**: Increase instance count
2. **Vertical Scaling**: Upgrade instance size
3. **Database Scaling**: Upgrade database resources
4. **Cache Scaling**: Upgrade Redis resources

### Performance Recommendations

- **Start with Standard** for most production sites
- **Use PostgreSQL** for complex queries or analytics
- **Upgrade to Production** when you reach 1000+ concurrent users
- **Monitor metrics** in DigitalOcean dashboard for optimization

## 🔧 Custom Deployment

For custom deployments:

1. Fork the repository
2. Modify the desired `.do/deploy-*.yaml` file
3. Adjust instance sizes, regions, or environment variables
4. Commit your changes
5. Deploy using your forked repository URL

Example customizations:
- Change region (nyc1, sfo3, fra1, sgp1, etc.)
- Adjust instance sizes
- Add custom environment variables
- Configure different database versions

## Support

For deployment issues:
- Check DigitalOcean App Platform [documentation](https://docs.digitalocean.com/products/app-platform/)
- Review application logs in the DigitalOcean dashboard
- Contact Mixcore support at [enterprise@mixcore.org](mailto:enterprise@mixcore.org)