# Mixcore CMS - Enterprise-Grade .NET Core CMS & API Platform [![Become a Backer](https://opencollective.com/mixcore/tiers/backer.svg?avatarHeight=36)](https://opencollective.com/mixcore#support) 

[![backer](https://opencollective.com/mixcore/tiers/backer/badge.svg?label=backer&color=brightgreen)](https://opencollective.com/mixcore#support) [![Donate](https://img.shields.io/badge/$-donate-ff69b4.svg)](https://www.paypal.me/mixcore) [![Buy us a coffee](https://img.shields.io/badge/$-BuyMeACoffee-orange.svg)](https://www.buymeacoffee.com/mixcore) 

> **🚀 Next-Gen Enterprise CMS & API Platform** - Build scalable, secure, and high-performance web applications with modern .NET Core microservices architecture.

## 🎯 Key Features

- **Modern Tech Stack**: Built with .NET 8.0 and .NET 9.0, ASP.NET Core, SignalR, and GraphQL
- **Microservices Architecture**: Scalable, maintainable, and cloud-native ready
- **Multi-Tenant Support**: Perfect for SaaS applications
- **Real-time Capabilities**: Powered by SignalR for instant updates
- **API-First Approach**: RESTful APIs and GraphQL endpoints
- **Headless CMS**: Content management with flexible frontend options
- **Enterprise Security**: OAuth 2.0, OpenID Connect, and JWT support
- **Cloud-Native**: Ready for Kubernetes and Docker deployment

## 📦 Tech Stack & Versions

| Component | Version | Description |
|-----------|---------|-------------|
| .NET Core | 8.0-9.0 | Core runtime and SDK |
| ASP.NET Core | 8.0-9.0 | Web framework |
| Entity Framework Core | 8.0 | ORM and data access |
| SignalR | 8.0 | Real-time communication |
| GraphQL | 7.0 | API query language |
| Docker | Latest | Containerization |
| Kubernetes | Latest | Container orchestration |

## 🏗️ Project Structure

The project follows a modern microservices architecture with the following main components:

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

## ✨ Special Features

- [x] **Modern Architecture** - Microservices-based architecture with .NET Core
- [x] **Multi-Database Support** - MySQL, SQL Server, PostgreSQL, SQLite, ScyllaDB
- [x] **Real-time Communication** - SignalR integration
- [x] **Message Queue** - Robust message queue system
- [x] **Identity & Auth** - Comprehensive authentication and authorization
- [x] **GraphQL Support** - Modern API querying
- [x] **E-commerce Ready** - Built-in e-commerce capabilities
- [x] **Automation** - Workflow automation services
- [x] **High Performance** - Optimized for enterprise workloads
- [x] **Cross Platform** - Runs on Windows, Linux, and macOS
- [x] **Container Ready** - Docker support out of the box
- [x] **CI/CD Ready** - GitHub Actions and Azure DevOps integration

## 🚀 Quick Start

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Option 1: Docker (Single Container)
```sh
# Pull the latest image
docker pull mixcore/mix.core:latest

# Run the container
docker run -it --rm -p 5000:80 --name mixcore_cms mixcore/mix.core:latest
```

### Option 2: Docker Compose (Full Stack)
```sh
# Build and start all services
docker-compose up -d

# Access services:
# - Mixcore CMS: http://localhost:5000
# - SQL Server: localhost:1433
# - MySQL: localhost:3306
# - phpMyAdmin: http://localhost:8080
```

### Option 3: Local Development
```sh
# Clone the repository
git clone --branch develop --recursive https://github.com/mixcore/mix.core.git

# Navigate to the project directory
cd mix.core/src/Mix.Cms.Web

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Access the application at http://localhost:5000
```

### Development Environment Setup
```sh
# Install GitPod (optional)
curl -fsSL https://gitpod.io/install | sh

# Or use VS Code with Dev Containers
code --install-extension ms-vscode-remote.remote-containers
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
