# Changelog

All notable changes to Mixcore CMS will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Enhanced `.editorconfig` with comprehensive C# formatting and naming rules
- `Directory.Build.props` for centralized build configuration
- `.gitattributes` for consistent line endings across platforms
- `.env.example` template for environment configuration
- `CODEOWNERS` file for automated code review assignments
- `global.json` to pin .NET SDK version
- This `CHANGELOG.md` file to track project changes
- Pull request template with checklist
- Enhanced issue templates with YAML forms

### Changed
- Updated `docker-compose.yml` to use environment variables instead of hardcoded credentials
- Updated `SECURITY.md` with comprehensive security policy and reporting guidelines
- Fixed placeholder email in `CONTRIBUTING.md`

### Removed
- Removed accidental `nul` file from repository root
- Removed duplicate `mix.core.git` folder

### Security
- Removed hardcoded passwords from `docker-compose.yml`
- Added security best practices documentation to `SECURITY.md`

---

## [2.0.0] - YYYY-MM-DD

### Added
- .NET 9.0 support
- Aspire integration for cloud-native development
- Multi-tenancy support
- GraphQL API layer
- gRPC services
- ScyllaDB support for high-performance scenarios
- Real-time capabilities with SignalR 9.0
- Workflow automation module
- Enhanced e-commerce services

### Changed
- Upgraded from .NET 8.0 to .NET 9.0
- Upgraded Entity Framework Core to 9.0
- Upgraded all NuGet packages to latest versions
- Improved modular architecture for microservices readiness

### Deprecated
- Legacy REST API endpoints (use v2 API instead)

### Security
- Enhanced JWT authentication
- OAuth 2.0 / OpenID Connect improvements
- Rate limiting implementation

---

## [1.0.0] - YYYY-MM-DD

### Added
- Initial release of Mixcore CMS
- .NET 8.0 support
- Multi-database support (MySQL, SQL Server, PostgreSQL, SQLite)
- REST API with Swagger documentation
- Redis caching integration
- Quartz.NET task scheduling
- Docker support
- Kubernetes deployment configurations
- CI/CD pipelines for GitHub Actions

---

## Version History Summary

| Version | .NET | Release Date | Status |
|---------|------|--------------|--------|
| 2.x | .NET 9.0 | Current | Supported |
| 1.x | .NET 8.0 | - | Supported |
| < 1.0 | .NET 7.0 | - | End of Life |

---

## How to Update

To update to the latest version:

```bash
# Pull latest changes
git pull origin master

# Update submodules
git submodule update --init --recursive

# Restore packages
dotnet restore src/Mixcore.sln

# Build
dotnet build src/Mixcore.sln -c Release
```

## Links

- [GitHub Releases](https://github.com/mixcore/mix.core/releases)
- [Migration Guide](https://github.com/mixcore/mix.core/wiki/Migration-Guide)
- [Breaking Changes](https://github.com/mixcore/mix.core/wiki/Breaking-Changes)

[Unreleased]: https://github.com/mixcore/mix.core/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/mixcore/mix.core/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/mixcore/mix.core/releases/tag/v1.0.0
