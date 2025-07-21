# Mix CMS Instructions Directory

This directory contains comprehensive documentation for working with Mix CMS across different use cases and audiences.

## File Organization

### 🎯 **QUICK START - Essential Patterns**

**Key Service Usage (Critical for Success):**
- Use `IMixDbDataServiceFactory` with dependency injection for data access
- Access fields with typed methods: `@item.Value<string>("fieldName")`
- Include required namespaces and inject services
- Use `SearchMixDbRequestModel` for data queries
- **Images:** Always use full, public URLs (e.g., `https://images.unsplash.com/photo-...`) never local paths
- **Documentation:** After each successful task, document it in markdown files for team collaboration

**Working Template Pattern (Based on Real Examples):**
```razor
@model dynamic  <!-- Use for database-driven templates -->
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@using Mix.Constant.Constants

@inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
@inject IMixDbDataServiceFactory mixDbDataServiceFactory

@{
    var mixDbDataService = mixDbDataServiceFactory.Create(dbSrv.DatabaseProvider, dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION));
    var request = new SearchMixDbRequestModel
    {
        TableName = "your_table_name",
        Queries = new List<MixQueryField>()
    };
    var data = await mixDbDataService.GetListByAsync(request);
}

@foreach (var item in data)
{
    <div>@item.Value<string>("fieldName")</div>
}
```

**Content Template Pattern (For Pages/Posts/Modules):**
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel  <!-- Use appropriate ViewModel -->

<div class="content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)
</div>
```

---

### 🤖 For AI Agents & Content Creators

#### Getting Started
- **[ai-getting-started.md](./ai-getting-started.md)** - Start here! Core concepts and overview of Mix CMS
  - Understanding Templates vs Content
  - Overview of MCP commands
  - Quick start checklist

#### Step-by-Step Workflows
- **[ai-workflows-basic-pages.md](./ai-workflows-basic-pages.md)** - Creating webpages, templates, and layouts
  - Master layouts and page templates
  - Page content creation
  - Template naming best practices

- **[ai-workflows-dynamic-data.md](./ai-workflows-dynamic-data.md)** - Working with databases and dynamic content
  - Creating database tables
  - Module templates with data
  - Query patterns and data display

- **[ai-workflows-posts.md](./ai-workflows-posts.md)** - Blog posts and article management
  - Post templates and content
  - SEO and content structure
  - Post status management

#### Advanced Patterns
- **[ai-template-patterns.md](./ai-template-patterns.md)** - Template patterns, troubleshooting, and best practices
  - Template code vs MCP commands
  - Rendering patterns
  - Security and performance

### 👨‍💻 For Developers
- **[developer-guide.md](./developer-guide.md)** - Technical guide for C# developers
  - .NET 9 and C# 13 patterns
  - Razor Pages development
  - MCP tool development
  - Database patterns
  - Code style guidelines

### 📋 Quick References
- **[mix-cms-reference.md](./mix-cms-reference.md)** - Mix CMS enums, constants, and technical reference
  - Template folder types and their numeric values
  - Content status codes and query operators
  - Database field types and naming conventions
  - Usage examples and best practices

- **[mcp-tools-reference.md](./mcp-tools-reference.md)** - Consolidated MCP tool documentation
  - Complete tool inventory
  - Parameter specifications
  - Response formats
  - Usage examples

- **[website-building-best-practices.md](./website-building-best-practices.md)** - Project workflow and methodology
  - Phased development approach
  - Dynamic content strategies
  - Team collaboration guidelines

## Getting Started

### 🚀 **New to Mix CMS?** 
Start with [ai-getting-started.md](./ai-getting-started.md) to understand the core concepts

### 📝 **Ready to Build?**
Follow the workflow guides in order:
1. [Basic Pages](./ai-workflows-basic-pages.md) - Learn page and template creation
2. [Dynamic Data](./ai-workflows-dynamic-data.md) - Add database-driven content  
3. [Blog Posts](./ai-workflows-posts.md) - Create blog functionality
4. [Template Patterns](./ai-template-patterns.md) - Master advanced techniques

### 🛠️ **Developing Tools?** 
Check [developer-guide.md](./developer-guide.md) for technical implementation

### 🔍 **Need Quick Reference?** 
- **Technical reference:** [mix-cms-reference.md](./mix-cms-reference.md) for enums, constants, and technical details
- **Tool parameters:** [mcp-tools-reference.md](./mcp-tools-reference.md) for specific tool parameters

## Documentation Structure Benefits

### Focused Learning Path
- **Beginner-friendly progression** from concepts to implementation
- **Specialized guides** for different content types
- **Advanced patterns** separated from basic workflows

### Easy Navigation  
- **Topic-specific files** for faster reference
- **Cross-references** between related concepts
- **Progressive complexity** from simple to advanced

### Maintainable Documentation
- **Modular structure** makes updates easier
- **Single responsibility** per file
- **Consistent formatting** across all guides

## Legacy Documentation

The original comprehensive guide has been restructured into focused, topic-specific files. This new organization provides:
- Better discoverability of specific topics
- Reduced cognitive load per document
- Easier maintenance and updates
- Clearer learning progression
