# Mix AI Agent Workflows: Complete Guide

This is the comprehensive workflow guide for **Mix AI Agents** working with Mix CMS. All common development tasks are covered in this single document.

## 🤖 MIX AI AGENT PROTOCOL
**Before Every Task**: You are a Mix AI Agent with these mandatory steps:

1. **Check MCP Server Support** - Verify Mix MCP Server can handle the requested task
2. **Use Mix.Mcp.Services First** - Primary interface for all Mix CMS operations
3. **Document MCP Usage** - Record which MCP tools were used and effectiveness
4. **Maintain Agent Identity** - Stay in Mix AI Agent character throughout
5. **MCP First, Code Second** - Only use direct C# when MCP tools insufficient

---

## Quick Reference: Essential Patterns

### Core Service Usage (Critical)
```razor
@model dynamic  <!-- For database-driven templates -->
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@using Mix.Constant.Constants

@inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
@inject IMixDbDataServiceFactory mixDbDataServiceFactory
```

### Template Types & Models
- **Pages**: `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- **Modules**: `@model Mixcore.Domain.ViewModels.ModuleContentViewModel`
- **Posts**: `@model Mixcore.Domain.ViewModels.PostContentViewModel`
- **Dynamic Data**: `@model dynamic`

### Folder Types Reference
```csharp
Masters = 7     // Master layouts (required first)
Pages = 1       // Page templates
Modules = 2     // Reusable modules
Posts = 5       // Blog/article templates
Forms = 3       // Form templates
Widgets = 6     // Widget components
```

---

## Workflow 1: Creating Pages

### Step 0: Review Content Type Instructions
- **CRITICAL**: Before starting, review the [Page Content Instructions](../patterns/template-patterns-pages.md) for required patterns and constraints

### Step 1: Check Prerequisites (Mix AI Agent Protocol)
```markdown
// As Mix AI Agent, ALWAYS check MCP Server support first
// Verify Mix.Mcp.Services can handle template and content operations

// Check existing templates and verify folderTypes
ListTemplates()

// Verify MCP Server database connectivity
GetTables()

// Required: Master Layout (folderType: 7) and Page Template (folderType: 1)
// Verify templateId has correct folderType before using in CreatePageContent
```

### Step 2: Create Templates (if needed)
```markdown
// Master Layout (required first)
CreateTemplate(
    folderType: 7,
    fileName: "MasterLayout",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<!-- Master layout with @RenderBody() and required sections -->"
)

// Page Template
CreateTemplate(
    folderType: 1,
    fileName: "StandardPage",
    extension: ".cshtml", 
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PageContentViewModel\n<div class=\"page\"><h1>@Model.Title</h1>@Html.Raw(Model.Content)</div>"
)
```

### Step 3: Create Page Content
```markdown
CreatePageContent(
    title: "Welcome",
    content: "<h1>Welcome</h1><p>Your content here...</p>",
    seoName: "welcome",
    templateId: {page_template_id},    // MUST be template with folderType: 1 (Pages)
    layoutId: {master_layout_id},      // MUST be template with folderType: 7 (Masters)
    tenantId: 1
)
```

**CRITICAL**: 
- The `templateId` must reference a template with `folderType: 1` (Pages)
- The `layoutId` must reference a template with `folderType: 7` (Masters)
- Use `ListTemplates()` to verify template folderType before creating page content.

### Step 4: Document Success (Mix AI Agent Protocol)
Update `project-progress.md`:
```markdown
### 2025-07-23 - Welcome Page Created (Mix AI Agent)
- **MCP Tools Used**: ListTemplates(), CreateTemplate(), CreatePageContent()
- **Mix AI Agent Identity**: Maintained throughout workflow
- **Master Layout**: MasterLayout.cshtml (ID: {layout_id})
- **Page Template**: StandardPage.cshtml (ID: {template_id})
- **Page Content**: "Welcome" page (ID: {page_id})
- **Status**: ✅ Complete - Page accessible at /welcome
- **MCP Server Performance**: All operations successful via Mix.Mcp.Services
- **Note**: Layout is automatically loaded by CMS based on theme settings
```

---

## Workflow 2: Creating Modules

### Step 0: Review Content Type Instructions
- **CRITICAL**: Before starting, review the [Module Content Instructions](../patterns/template-patterns-modules.md) for required patterns and constraints

### Step 1: Create Module Content
```markdown
CreateModuleContent(
    title: "Featured Products",
    excerpt: "Display featured products from database",
    systemName: "featured-products",
    pageSize: 10,
    tenantId: 1,
    type: 0
)
```

### Step 2: Create Module Template
```markdown
CreateTemplate(
    folderType: 2,
    fileName: "FeaturedProducts",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.ModuleContentViewModel\n<div class=\"module\"><h2>@Model.Title</h2>@Html.Raw(Model.Excerpt)</div>"
)
```

**CRITICAL**: When linking module content to a template, the `templateId` must reference a template with `folderType: 2` (Modules).

### Step 3: Use Module in Pages
```razor
@await Html.PartialAsync("../Modules/FeaturedProducts.cshtml", moduleModel)
```

---

## Workflow 3: Creating Blog Posts

### Step 0: Review Content Type Instructions
- **CRITICAL**: Before starting, review the [Post Content Instructions](../patterns/template-patterns-posts.md) for required patterns and constraints

### Step 1: Create Post Template (if needed)
```markdown
CreateTemplate(
    folderType: 5,
    fileName: "BlogPost",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PostContentViewModel\n<article><h1>@Model.Title</h1><div class=\"meta\">@Model.CreatedDateTime.ToString(\"MMMM dd, yyyy\")</div>@Html.Raw(Model.Content)</article>"
)
```

### Step 2: Create Post Content
```markdown
CreatePostContent(
    title: "Getting Started Guide",
    content: "<p>This post explains how to get started...</p>",
    excerpt: "<p>A comprehensive guide to getting started...</p>",
    seoName: "getting-started-guide",
    templateId: {post_template_id},    // MUST be template with folderType: 5 (Posts)
    tenantId: 1
)
```

**CRITICAL**: The `templateId` must reference a template with `folderType: 5` (Posts). Use `ListTemplates()` to verify template folderType before creating post content.

### Step 3: Manage Post Status
- **0 = Preview**: For drafts and previews
- **1 = Published**: Live and public
- **2 = Draft**: Work in progress

---

## Workflow 4: Working with Database Data

### Step 1: Create Database Table
```markdown
CreateDatabaseFromPrompt(
    displayName: "Products",
    schemaDescription: "A table for products with name (text), description (text), price (decimal), image (text)"
)
```

### Step 2: Document Schema (CRITICAL)
Update `database-schema.md`:
```markdown
## mix_products
- **id** (int) - Primary key
- **name** (nvarchar) - Product name
- **description** (nvarchar) - Product description
- **price** (decimal) - Product price
- **image** (nvarchar) - Full image URL (e.g., https://images.unsplash.com/...)
```

### Step 3: Add Sample Data
```markdown
CreateManyMixDbData(
    databaseSystemName: "mix_products",
    dataJson: '[{"name":"Wireless Headphones","description":"High-quality audio","price":99.99,"image":"https://images.unsplash.com/photo-1505740420928-5e560c06d30e"}]'
)
```

### Step 4: Create Data-Driven Template
```razor
@model dynamic
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
        TableName = "mix_products",
        Queries = new List<MixQueryField>()
    };
    var products = await mixDbDataService.GetListByAsync(request);
}

@foreach (var product in products)
{
    <div class="product-card">
        <img src="@(product.Value<string>("image"))" alt="@(product.Value<string>("name"))" />
        <h3>@(product.Value<string>("name"))</h3>
        <p>@(product.Value<string>("description"))</p>
        <span class="price">$@(product.Value<decimal>("price"))</span>
    </div>
}
```

---

## Workflow 5: Creating Relationships

### When to Use Relationships
- Pages with multiple modules (complex layouts)
- Posts with categories or tags
- Custom content associations

### Step 1: Create Relationship
```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "Page",
    destinateTableName: "Module",
    displayName: "Page Modules",
    propertyName: "modules",
    relationshipType: 0
)
```

### Step 2: Load Related Data
```markdown
GetPageContent(id: 1, loadNestedData: true)
```

### Step 3: Use in Templates
```razor
@if (Model.Modules != null && Model.Modules.Any())
{
    @foreach (var module in Model.Modules)
    {
        @await Html.PartialAsync($"../Modules/{module.Template.FileName}.cshtml", module)
    }
}
```

---

## Best Practices & Critical Guidelines

### Template Naming
- `extension`: Always ".cshtml" (include the dot)
- `fileName`: Never include ".cshtml" (e.g., "HomePage", not "HomePage.cshtml")

### Page Content Template Requirements
⚠️ **CRITICAL**: When creating page content with `CreatePageContent()` or `UpdatePageContent()`:
- The `templateId` parameter MUST reference a template with `folderType: 1` (Pages)
- The `layoutId` parameter MUST reference a template with `folderType: 7` (Masters)
- Use `ListTemplates()` to verify the template's folderType before using
- Page content cannot use templates with other folderTypes (Modules, Posts, etc.)

### Post Content Template Requirements
⚠️ **CRITICAL**: When creating post content with `CreatePostContent()` or `UpdatePostContent()`:
- The `templateId` parameter MUST reference a template with `folderType: 5` (Posts)
- Use `ListTemplates()` to verify the template's folderType before using
- Post content cannot use templates with other folderTypes (Pages, Modules, etc.)

### Module Content Template Requirements
⚠️ **CRITICAL**: When creating module content with `CreateModuleContent()` or `UpdateModuleContent()`:
- The `templateId` parameter MUST reference a template with `folderType: 2` (Modules)
- Use `ListTemplates()` to verify the template's folderType before using
- Module content cannot use templates with other folderTypes (Pages, Posts, etc.)

**Example Verification:**
```markdown
// First, check template folderType
ListTemplates()
// Look for templates with correct folderType:
// - folderType: 1 for Pages (templateId in CreatePageContent)
// - folderType: 7 for Masters (layoutId in CreatePageContent)
// - folderType: 5 for Posts (templateId in CreatePostContent)
// - folderType: 2 for Modules (templateId in CreateModuleContent)
```

### Image URLs
✅ **Always use full public URLs:**
- `https://images.unsplash.com/photo-...`
- `https://picsum.photos/300/200`

❌ **Never use local paths:**
- `/images/photo.jpg`
- `./assets/image.png`

### Required Razor Sections in Master Layouts
```razor
@RenderSection("Schema", false)
@RenderSection("Seo", false)
<!--[STYLES]-->
@RenderSection("Styles", false)
@RenderSection("Scripts", false)
```

### Field Access Patterns
```razor
@(item.Value<string>("fieldName"))    // For text fields
@(item.Value<decimal>("price"))       // For numbers
@(item.Value<bool>("isActive"))       // For booleans
```

### Error Handling
- Always include null checks for optional fields
- Use `@Html.Raw()` for HTML content, `@Model.Property` for plain text
- Test with `loadNestedData: true` for related content

---

## Documentation Requirements

### After Every Successful Task:
1. **Update project-progress.md** with task completion details
2. **Update database-schema.md** when creating tables
3. **Include Status**: ✅ Complete / ⚠️ Needs Review / ❌ Issues
4. **Record IDs**: Save template/content IDs for future reference

### Required Format:
```markdown
### YYYY-MM-DD - [Task Description]
- **Action**: What was accomplished
- **Files**: Templates/pages/tables created
- **IDs**: Record important IDs for linking
- **Status**: Current status
- **Notes**: Context for future work
```

---

## Troubleshooting Guide

### Common Issues:
- **"Template already exists"**: Use `ListTemplates` to check first
- **Missing layout**: Ensure Master Layout (folderType: 7) exists
- **Wrong template type for pages**: Verify templateId has folderType: 1 (Pages) and layoutId has folderType: 7 (Masters)
- **Wrong template type for posts**: Verify templateId has folderType: 5 (Posts) for post content
- **Wrong template type for modules**: Verify templateId has folderType: 2 (Modules) for module content
- **Data not displaying**: Check using statements and service injection
- **Broken images**: Use full URLs, include null checks
- **Relationship issues**: Verify `loadNestedData: true` parameter

### Verification Steps:
1. Check prerequisites with `ListTemplates`, `GetTables`
2. **Verify template folderType**: 
   - Pages: templateId folderType: 1, layoutId folderType: 7
   - Posts: templateId folderType: 5
   - Modules: templateId folderType: 2
3. Verify schema with `GetTableSchema`
4. Test creation with appropriate MCP tools
5. Validate rendering in templates
6. Document success and record IDs

---

## Quick Command Reference

### Template Management
- `ListTemplates()` - See all templates
- `CreateTemplate()` - Make new template
- `UpdateTemplate(id)` - Modify existing
- `DeleteTemplate(id, confirmDelete: "YES")` - Remove

### Content Management
- `CreatePageContent()` - New webpage (templateId: folderType 1, layoutId: folderType 7)
- `UpdatePageContent()` - Update webpage (templateId: folderType 1, layoutId: folderType 7)
- `CreatePostContent()` - New blog post (templateId: folderType 5)
- `UpdatePostContent()` - Update blog post (templateId: folderType 5)
- `CreateModuleContent()` - New module (templateId: folderType 2)
- `UpdateModuleContent()` - Update module (templateId: folderType 2)
- `ListPageContents()` - See all pages
- `GetPageContent(id)` - Get specific page

### Database Operations
- `CreateDatabaseFromPrompt()` - New table
- `CreateManyMixDbData()` - Add records
- `GetTableSchema(tableName)` - Check structure
- `GetListMidxDbData()` - Query data

### Relationships
- `CreateMixDbRelationshipFromPrompt()` - Link content
- Use `loadNestedData: true` in queries

This complete guide consolidates all essential workflows for Mix CMS development, providing AI agents with focused, actionable instructions for common tasks.
