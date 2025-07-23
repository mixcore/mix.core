# Template Patterns Overview

This directory contains specialized guides for each Mix CMS template type. Choose the guide that matches your template's `folderType`:

## Template Type Guides

### Core Template Types
- **[Master Layouts](./template-patterns-masters.md)** - `folderType: 7` - Site-wide layouts and structure
- **[Pages](./template-patterns-pages.md)** - `folderType: 1` - Static and dynamic page templates
- **[Modules](./template-patterns-modules.md)** - `folderType: 2` - Reusable content components
- **[Posts](./template-patterns-posts.md)** - `folderType: 5` - Blog posts and articles

### Specialized Template Types
- **[Forms](./template-patterns-forms.md)** - `folderType: 3` - User input and data collection
- **[Widgets](./template-patterns-widgets.md)** - `folderType: 6` - Small reusable UI components
- **[Documents](./template-patterns-edms.md)** - `folderType: 4` - Document management templates

---

## Quick Reference

### MCP Commands vs Template Code

**MCP Commands** are the tools you use to create and manage your site structure (like `CreateTemplate`, `CreatePageContent`, `GetListMidxDbData`). These are called through the AI assistant.

**Template Code** is the Razor/C# code you write inside your `.cshtml` templates to display data dynamically. This code uses the Mix CMS services directly.

### Template Folder Types Reference

```csharp
public enum MixTemplateFolderType
{
    Layouts = 0,    // Layout templates (deprecated, use Masters)
    Pages = 1,      // Page templates  
    Modules = 2,    // Module templates
    Forms = 3,      // Form templates
    Edms = 4,       // Document management templates
    Posts = 5,      // Post/blog templates
    Widgets = 6,    // Widget templates
    Masters = 7,    // Master layout templates
}
```

### Essential Guidelines

- **Check MCP Tool Support First:** Before executing any task, check if there's an existing MCP tool that can help accomplish it. Use `ListSections` to explore available tools and resources.
- **Master Layouts First:** Always create your `folderType: 7` Master Layout before creating pages.
- **Public Image URLs:** When using images in templates, always use full, public URLs (e.g., from Unsplash). Do not use local file paths.

---

## Common Patterns Across All Template Types

### Template Naming Conventions
- The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
- The `fileName` parameter do NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
- The `content` parameter do NOT include `Layout = "..."`
- The system will automatically combine them to create the full filename

### Partial Rendering Pattern
Always follow the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

```razor
@await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);
```

### Required Using Statements for MixDb Data
When using MixDb data in templates, always include these using statements at the top:

```razor
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@using Mix.Constant.Constants

@inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
@inject IMixDbDataServiceFactory mixDbDataServiceFactory
```

---

## Getting Started

1. **Choose Your Template Type** from the guides above
2. **Read the Specific Guide** for detailed patterns and examples
3. **Follow the MCP Commands** to create your template structure
4. **Implement Template Code** using the provided patterns
5. **Test and Debug** using the troubleshooting sections

---

## Additional Resources

- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
- **[Developer Guide](./developer-guide.md)** - Advanced C# development patterns
- **[Website Building Best Practices](./website-building-best-practices.md)** - Project methodology
- **[AI Getting Started](./ai-getting-started.md)** - Quick start guide for AI agents
