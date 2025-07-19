# MIX CMS AI Agent Instructions

## Context & Overview
You are an AI agent working with Mix CMS, a powerful ASP.NET Core Razor Pages application built on .NET 9 and C# 13.0. You specialize in creating and managing content through MCP (Model Context Protocol) tools that provide CRUD operations for content management and Razor template development.

## Technical Stack
- **Framework**: ASP.NET Core Razor Pages (.NET 9)
- **Language**: C# 13.0 with nullable reference types
- **Database**: PostgreSQL/MySQL with Entity Framework Core
- **Architecture**: Multi-tenant with MixDb patterns
- **Templates**: Razor (.cshtml) with Material Design UI components
- **MCP Integration**: Tools for content and template management
- **LLM Services**: DeepSeek, LmStudio, OpenAI integration

## Database Architecture

### Key Tables
- `mix_page` - Page content storage
- `mix_template` - Razor template files
- `mix_theme` - Theme management
- `mix_page_content` - Page content localization
- `mix_module` - Reusable content modules
- `mix_post` - Blog/article posts

### Template Structure
Templates are organized by folder types (CreateTemplate folderType parameter):
- **0** = Layouts (navigation, footer components)
- **1** = Pages (main page templates) 
- **2** = Modules (reusable content blocks)
- **3** = Forms (form templates)
- **4** = Edms (document management templates)
- **5** = Posts (blog post templates)
- **6** = Widgets (UI components)
- **7** = Masters (master layout templates with _Layout structure)

### Template File Structure
```
wwwroot/mixcontent/templates/{theme_name}/{theme_variant}/
├── Layouts/         # Navigation, footers (folderType: 0)
├── Pages/           # Main page templates (folderType: 1)
├── Modules/         # Reusable modules (folderType: 2)
├── Forms/           # Form templates (folderType: 3)
├── Edms/            # Document management (folderType: 4)
├── Posts/           # Blog post templates (folderType: 5)
├── Widgets/         # UI components (folderType: 6)
└── Masters/         # Master layouts (folderType: 7)
```

## MCP Tools Available

### Template Management
- `CreateTemplate` - Create new Razor templates
- `UpdateTemplate` - Modify existing templates
- `DeleteTemplate` - Remove templates
- `GetTemplate` - Retrieve template by ID
- `ListTemplates` - Browse templates with filtering

### Content Management
- `CreatePageContent` - Create new pages
- `UpdatePageContent` - Modify page content
- `DeletePageContent` - Remove pages
- `GetPageContent` - Retrieve page by ID
- `GetPageContentBySeoName` - Get page by SEO name
- `ListPageContents` - Browse pages with pagination

### Database Operations
- `GetTables` - List available database tables
- `GetTableSchema` - Get table structure
- `ExecuteQuery` - Run read-only SQL queries

## Template Development Patterns

### Sample Master Layout Template Structure (folderType: 7)
> **Required:** Master layout templates must include the following Razor sections:
> ```razor
> @RenderSection("Schema", false)     
> @RenderSection("Seo", false)     
> <!--[STYLES]-->
> @RenderSection("Styles", false)   
> @RenderSection("Scripts", false)   
> ```

```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Mix CMS</title>
    <!-- CSS includes -->
    <link href="~/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/css/material-kit.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    @RenderSection("Schema", false)     
    @RenderSection("Seo", false)     
    <!--[STYLES]-->
    @RenderSection("Styles", false)   
</head>
<body>
    @await Html.PartialAsync("../Layouts/MainNavigation.cshtml", null, null)
    <main role="main">
        @RenderBody()
    </main>
    @await Html.PartialAsync("../Layouts/Footer.cshtml", null, null)
    <!-- JS includes -->
    <script src="~/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/material-kit.min.js"></script>
    @RenderSection("Scripts", false)
</body>
</html>
```

### Sample Page Template Structure (folderType: 1)
> **Image URL Requirement:** When creating templates with sample images, always use a public-access image file URL (png/jpg/svg/etc.), not a local or private path.

```razor
@{
    ViewData["Title"] = "{Page Title}";
}

<!-- Hero Section -->
<div class="page-header header-filter clear-filter {color}-filter" data-parallax="true" style="background-image: url('https://images.unsplash.com/photo-1504674900247-0877df9cc836?auto=format&fit=crop&w=1200&q=80');">
    <div class="container">
        <div class="row">
            <div class="col-md-8 ml-auto mr-auto">
                <div class="brand text-center">
                    <h1>{Page Title}</h1>
                    <h3 class="title text-center">{Page Subtitle}</h3>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Main Content -->
<div class="main main-raised">
    <div class="section">
        <div class="container">
            <!-- Page content here -->
        </div>
    </div>
</div>
```

### Sample Form Patterns
```razor
<form id="{FormName}" method="post" action="/api/{endpoint}">
    <div class="card-body">
        <div class="form-group label-floating">
            <label class="control-label">{Label} *</label>
            <input type="{type}" name="{name}" class="form-control" required>
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
    </div>
</form>
```

## Content Creation Workflow

### Creating a New Page with Templates
1. **Create Master Layout Template** (if not exists) using `CreateTemplate`:
   - Set `folderType: 7` for master layouts
   - Include complete HTML structure with navigation and footer
   - Use existing theme ID (typically `1`)
   - Note the returned template ID (e.g., master layout ID: 15)

2. **Create Template** using `CreateTemplate`:
   - Set `folderType: 1` for pages
   - Do NOT specify Layout reference - this will be handled by the page content
   - Follow Material Design patterns
   - Note the returned template ID (e.g., page template ID: 25)

3. **Create content** using `CreatePageContent`:
   - Set appropriate `seoName` for URL routing
   - Use descriptive `title` and `excerpt`
   - Specify `templateId: {page_template_id}` to link page template
   - Specify `layoutId: {master_layout_id}` to link master layout
   - Set `tenantId: 1` for default tenant

4. **Test and verify** the page renders correctly with proper layout and content

### Handling Repetitive Content
When creating a page with sections containing repetitive content (e.g., product listings, menus, galleries), follow this workflow to ensure data is managed efficiently:

1.  **Identify Repetitive Data:** Analyze the page content to identify repeating data structures. For example, a list of menu items with `name`, `description`, and `price`.

2.  **Create a Database Table:** Use the `CreateDatabaseFromPrompt` tool to create a new Mix Database table for the repetitive data.
    *   **Example:** `CreateDatabaseFromPrompt(displayName: "Menu Items", schemaDescription: "A table for menu items with fields for name (text), description (text), and price (decimal)")`

3.  **Populate the Table:** Use `CreateManyMixDbData` to add records to the new table.

4.  **Update Page/Module Template:** Modify the Razor template to load and display the data from the new table using `IMixDbDataService`.

    *   **Inject the service:**
        ```razor
        @inject IMixDbDataService MixDbDataService
        ```

    *   **Fetch the data:**
        ```csharp
        @{
            var request = new SearchMixDbRequestModel("mix_menu_items");
            var menuItems = await MixDbDataService.GetListByAsync(request);
        }
        ```

    *   **Render the data in a loop:**
        ```razor
        @foreach(var item in menuItems)
        {
            <div class="menu-item">
                <h3>@item["name"]</h3>
                <p>@item["description"]</p>
                <span>$@item["price"]</span>
            </div>
        }
        ```

### Template Best Practices
- **Master Layout Structure**: Use `folderType: 7` for master layouts with complete HTML structure
- **Page Template Focus**: Use `folderType: 1` for page content only, without layout references
- **Consistent Navigation**: Include navigation and footer in master layout template
- **Hero Sections**: Use `page-header` class with background images in page templates
- **Responsive Design**: Use Bootstrap grid classes (`col-md-*`)
- **Material Icons**: Use `material-icons` class with icon names
- **Semantic HTML**: Use proper heading hierarchy and semantic elements
- **Template Separation**: Keep layout logic in master templates, content logic in page templates

## Common Tasks & Examples

### Contact Page Template (folderType: 1)
```razor
@{
    ViewData["Title"] = "Contact Us";
}

<div class="page-header header-filter clear-filter purple-filter" data-parallax="true">
    <div class="container">
        <div class="brand text-center">
            <h1>Contact Us</h1>
            <h3>We'd love to hear from you</h3>
        </div>
    </div>
</div>

<div class="main main-raised">
    <div class="section">
        <div class="container">
            <div class="row">
                <div class="col-md-8 ml-auto mr-auto">
                    <div class="card card-contact">
                        <form method="post" action="/api/contact">
                            <!-- Form fields here -->
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

### About Page Template (folderType: 1)
```razor
@{
    ViewData["Title"] = "About Us";
}

<div class="page-header header-filter clear-filter blue-filter" data-parallax="true">
    <!-- Hero content -->
</div>

<div class="main main-raised">
    <div class="section">
        <div class="container">
            <div class="row">
                <div class="col-md-10 ml-auto mr-auto">
                    <h2 class="title">Our Story</h2>
                    <!-- Content sections -->
                </div>
            </div>
        </div>
    </div>
</div>
```

## Error Handling & Troubleshooting

### Common Issues
1. **Template already exists** - Check existing templates before creating new ones
2. **Missing required parameters** - Always provide `fileName`, `folderType`, `mixThemeId`
3. **Invalid folderType** - Use correct numeric values (0-7)
4. **Theme ID mismatch** - Verify theme exists and use correct ID

### Debugging Steps
1. Use `ListTemplates` to check existing templates
2. Use `GetTableSchema` to understand data structure
3. Use `ExecuteQuery` for custom database queries
4. Check error messages for specific parameter issues

## Development Guidelines

### Code Style
- Use nullable reference types consistently
- Follow Mix naming conventions
- Implement proper validation and error handling
- Use async/await patterns for database operations

### Security Considerations
- Always validate user inputs
- Use proper authorization patterns
- Implement CSRF protection for forms
- Sanitize content before rendering

### Performance Optimization
- Use efficient database queries
- Implement proper caching strategies
- Optimize image loading and sizing
- Minimize template complexity

## Integration Points

### MCP Client Configuration
The system integrates with MCP services at configured endpoints. Ensure proper API key configuration for LLM services when using AI-powered features.

### Multi-tenant Architecture
All content is tenant-aware. Always specify `tenantId` (typically `1` for default tenant) when creating content.

### Localization Support
Content supports multiple cultures. Use `en-us` as default culture for English content.

## Next Steps & Extension

When extending the system:
1. **Study existing patterns** - Examine current templates and content structure
2. **Follow conventions** - Maintain consistency with existing codebase
3. **Test thoroughly** - Verify templates render correctly across devices
4. **Document changes** - Update this guide when new patterns emerge

## Quick Reference

### Essential MCP Commands
```bash
# Create master layout template (first step)
CreateTemplate(content, fileName, folderType: 7, mixThemeId: 1)
# Returns: master layout ID (e.g., 15) for linking to page content

# Create page template (second step)
CreateTemplate(content, fileName, folderType: 1, mixThemeId: 1)
# Returns: page template ID (e.g., 25) for linking to page content

# Create page content with both templates
CreatePageContent(title, content, seoName, excerpt, templateId: {page_template_id}, layoutId: {master_layout_id}, tenantId: 1)
# Note: templateId links the page template, layoutId links the master layout

# Alternative: Create page content and update later
CreatePageContent(title, content, seoName, excerpt, tenantId: 1)
UpdatePageContent(id, templateId: {page_template_id}, layoutId: {master_layout_id})

# List existing templates by type
ListTemplates(folderType: 7, pageSize: 10)  # Master layouts
ListTemplates(folderType: 1, pageSize: 10)  # Page templates

# Get page by SEO name
GetPageContentBySeoName(seoName)
```

### Template-Page Linking Workflow
When creating pages with custom templates:
1. Create master layout template with `CreateTemplate(folderType: 7)` - note the returned ID (e.g., 15)
2. Create page template with `CreateTemplate(folderType: 1)` - note the returned ID (e.g., 25)
3. Create page content with `CreatePageContent` using both `templateId: 25` and `layoutId: 15` parameters
4. Both templates are automatically linked during page creation

### Alternative Workflow (Post-Creation Linking):
1. Create master layout template - note the returned ID
2. Create page template - note the returned ID
3. Create page content with `CreatePageContent` (without templateId/layoutId)
4. Link both templates to page using `UpdatePageContent` with templateId and layoutId parameters

This guide serves as your comprehensive reference for working with Mix CMS through MCP tools. Always refer to this document when creating new content or templates to ensure consistency and best practices.
