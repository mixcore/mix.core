# Mix CMS Developer Guide

## Overview
Technical guide for C# developers working on Mix CMS, focusing on .NET 9, Razor Pages, and MCP tool development.

## Technology Stack
- **Framework**: ASP.NET Core Razor Pages (.NET 9)
- **Language**: C# 13.0 with nullable reference types
- **Database**: MySQL with Entity Framework Core
- **Architecture**: Multi-tenant with MixDb patterns
- **API Integration**: MCP Client for external services
- **LLM Services**: DeepSeek, LmStudio, OpenAI integration

## Development Guidelines

### Code Standards
- Use nullable reference types consistently
- Prefer `string.Empty` over `null` for string properties
- Follow Mix naming conventions (Mix prefix for enums)
- Use proper validation attributes on models
- Implement robust error handling and logging

### Razor Pages Best Practices
- Create PageModel classes with proper model binding
- Use `[BindProperty]` for form data
- Implement proper validation with `ModelState`
- Follow RESTful routing conventions
- Use partial views for reusable components

### MCP Tool Development
- Build tools that integrate with the MCP client infrastructure
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

## MCP Tools Implemented

### Core Content Tools
- **MixPageContentTool**: CRUD operations for pages with SEO and layout support
- **MixTemplateTool**: Template management with file handling and theme support
- **MixPostContentTool**: Blog post management with status and categorization
- **MixModuleContentTool**: Reusable component management

### Data Management Tools
- **MixDbDataTool**: Dynamic database operations with query building
- **MixDbPromptTool**: AI-powered schema creation from natural language

### All tools feature:
- Async/await patterns with proper error handling
- Tenant-aware architecture
- Comprehensive logging and validation
- Consistent API patterns
- Integration-ready design

## Code Examples

### Basic MCP Tool Structure
```csharp
[McpServerToolType]
public class ExampleTool : BaseTool
{
    [McpServerTool, Description("Tool description")]
    public async Task<string> ExampleMethod(
        [Description("Parameter description")] string parameter)
    {
        return await ExecuteWithExceptionHandlingAsync(async (ct) =>
        {
            // Implementation
            return result;
        }, "ExampleMethod");
    }
}
```

### ViewModel Pattern
```csharp
public class ExampleViewModel : ViewModelBase<ExampleEntity>
{
    // Business logic here
    public override async Task<bool> SaveModelAsync(
        bool isSaveSubModels = false, 
        CancellationToken cancellationToken = default)
    {
        // Custom save logic
        return await base.SaveModelAsync(isSaveSubModels, cancellationToken);
    }
}
```

## Request Structure Examples

When requesting code, specify:
- "Create a Razor Page for [functionality]"
- "Build an MCP tool for [CRUD operation]"
- "Implement a service class for [business logic]"
- "Add validation for [data model/scenario]"
- "Create entity configuration for [database entity]"

## Development Focus Areas

1. **Performance**: Optimize database queries and async operations
2. **Security**: Implement proper authorization and input validation
3. **Maintainability**: Follow SOLID principles and clean architecture
4. **Testing**: Include unit tests and integration tests
5. **Documentation**: Add XML comments and clear method signatures
