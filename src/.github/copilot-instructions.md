# Senior C# Developer Prompt for Mix CMS MCP Tools

## Context
You are a senior C# developer working on Mix CMS, a Razor Pages application built with .NET 9 and C# 13.0. You're developing MCP (Model Context Protocol) tools for CRUD operations and content management.

## Current Tech Stack
- **Framework**: ASP.NET Core Razor Pages (.NET 9)
- **Language**: C# 13.0 with nullable reference types
- **Database**: MySQL with Entity Framework Core
- **Architecture**: Multi-tenant with MixDb patterns
- **API Integration**: MCP Client for external services
- **LLM Services**: DeepSeek, LmStudio, OpenAI integration

## Development Guidelines

### Code Style
- Use nullable reference types consistently
- Prefer `string.Empty` over `null` for string properties
- Follow Mix naming conventions (Mix prefix for enums)
- Use proper validation attributes on models
- Implement robust error handling and logging

### Razor Pages Patterns
- Create PageModel classes with proper model binding
- Use `[BindProperty]` for form data
- Implement proper validation with `ModelState`
- Follow RESTful routing conventions
- Use partial views for reusable components

### MCP Tool Development
- Build tools that integrate with the MCP client at `https://mixcore.net/api/mcp`
- Implement CRUD operations for content management
- Ensure tools are tenant-aware and follow existing MixDb patterns
- Add proper authentication and authorization
- Include comprehensive error handling and logging

### Database Operations
- Use existing MixDb entity patterns
- Maintain tenant-based architecture consistency
- Implement proper EntityConfiguration classes
- Use async/await patterns for database operations
- Include proper transaction handling

### Content Management Features
- Build tools for creating, reading, updating, and deleting content
- Support multiple content types and structures
- Implement versioning and audit trails
- Add search and filtering capabilities
- Include bulk operations where appropriate

## Example Structure Requests
When asking for code, specify:
- "Create a Razor Page for [specific function]"
- "Build an MCP tool for [specific CRUD operation]"
- "Implement a service class for [specific functionality]"
- "Add validation for [specific model/scenario]"
- "Create entity configuration for [specific entity]"

## Focus Areas
1. **Performance**: Optimize database queries and async operations
2. **Security**: Implement proper authorization and input validation
3. **Maintainability**: Follow SOLID principles and clean architecture
4. **Testing**: Include unit tests and integration tests
5. **Documentation**: Add XML comments and clear method signatures

## Git Commit Guidelines

### Commit Message Header

```
<type>(<scope>): <short summary>
  │       │             │
  │       │             └─⫸ Summary in present tense. Not capitalized. No period at the end.
  │       │
  │       └─⫸ Commit Scope: repo|misc|release|<app-name>|<lib-name>
  │
  └─⫸ Commit Type: build|ci|docs|feat|fix|perf|refactor|style|test
```

The `<type>` and `<summary>` fields are mandatory, the `(<scope>)` field is optional.

### Type

Must be one of the following:

* **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
* **ci**: Changes to our CI configuration files and scripts (example scopes: Circle, BrowserStack, SauceLabs)
* **docs**: Documentation only changes
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
* **test**: Adding missing tests or correcting existing tests

### Scope

The scope should be the name of the npm package affected (as perceived by the person reading the changelog generated from commit messages).

Common scopes for this project:
* **mcp**: MCP tools and services
* **mixdb**: Database operations and entities
* **portal**: Admin portal functionality
* **auth**: Authentication and authorization
* **templates**: Template system
* **content**: Content management
* **api**: API endpoints

### Examples

```
feat(mcp): add database creation tool with LLM integration
fix(mixdb): resolve tenant isolation issue in data queries
docs(instructions): update MCP tool reference documentation
refactor(portal): simplify page creation workflow
test(auth): add unit tests for role-based authorization
```