# Mixcore CMS - Enterprise-Grade .NET Core CMS & API Platform [![Become a Backer](https://opencollective.com/mixcore/tiers/backer.svg?avatarHeight=36)](https://opencollective.com/mixcore#support) 

[![backer](https://opencollective.com/mixcore/tiers/backer/badge.svg?label=backer&color=brightgreen)](https://opencollective.com/mixcore#support) [![Donate](https://img.shields.io/badge/$-donate-ff69b4.svg)](https://www.paypal.me/mixcore) [![Buy us a coffee](https://img.shields.io/badge/$-BuyMeACoffee-orange.svg)](https://www.buymeacoffee.com/mixcore) 

> **🚀 Enterprise-Grade CMS & API Platform** - Build scalable, secure, and high-performance web applications with modern .NET Core microservices architecture. Perfect for agencies, enterprises, and developers building complex digital experiences.

## 🎯 Why Choose Mixcore?

### For Enterprises
- **Enterprise-Grade Security**: Built-in OAuth 2.0, OpenID Connect, and JWT support with enterprise security features
- **High Availability**: Designed for 99.99% uptime with built-in redundancy and failover
- **Scalability**: Horizontally scalable architecture to handle millions of requests
- **Compliance Ready**: GDPR, HIPAA, and SOC2 compliance features out of the box
- **Enterprise Support**: Professional support and SLAs available

### For Agencies
- **Multi-Client Support**: Built-in multi-tenant architecture for managing multiple clients
- **White-Labeling**: Complete white-labeling capabilities for agencies
- **Rapid Development**: Pre-built components and templates for faster development
- **Client Management**: Built-in client management and billing features
- **Agency Dashboard**: Centralized dashboard for managing all client projects

### For Developers
- **Modern Stack**: Latest .NET 9.0, ASP.NET Core, and modern frontend technologies
- **API-First**: RESTful APIs and GraphQL for flexible integration
- **Extensible**: Modular architecture for easy customization
- **Developer Tools**: Comprehensive SDKs and development tools
- **Active Community**: Large developer community and extensive documentation

## 🛠️ Key Features

### Core Platform
- **Modern Tech Stack**: Built with .NET 9.0, ASP.NET Core, SignalR, and GraphQL
- **Microservices Architecture**: Scalable, maintainable, and cloud-native ready
- **Multi-Tenant Support**: Perfect for SaaS applications and agencies
- **Real-time Capabilities**: Powered by SignalR for instant updates
- **API-First Approach**: RESTful APIs and GraphQL endpoints
- **Headless CMS**: Content management with flexible frontend options
- **Enterprise Security**: OAuth 2.0, OpenID Connect, and JWT support
- **Cloud-Native**: Ready for Kubernetes and Docker deployment

### Enterprise Features
- **High Availability**: Built-in redundancy and failover mechanisms
- **Scalability**: Horizontal scaling support for high traffic
- **Security**: Enterprise-grade security features and compliance tools
- **Monitoring**: Comprehensive monitoring and analytics
- **Backup & Recovery**: Automated backup and disaster recovery
- **Multi-Region Support**: Deploy across multiple regions
- **Enterprise SSO**: Support for enterprise identity providers
- **Audit Logging**: Comprehensive audit trails and logging

### Agency Features
- **Multi-Client Management**: Manage multiple clients from one dashboard
- **White-Labeling**: Custom branding and white-labeling options
- **Client Billing**: Built-in billing and invoicing
- **Template Library**: Pre-built templates and components
- **Client Analytics**: Client-specific analytics and reporting
- **Resource Management**: Team and resource management tools
- **Project Templates**: Reusable project templates
- **Client Portal**: Customizable client portals

## 📦 Tech Stack & Versions

| Component | Version | Description |
|-----------|---------|-------------|
| .NET Core | 9.0 | Core runtime and SDK |
| ASP.NET Core | 9.0 | Web framework |
| Entity Framework Core | 9.0 | ORM and data access |
| SignalR | 9.0 | Real-time communication |
| GraphQL | 7.0 | API query language |
| Docker | Latest | Containerization |
| Kubernetes | Latest | Container orchestration |
| Redis | 7.0 | Caching and message broker |
| SQL Server | 2022 | Primary database |
| MySQL | 8.0 | Secondary database |
| ScyllaDB | Latest | NoSQL database |

## 🏗️ Architecture

### Core Platform
- `platform/` - Core platform services and libraries
  - `mix.library` - Core library functionality
  - `mix.database` - Database abstraction layer
  - `mix.identity` - Identity and authentication services
  - `mix.signalr` - Real-time communication
  - `mix.quartz` - Job scheduling
  - `mix.queue` - Message queue handling
  - `mix.auth` - Authentication services
  - `mix.scylladb` - NoSQL database integration

### Modules
- `modules/` - Extensible modules
  - `mix.common` - Common utilities
  - `mix.messenger` - Messaging services
  - `mix.scheduler` - Task scheduling
  - `mix.storage` - File storage services
  - `mix.log` - Logging services
  - `mix.portal` - Portal interface

### Services
- `services/` - Microservices
  - `mix-auth-service` - Authentication service
  - `mix-message-queue` - Message queue service
  - `mix-databases` - Database services
  - `mix-ecommerce` - E-commerce services
  - `mix-graphql` - GraphQL services
  - `mix-automation` - Automation services

### Applications
- `applications/` - Main applications
  - `mixcore` - Main application
  - `mixcore.gateway` - API Gateway
  - `mixcore.host.aspire` - Aspire hosting

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Quick Start with Docker Compose
```sh
# Clone the repository
git clone --branch develop --recursive https://github.com/mixcore/mix.core.git

# Navigate to the project directory
cd mix.core

# Build and start all services
docker-compose up --build

# Access services:
# - Mixcore CMS: http://localhost:5000
# - API Gateway: http://localhost:5002
# - SQL Server: localhost:1433
# - MySQL: localhost:3306
# - Redis: localhost:6379
# - phpMyAdmin: http://localhost:8080
```

### Local Development
```sh
# Clone the repository
git clone --branch develop --recursive https://github.com/mixcore/mix.core.git

# Navigate to the project directory
cd mix.core

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project src/applications/mixcore/mixcore.csproj

# Access the application at http://localhost:5000
```

### Development Environment Setup
```sh
# Install VS Code with Dev Containers
code --install-extension ms-vscode-remote.remote-containers

# Or use Visual Studio 2022
# Download from: https://visualstudio.microsoft.com/vs/
```

## 📚 Documentation & Resources

| Resource | Link | Description |
|----------|------|-------------|
| Demo | https://demo.mixcore.org | Live demo environment |
| Documentation | https://docs.mixcore.org | Comprehensive documentation |
| API Reference | https://api.mixcore.org | API documentation |
| Community | https://community.mixcore.org | Community forum |
| YouTube | https://www.youtube.com/channel/UChqzh6JnC8HBUSQ9AWIcZAw | Tutorials & guides |
| Twitter | https://twitter.com/mixcore_cms | Latest updates |
| Medium | https://medium.com/mixcore | Technical articles |

## 💼 Enterprise Support

We offer various support plans for enterprises and agencies:

- **Standard Support**: Email support, documentation access
- **Professional Support**: 24/7 support, SLAs, dedicated account manager
- **Enterprise Support**: Custom solutions, on-site support, training
- **Agency Program**: White-labeling, multi-client management, custom features

Contact us at [enterprise@mixcore.org](mailto:enterprise@mixcore.org) for more information.

## 📄 License

Mixcore CMS is licensed under the **[MIT License](https://github.com/mixcore/mix.core/blob/master/LICENSE)**

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

For more details, see our [Contributing Guide](https://github.com/mixcore/mix.core/blob/develop/CONTRIBUTING.md)

## 💖 Support

If you find this project useful, please consider:
- [Becoming a backer](https://opencollective.com/mixcore#support)
- [Making a donation](https://www.paypal.me/mixcore)
- [Buying us a coffee](https://www.buymeacoffee.com/mixcore)

## 📊 Activity

![Alt](https://repobeats.axiom.co/api/embed/4ec425735bae424c69c063f2bac106c3107b6db4.svg "Repobeats analytics image")

## ⭐ Star History

[![Star History Chart](https://api.star-history.com/svg?repos=mixcore/mix.core&type=Date)](https://star-history.com/#mixcore/mix.core&Date)

## 👥 Contributors

<a href="https://github.com/mixcore/mix.core/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=mixcore/mix.core" />
</a>
