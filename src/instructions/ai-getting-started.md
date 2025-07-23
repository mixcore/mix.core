# Mix CMS AI Agent: Getting Started Guide

Welcome! You're working with an AI assistant designed to help you build and manage websites using Mix CMS. This guide will walk you through the essential concepts and tools you'll need.

---

## Core Concepts: Building Blocks of Your Website

Your website is made of two main things: **Templates** (the design and layout) and **Content** (the text and images).

### Templates: The Blueprint for Your Pages

Templates define how your content looks. Think of them as reusable blueprints. We have different types for different jobs:

-   **Master Layouts (`folderType: 7`):** The main skeleton of your site. This is where your site-wide header, footer, and navigation live. **Every page needs one.**
-   **Page Templates (`folderType: 1`):** The layout for a specific type of page, like a blog post or a contact page. It defines the content area within the Master Layout.
-   **Modules (`folderType: 2`):** Reusable blocks of content, like a contact form or an image gallery that can be placed on any page.

#### Template Folder Types Reference

The `folderType` parameter corresponds to the `MixTemplateFolderType` enum:

```csharp
public enum MixTemplateFolderType
{
    Layouts = 0,    // Layout templates
    Pages = 1,      // Page templates  
    Modules = 2,    // Module templates
    Forms = 3,      // Form templates
    Edms = 4,       // Document management templates
    Posts = 5,      // Post/blog templates
    Widgets = 6,    // Widget templates
    Masters = 7,    // Master layout templates
}
```

**Most commonly used:**
- `folderType: 7` (Masters) - Master layouts
- `folderType: 1` (Pages) - Page templates
- `folderType: 2` (Modules) - Reusable modules
- `folderType: 5` (Posts) - Blog posts and articles

### Content: The Information on Your Pages

-   **Pages (`CreatePageContent`):** The actual webpages that your visitors see, like "Home" or "About". Each page uses a Page Template to display its content. The `content` parameter should include HTML that will be rendered in the template. The Master Layout is automatically loaded by the CMS based on theme settings.
-   **Posts (`CreatePostContent`):** Used for blog entries or news articles. The `content` parameter should include HTML for the post body that will be rendered in the template. The layout is automatically determined by the CMS.
-   **Custom Data (`CreateDatabaseFromPrompt`):** For lists of things, like products, team members, or service offerings. Instead of hard-coding them into a page, you can store them in a database table and display them dynamically.

---

## Your Toolbox: The MCP Commands

You have a set of powerful tools (MCP commands) to create and manage your site.

### For Managing Templates

-   `CreateTemplate`: Make a new template.
-   `ListTemplates`: See all existing templates.
-   `UpdateTemplate`: Change an existing template.
-   `DeleteTemplate`: Remove a template.

### For Managing Content

-   `CreatePageContent`: Create a new webpage.
-   `CreatePostContent`: Create a new blog post or article.
-   `CreateModuleContent`: Create a new reusable module.
-   `ListPageContents`: See all your pages.
-   `ListPostContents`: See all your posts.
-   `ListModuleContents`: See all your modules.
-   `UpdatePageContent`: Change a page's content or settings.
-   `UpdatePostContent`: Change a post's content or settings.
-   `UpdateModuleContent`: Change a module's content or settings.
-   `DeletePageContent`: Remove a page.
-   `DeletePostContent`: Remove a post.
-   `DeleteModuleContent`: Remove a module.

### For Managing Data

-   `CreateDatabaseFromPrompt`: Create a new database table from a simple description.
-   `CreateManyMixDbData`: Add multiple records (e.g., products, services) to a table at once. **Always use full, public URLs for images, photos, pictures, ...** (e.g., `https://images.unsplash.com/photo-...`).
-   `GetListMidxDbData`: Fetch data from a table to display on a page.

### For Managing Relationships

-   `CreateMixDbRelationshipFromPrompt`: Create relationships between different content types (e.g., pages with modules, posts with categories).
-   **Use Cases:** Connect pages with nested modules, organize posts by categories, create content hierarchies.
-   **Important:** Always use `loadNestedData: true` when querying content with relationships.

---

## Getting Started Resources

Now that you understand the basics, here are the next steps:

### 📚 **Step-by-Step Guides**
- **[Creating Basic Pages](./ai-workflows-basic-pages.md)** - Learn how to create webpages, templates, and layouts
- **[Creating Basic Modules](./ai-workflows-basic-modules.md)** - Learn how to create and manage reusable modules
- **[Working with Dynamic Data](./ai-workflows-dynamic-data.md)** - Handle databases, modules, and data-driven content
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog posts and news articles
- **[Creating Database Relationships](./ai-workflows-relationships.md)** - Learn when and how to create relationships between content types

### 🛠️ **Technical References**
- **[Template Patterns & Best Practices](./patterns/template-patterns-overview.md)** - Template code patterns, rendering, and troubleshooting
- **[Mix CMS Reference](./reference/mix-cms-reference.md)** - Enums, constants, and technical reference guide
- **[MCP Tools Reference](./reference/mcp-tools-reference.md)** - Complete MCP command documentation and enhanced agentic server architecture

### 📖 **Additional Resources**
- **[Developer Guide](./developer-guide.md)** - Technical guide for C# developers
- **[Website Building Best Practices](./website-building-best-practices.md)** - Project workflow and methodology
- **[MixDb Lessons Learned](./mixdb-lessons-learned.md)** - Database best practices and common pitfalls

---

## Key Development Patterns

### Service Usage (Critical)
- **Data Access:** Use `IMixDbDataServiceFactory` with dependency injection
- **Field Access:** Use typed methods: `@item.Value<string>("fieldName")`
- **Required Setup:** Include namespaces and inject services
- **Query Pattern:** Use `SearchMixDbRequestModel` for data queries
- **Template Models:** Use correct model based on template type:
  - Pages: `@model Mixcore.Domain.ViewModels.PageContentViewModel`
  - Posts: `@model Mixcore.Domain.ViewModels.PostContentViewModel`
  - Modules: `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` or `@model dynamic`
  - Database-driven: `@model dynamic`

### Template Structure
```razor
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
        TableName = "table_name",
        Queries = new List<MixQueryField>()
    };
    var items = await mixDbDataService.GetListByAsync(request);
}

@foreach (var item in items)
{
    <div>@item.Value<string>("name")</div>
}
```

### Partial Rendering
- Use relative paths: `"../Modules/TemplateName.cshtml"` (must start with "../" and end with ".cshtml")
- Follow enum pattern: `$"../{template.FolderType.ToString()}/FileName.cshtml"`

### Task Documentation (CRITICAL)
**After every successful task execution, you MUST document it in markdown files:**
- Update `database-schema.md` for any database changes
- Update `project-progress.md` for completed features
- Include date, description, status, and notes for team collaboration
- This ensures others can work in the same context and build upon your work

---

## Quick Start Checklist

✅ **Before you begin:**
1. Understand the difference between Templates (design) and Content (information)
2. Know that every page needs a Master Layout
3. Remember that dynamic data should use database tables, not hard-coded content

✅ **Your first website:**
1. Create a Master Layout template (`folderType: 7`)
2. Create a Page Template (`folderType: 1`)  
3. Create your first page content that uses the page template (layout is automatically assigned by CMS)

✅ **Next steps:**
1. Add dynamic content using database tables
2. Create reusable modules for common components
3. Set up blog posts if needed
4. Create relationships between content types when building complex layouts
5. Use the MCP Tools for CRUD operations on all content types

---

## Important Notes for Complex Websites

### When to Use Relationships
- **Page-Module Relationships:** When pages need multiple nested modules (header, hero, content, footer)
- **Post-Category Relationships:** When organizing blog content by topics or tags
- **Custom Content Relationships:** When connecting business data (e.g., appointments to services)

### MCP Tools Best Practices
- Always check existing templates with `ListTemplates` before creating new ones
- Use `loadNestedData: true` when querying content with relationships
- Document all relationships in `database-schema.md` and `project-progress.md`
- Follow the proper `folderType` values for template creation

---

## Need Help?

- **Troubleshooting:** Check the [Template Patterns & Best Practices](./patterns/template-patterns-overview.md) guide
- **Relationships:** Consult [Creating Database Relationships](./ai-workflows-relationships.md) for complex content connections
- **Command Reference:** Use [MCP Tools Reference](./reference/mcp-tools-reference.md) for specific tool parameters
- **Technical Issues:** Consult the [Developer Guide](./reference/developer-guide.md) for advanced topics
