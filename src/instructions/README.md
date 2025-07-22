# Mix CMS Instructions Directory

This directory provides focused, organized documentation for AI agents working with Mix CMS. All content is structured for maximum accessibility and task-oriented execution.

---

## 🚀 Quick Start for AI Agents

### Primary Entry Point
**[🎯 AI Workflows Complete Guide](./workflows/ai-workflows-complete.md)** - Start here for all common development tasks

### Key Navigation Paths
- **New to Mix CMS?** → [Getting Started Guide](./ai-getting-started.md)
- **Need specific patterns?** → [Template Patterns](./patterns/)
- **Technical reference?** → [Reference Documentation](./reference/)

---

## 📁 Directory Structure

### `/workflows/` - Task-Oriented Guides
- **[ai-workflows-complete.md](./workflows/ai-workflows-complete.md)** - Comprehensive workflow guide (PRIMARY REFERENCE)

### `/patterns/` - Template & Design Patterns  
- **[template-patterns-overview.md](./patterns/template-patterns-overview.md)** - Template pattern index
- **[template-patterns-masters.md](./patterns/template-patterns-masters.md)** - Master layout patterns
- **[template-patterns-pages.md](./patterns/template-patterns-pages.md)** - Page template patterns
- **[template-patterns-modules.md](./patterns/template-patterns-modules.md)** - Module patterns
- **[template-patterns-posts.md](./patterns/template-patterns-posts.md)** - Blog/post patterns
- **[template-patterns-forms.md](./patterns/template-patterns-forms.md)** - Form patterns
- **[template-patterns-widgets.md](./patterns/template-patterns-widgets.md)** - Widget patterns

### `/reference/` - Technical Documentation
- **[mcp-tools-reference.md](./reference/mcp-tools-reference.md)** - Complete MCP command reference
- **[developer-guide.md](./reference/developer-guide.md)** - C# development guidelines
- **[mix-cms-reference.md](./reference/mix-cms-reference.md)** - Mix CMS technical reference

### Root Level - Core Guides
- **[ai-getting-started.md](./ai-getting-started.md)** - Introduction and core concepts
- **[mix-ai-agent.md](./mix-ai-agent.md)** - Legacy agent guide (preserved for reference)
- **[mixdb-lessons-learned.md](./mixdb-lessons-learned.md)** - Best practices and troubleshooting
- **[website-building-best-practices.md](./website-building-best-practices.md)** - Project methodology

---

## 🎯 AI Agent Usage Recommendations

### For Task Execution
1. **Start with**: [ai-workflows-complete.md](./workflows/ai-workflows-complete.md) - Contains all common workflows
2. **Reference patterns**: Use `/patterns/` for specific template examples
3. **Technical details**: Check `/reference/` for API documentation

### For Learning & Understanding
1. **Core concepts**: [ai-getting-started.md](./ai-getting-started.md)
2. **Best practices**: [website-building-best-practices.md](./website-building-best-practices.md)
3. **Lessons learned**: [mixdb-lessons-learned.md](./mixdb-lessons-learned.md)

### For Troubleshooting
1. **Common issues**: Check the troubleshooting sections in workflow guides
2. **Technical reference**: [reference/mcp-tools-reference.md](./reference/mcp-tools-reference.md)
3. **Development issues**: [reference/developer-guide.md](./reference/developer-guide.md)

---

## 🔥 Essential Quick Reference

### Folder Types (Critical)
```csharp
Masters = 7     // Master layouts (create first)
Pages = 1       // Page templates  
Modules = 2     // Reusable modules
Posts = 5       // Blog templates
Forms = 3       // Form templates
Widgets = 6     // Widget components
```

### Template Models
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel     // Pages
@model Mixcore.Domain.ViewModels.ModuleContentViewModel   // Modules  
@model Mixcore.Domain.ViewModels.PostContentViewModel     // Posts
@model dynamic                                            // Database-driven content
```

### Essential Services
```razor
@inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
@inject IMixDbDataServiceFactory mixDbDataServiceFactory
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
