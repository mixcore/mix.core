# Template Patterns & Best Practices

This guide covers advanced template patterns, troubleshooting, and best practices for Mix CMS development.

---

## MCP Commands vs Template Code

**MCP Commands** are the tools you use to create and manage your site structure (like `CreateTemplate`, `CreatePageContent`, `GetListMidxDbData`). These are called through the AI assistant.

**Template Code** is the Razor/C# code you write inside your `.cshtml` templates to display data dynamically. This code uses the Mix CMS services directly.

---

## Template Models

Each template type has a specific model that provides access to content and data:

- **Page Templates:** Use `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- **Module Templates:** Use `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` 
- **Post Templates:** Use `@model Mixcore.Domain.ViewModels.PostContentViewModel`

These models provide access to the content properties, metadata, and related data for each content type.

---

## Rendering Content in Templates

When you create content using MCP commands (like `CreatePageContent`, `CreatePostContent`, `CreateModuleContent`), the `content` parameter should contain HTML that will be rendered in your templates:

### Page Templates
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)  <!-- This renders the HTML content -->
</div>
```

### Module Templates
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="module-content">
    <h2>@Model.Title</h2>
    @Html.Raw(Model.Excerpt)  <!-- This renders the HTML content for modules -->
</div>
```

### Post Templates
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="post-content">
    <h1>@Model.Title</h1>
    <div class="post-excerpt">@Html.Raw(Model.Excerpt)</div>
    <div class="post-body">@Html.Raw(Model.Content)</div>
</article>
```

**Important:** Use `@Html.Raw(Model.Content)` to render HTML content, or `@Model.Content` for plain text display.

---

## When to Use Which Approach

### Use MCP Commands to:
- Create templates and pages
- Set up database tables
- Add initial data to tables
- Manage your site structure

### Use Template Code to:
- Display data from database tables on your web pages
- Create dynamic content that changes based on data
- Implement search, filtering, and pagination

### Example Workflow:
1. **MCP Command:** `CreateDatabaseFromPrompt` to create a "products" table
2. **Documentation:** Use `GetTableSchema` and document the schema in `database-schema.md`
3. **MCP Command:** `CreateManyMixDbData` to add product records
4. **Template Code:** Use `SearchMixDbRequestModel` and `IMixDbDataService` in your `.cshtml` file to display the products, referencing the documented column names

---

## Best Practices & Key Reminders

### Essential Guidelines

-   **Check MCP Tool Support First:** Before executing any task, check if there's an existing MCP tool that can help accomplish it. Use `ListSections` to explore available tools and resources.

-   **Master Layouts First:** Always create your `folderType: 7` Master Layout before creating pages.

-   **Public Image URLs:** When using images in templates, always use full, public URLs (e.g., from Unsplash). Do not use local file paths.

### Service Usage (CRITICAL)

-   **Use IMixDbDataServiceFactory:** Always use `IMixDbDataServiceFactory` with dependency injection for data access
-   **Field Access:** Use typed methods `@(item.Value<string>("fieldName"))` with injected services
-   **Template Models:** Use correct view models based on template type:
    - **Page Templates:** `@model Mixcore.Domain.ViewModels.PageContentViewModel`
    - **Module Templates:** `@model Mixcore.Domain.ViewModels.ModuleContentViewModel`
    - **Post Templates:** `@model Mixcore.Domain.ViewModels.PostContentViewModel`
    - **Data-driven Templates:** `@model dynamic` for MixDb data display
-   **Include Required Namespaces:** Always include necessary using statements for dependency injection

### Template Structure Best Practices

-   **Correct Model Declaration:** Always start templates with the appropriate model:
    - Page templates: `@model Mixcore.Domain.ViewModels.PageContentViewModel`
    - Module templates: `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` or `@model dynamic`
    - Post templates: `@model Mixcore.Domain.ViewModels.PostContentViewModel`
-   **Standard Namespaces:** Include `@using` statements when using MixDb dependency injection
-   **Relative Paths:** Always use relative paths for partials: `"../Modules/TemplateName.cshtml"` (must start with "../" and end with ".cshtml")
-   **Image URLs:** Always use full, public URLs for images in templates and sample data (e.g., `https://images.unsplash.com/photo-...`)

### Database Schema Management

-   **Database Schema Documentation:** When creating new tables with MCP tools, **ALWAYS** document the schema in your project's `database-schema.md` file immediately after creation. Include table name, columns, data types, and relationships. Use `GetTableSchema` to retrieve exact schema details and verify column names before documenting. This ensures you use the correct field names when rendering data in templates.

### Task Completion Tracking

-   **Document Every Success:** After completing any task successfully, document it in the relevant markdown file with date, description, and status. This enables team collaboration and prevents duplicate work.

-   **Required Documentation Format:**
```markdown
## Completed Tasks
### YYYY-MM-DD - [Task Name]
- **Action:** What was accomplished
- **Files:** Templates/pages created or modified
- **Status:** ✅ Complete / ⚠️ Needs Review
- **Notes:** Important context for future work
```

---

## Common Template Patterns

### Page Templates (Use PageContentViewModel)
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    <div class="page-excerpt">@Html.Raw(Model.Excerpt)</div>
    <div class="page-body">@Html.Raw(Model.Content)</div>
</div>
```

### Module Templates (Use ModuleContentViewModel)
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="module-content">
    <h2>@Model.Title</h2>
    <div class="module-excerpt">@Html.Raw(Model.Excerpt)</div>
</div>
```

### Post Templates (Use PostContentViewModel)
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="post-content">
    <h1>@Model.Title</h1>
    <div class="post-meta">
        <span class="date">@Model.CreatedDateTime.ToString("MMMM dd, yyyy")</span>
        <span class="author">By @Model.CreatedBy</span>
    </div>
    <div class="post-excerpt">@Html.Raw(Model.Excerpt)</div>
    <div class="post-body">@Html.Raw(Model.Content)</div>
</article>
```

### Database-Driven Templates (Use dynamic with MixDb Data)
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
        TableName = "table_name",
        Queries = new List<MixQueryField>()
    };
    var items = await mixDbDataService.GetListByAsync(request);
}

@foreach (var item in items)
{
    <div class="card">
        @if(!string.IsNullOrEmpty(item.Value<string>("image")))
        {
            <img src="@(item.Value<string>("image"))" alt="@(item.Value<string>("name"))" class="card-image" />
        }
        <h3>@(item.Value<string>("name"))</h3>
        <p>@(item.Value<string>("description"))</p>
    </div>
}
```

### Template with Image Handling
```razor
<div class="team-member">
    <img src="@(item.Value<string>("photo"))" alt="@(item.Value<string>("name"))" class="profile-photo" />
    <h3>@(item.Value<string>("name"))</h3>
    <p class="position">@(item.Value<string>("position"))</p>
    <p>@(item.Value<string>("bio"))</p>
</div>
```

**Important:** Always use full URLs like `https://images.unsplash.com/photo-...` for image sources.

---

## Choosing the Right Template Model

### When to Use Each Model Type

**Use `@model Mixcore.Domain.ViewModels.PageContentViewModel` when:**
- Creating page templates (`folderType: 1`)
- Displaying page content created with `CreatePageContent`
- Need access to page properties: `Title`, `Content`, `Excerpt`, `SeoName`

**Use `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` when:**
- Creating module templates (`folderType: 2`)
- Displaying module content created with `CreateModuleContent`
- Need access to module properties: `Title`, `SystemName`, `Excerpt`

**Use `@model Mixcore.Domain.ViewModels.PostContentViewModel` when:**
- Creating post templates (`folderType: 5`)
- Displaying post content created with `CreatePostContent`
- Need access to post properties: `Title`, `Content`, `Excerpt`, `CreatedDateTime`, `CreatedBy`

**Use `@model dynamic` when:**
- Creating templates that display MixDb database data
- Working with custom database tables created via `CreateDatabaseFromPrompt`
- Need to access dynamic data fields with `Value<T>()` methods
- Creating reusable components that fetch and display database records

### Template Creation Examples

**Creating a Page Template:**
```csharp
CreateTemplate(
    folderType: 1,
    fileName: "HomePage",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PageContentViewModel\n<div class=\"page\"><h1>@Model.Title</h1>@Html.Raw(Model.Content)</div>"
)
```

**Creating a Module Template for Database Data:**
```csharp
CreateTemplate(
    folderType: 2,
    fileName: "ProductGrid",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model dynamic\n@using Mix.Mixdb.Interfaces\n@inject IMixDbDataServiceFactory mixDbDataServiceFactory\n<!-- Database query and display code here -->"
)
```

### Content Templates with Built-in Models
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)
</div>
```

### Module Rendering in Pages (the relative path must start with "../" and end with ".cshtml")
```razor
@await Html.PartialAsync("../Modules/ServiceCard.cshtml")
```

---

## Troubleshooting Common Issues

### Data Access Problems
- **Issue:** "Service not registered"
  - **Solution:** Ensure you inject both `Mix.Database.Services.MixGlobalSettings.DatabaseService` and `IMixDbDataServiceFactory`
- **Issue:** Field access returns null
  - **Solution:** Use typed methods: `@(item.Value<string>("fieldName"))` or `@(item.Value<int>("fieldName"))`
- **Issue:** Connection issues
  - **Solution:** Use `dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)` for connection string

### Template Rendering Issues
- **Issue:** Partial not found
  - **Solution:** Use relative paths: `"../Modules/TemplateName.cshtml"` (must start with "../" and end with ".cshtml")
- **Issue:** Missing using statements
  - **Solution:** Include all required namespaces at the top of your template

### Performance Tips
- **Use specific queries** instead of loading all data
- **Implement proper error handling** with null checks
- **Cache frequently accessed data** when possible

-   **Schema Verification for Content Rendering:** Before creating content that loads data from MixDb, always check the database schema using `GetTableSchema` or refer to your `database-schema.md` documentation to ensure you understand the structure. This ensures you use the correct field names when rendering data in templates.

### Template Naming Conventions

-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter do NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
    - The system will automatically combine them to create the full filename

-   **Check for Existing Templates:** Use `ListTemplates` to avoid creating duplicates.

### Module Rendering Patterns

-   **Module Rendering Pattern:**
    - Always create module content first using `CreateModuleContent`
    - Then create template with `.cshtml` extension
    - Associate posts/content using `CreateModulePostAssociation`
    - Render in templates using the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

```razor
@{
    var module = Model.GetModule("moduleSystemName");
}
@if(module != null){
    @await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);
}
```

### Required Code Patterns

-   **Required Using Statements:** When using MixDb data in templates, always include these using statements at the top:
```razor
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@inject IMixDbDataService MixDbDataService
```

-   **Partial Rendering Pattern:** When rendering partial templates, always follow the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

### Template Folder Types Reference

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

### Partial Rendering Examples (Using FolderType.ToString())

    - **Modules (FolderType.Modules):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
    - **Pages (FolderType.Pages):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
    - **Posts (FolderType.Posts):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
    - **Master Layouts (FolderType.Masters):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`

```razor
// Example for modules (FolderType.Modules renders as "Modules")
@await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);

// Example for posts (FolderType.Posts renders as "Posts")
@await Html.PartialAsync($"../{post.Template.FolderType.ToString()}/{post.Template.FileName}.cshtml", post, null);
```

-   **Required Razor Sections:** Your Master Layout template **must** include these lines for styles and scripts to work correctly:
```razor
@RenderSection("Schema", false)     
@RenderSection("Seo", false)     
<!--[STYLES]-->
@RenderSection("Styles", false)   
@RenderSection("Scripts", false)   
```

---

## Troubleshooting Guide

### Template Issues

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` to check first.
-   **Page is missing header/footer:** You likely forgot to assign the `layoutId` when you created the page with `CreatePageContent`. You can fix this with `UpdatePageContent`.
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.

### Data Issues

-   **Data not displaying:** Check that you have the correct using statements, service injection, and that your `SearchMixDbRequestModel` uses the right property names (`FieldName`, `CompareOperator`).
-   **Incorrect field names in templates:** If you're seeing null values or errors when rendering data, verify the database schema using `GetTableSchema` to ensure you're using the correct field names. Field names are case-sensitive.
-   **Wrong comparison operator:** Use `MixCompareOperator.Equal`, `MixCompareOperator.Like`, `MixCompareOperator.LessThan`, etc. (not `ExpressionMethod`).

### MCP Response Format
MCP commands return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and pages together.

---

## Advanced Template Patterns

### Dynamic Content Loading

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
        TableName = "your_table_system_name",
        Queries = new List<MixQueryField>()
    };
    var data = await mixDbDataService.GetListByAsync(request);
}

@foreach (var item in data)
{
    <div class="item">
        <h3>@(item.Value<string>("title"))</h3>
        <p>@(item.Value<string>("description"))</p>
    </div>
}
```

### Conditional Rendering

```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

@if (!string.IsNullOrEmpty(Model.Excerpt))
{
    <div class="page-excerpt">
        @Html.Raw(Model.Excerpt)
    </div>
}

<div class="page-content">
    @Html.Raw(Model.Content)
</div>
```

### Module Integration

```razor
@{
    var featuredModule = Model.GetModule("featured-products");
    var testimonialModule = Model.GetModule("testimonials");
}

<div class="page-content">
    @Html.Raw(Model.Content)
    
    @if(featuredModule != null)
    {
        <section class="featured-section">
            @await Html.PartialAsync($"../{featuredModule.Template.FolderType.ToString()}/{featuredModule.Template.FileName}.cshtml", featuredModule, null)
        </section>
    }
    
    @if(testimonialModule != null)
    {
        <section class="testimonials-section">
            @await Html.PartialAsync($"../{testimonialModule.Template.FolderType.ToString()}/{testimonialModule.Template.FileName}.cshtml", testimonialModule, null)
        </section>
    }
</div>
```

---

## Performance Considerations

### Efficient Data Loading

- Cache frequently accessed data when possible
- Use specific queries rather than loading all data
- Implement pagination for large datasets
- Consider async loading for non-critical content

### Template Optimization

- Keep templates focused and modular
- Avoid complex logic in templates
- Use partial views for reusable components
- Minimize database calls within loops

---

## Security Best Practices

### Input Sanitization

- Always validate user input
- Use `@Html.Raw()` only for trusted content
- Escape user-generated content appropriately
- Implement proper authorization checks

### Template Security

- Avoid exposing sensitive data in templates
- Use proper model binding
- Implement content security policies
- Validate all parameters and models

---

## Next Steps

For deeper technical implementation:
- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
- **[Developer Guide](./developer-guide.md)** - Advanced C# development patterns
- **[Website Building Best Practices](./website-building-best-practices.md)** - Project methodology
