# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Mixcore CMS is an enterprise-grade .NET 9.0 CMS & API platform with modular monolith architecture that's microservices-ready. It supports multi-tenancy, real-time SignalR capabilities, GraphQL, and multiple database providers.

## Common Commands

```bash
# Build solution
dotnet build src/Mixcore.sln -c Release

# Run tests with coverage
dotnet test src/Mixcore.sln --no-build --configuration Release --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test src/Mixcore.sln --filter "FullyQualifiedName~ProductServiceTests"

# Run main application
dotnet run --project src/applications/mixcore/mixcore.csproj

# Docker development stack (includes MySQL, Redis, PhpMyAdmin)
docker-compose up --build

# Restore dependencies
dotnet restore src/Mixcore.sln
```

**Local ports:** App: 5000/5001, MySQL: 3306, Redis: 6379, PhpMyAdmin: 8080

## Architecture

### Directory Structure

```
src/
├── applications/
│   ├── mixcore/              # Main web application entry point
│   ├── mixcore.spa/          # SPA with React/Angular
│   └── mixcore.gateway/      # API Gateway (Ocelot)
├── modules/                  # Feature modules (self-contained)
│   ├── mix.account/          # User account management
│   ├── mix.portal/           # Admin portal
│   ├── mix.scheduler/        # Task scheduling (Quartz.NET)
│   ├── mix.messenger/        # Messaging/notifications
│   ├── mix.storage/          # File storage
│   └── mix.tenancy/          # Multi-tenancy support
├── platform/                 # Foundation/infrastructure
│   ├── core/mix-heart/       # Core library (git submodule)
│   ├── mix.auth/             # Authentication
│   ├── mix.database/         # Database abstractions
│   ├── mix.mixdb/            # MixDB custom database
│   ├── mix.signalr/          # SignalR integration
│   └── mix.queue/            # Message queue abstraction
└── services/                 # Microservice implementations
    ├── core/ecommerces/      # E-commerce services
    ├── core/graphql/         # GraphQL layer
    └── mix.automation/       # Workflow automation
```

### Key Patterns

- **Request Flow:** Client → API Gateway → Authentication → Authorization → Controller → Service → Repository → Database
- **Module Independence:** Modules can be extracted as standalone microservices by removing references, copying MixContent folder, and updating ocelot.json
- **Multi-Database:** Supports MySQL, SQL Server, PostgreSQL, SQLite via configuration
- **Git Submodule:** `mix-heart` in `/src/platform/core/mix-heart/` requires `--recursive` clone

## Code Style

- PascalCase for classes/methods/public members; camelCase for local variables/private fields; UPPERCASE for constants
- Interface prefix: "I" (e.g., `IUserService`)
- Use C# 10+ features: records, pattern matching, null-coalescing
- Async/await for I/O-bound operations
- Repository pattern with Entity Framework Core
- Dependency Injection throughout

## Testing

- Framework: xUnit with Moq and FluentAssertions
- Test projects: `mix.mixdb.Tests/`, `mix.xunittest/`
- Naming convention: `MethodName_Scenario_ExpectedResult`
- Integration tests use WebApplicationFactory

## Technology Stack

- .NET 9.0 / ASP.NET Core 9.0 / Entity Framework Core 9.0
- SignalR 9.0, GraphQL 7.0, gRPC 2.70.0
- Redis 7.0 (caching), Quartz.NET (scheduling)
- JWT/OAuth 2.0 authentication
