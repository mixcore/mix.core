# AI Workflows: Creating Basic Pages and Templates

This guide provides step-by-step workflows for creating basic website pages and templates using Mix CMS MCP Tools for CRUD operations.

---

## How to Create a New Webpage

Follow these steps to create a fully functional webpage using MCP Tools.

### Step 1: Check Existing Templates
Use `ListTemplates` to see all available templates and avoid creating duplicates. You'll need to identify:
- **Master Layout Template**: A template with `folderType` = "Masters" (for layoutId)
- **Page Template**: A template with `folderType` = "Pages" (for templateId)

### Step 2: Create Templates (if needed)
If you don't have the required templates, create them first:
- **Master Layout**: Use `CreateTemplate` with `folderType: 7` (Masters)
- **Page Template**: Use `CreateTemplate` with `folderType: 1` (Pages)

**Required Razor Sections in Master Layout:**
Your Master Layout template **must** include these lines for styles and scripts to work correctly:
```razor
@RenderSection("Schema", false)     
@RenderSection("Seo", false)     
<!--[STYLES]-->
@RenderSection("Styles", false)   
@RenderSection("Scripts", false)   
```

**Template Models for Pages:**
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)  <!-- This renders the HTML content -->
</div>
```

### Step 3: Create the Page Content

Finally, create the page itself using MCP Tools and link it to the templates you identified/created.

-   **MCP Tool:** `CreatePageContent`
-   **Parameters:**
    -   `title`: "Welcome to Our Website"
    -   `content`: The HTML content that will be rendered in the template (e.g., `"<h1>Welcome</h1><p>Our company provides excellent services...</p>"`)
    -   `seoName`: "home" (this becomes the URL, e.g., `yoursite.com/home`)
    -   `templateId`: The ID of a template with `folderType` = "Pages" (get from `ListTemplates`)
    -   `layoutId`: The ID of a template with `folderType` = "Masters" (get from `ListTemplates`)
    -   `tenantId`: 1

---

## CRUD Operations for Page Content

Use these MCP Tools for managing page content:

### Create Page Content
-   **MCP Tool:** `CreatePageContent`
-   **Purpose:** Create new pages

### Read Page Content
-   **MCP Tool:** `GetPageContent` (by ID) or `GetPageContentBySeoName` (by SEO name)
-   **MCP Tool:** `ListPageContents` (list multiple pages with filtering)
-   **Purpose:** Retrieve existing page data

### Update Page Content
-   **MCP Tool:** `UpdatePageContent`
-   **Purpose:** Modify existing pages
-   **Required:** `id` parameter (from create/list operations)

### Delete Page Content
-   **MCP Tool:** `DeletePageContent`
-   **Purpose:** Remove pages
-   **Required:** `id` parameter and `confirmDelete: "YES"`

---

## Managing Page-Module Relationships

When pages have many nested modules, use MCP Tools for CRUD relationship operations to properly link and manage the connections between pages and modules.

### Create Relationships
-   **MCP Tool:** `CreateMixDbRelationshipFromPrompt`
-   **Purpose:** Create relationships between pages and modules
-   **Parameters:**
    -   `sourceTableName`: "Page" (the page content)
    -   `destinateTableName`: "Module" (the module content)
    -   `displayName`: "Page Modules" (relationship display name)
    -   `propertyName`: "modules" (property name for loading related data)
    -   `relationshipType`: 0 (one-to-many relationship)

### Managing Nested Module Content
When working with pages that contain multiple modules:

1. **Create the page first** using `CreatePageContent`
2. **Create individual modules** using `CreateModuleContent`
3. **Establish relationships** using `CreateMixDbRelationshipFromPrompt`
4. **Load related data** by setting `loadNestedData: true` in read operations

### Example Workflow for Complex Pages
```markdown
1. Create master page with `CreatePageContent`
2. Create individual modules (header, content, sidebar, footer)
3. Link modules to page using relationship tools
4. Verify nested structure with `GetPageContent` (loadNestedData: true)
```

---

## Template Naming Best Practices

-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
    - The system will automatically combine them to create the full filename

-   **Template Identification:**
    - **layoutId**: Must be the ID of a template with `folderType` = "Masters" (folderType: 7)
    - **templateId**: Must be the ID of a template with `folderType` = "Pages" (folderType: 1)
    - Use `ListTemplates` MCP Tool to find existing templates and their folderType values

-   **Check for Existing Templates:** Use `ListTemplates` MCP Tool to avoid creating duplicates.

---

## Partial Rendering Patterns

When rendering partial templates, always follow the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

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

- **Pages (FolderType.Pages):** `"../Pages/{templateName}.cshtml"`
- **Modules (FolderType.Modules):** `"../Modules/{templateName}.cshtml"`
- **Posts (FolderType.Posts):** `"../Posts/{templateName}.cshtml"`

```razor
// Example for pages (renders as "../Pages/{templateName}.cshtml")
@await Html.PartialAsync($"../{page.Template.FolderType.ToString()}/{page.Template.FileName}.cshtml", page, null);

// Example for modules (renders as "../Modules/{templateName}.cshtml")
@await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);
```

---

## Common Issues and Solutions

### Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` MCP Tool to check first.
-   **Page is missing header/footer:** You likely forgot to assign the correct `layoutId` (must be a template with `folderType` = "Masters") when you created the page with `CreatePageContent`. You can fix this with `UpdatePageContent`.
-   **Template linking issues:** Ensure `templateId` points to a template with `folderType` = "Pages" and `layoutId` points to a template with `folderType` = "Masters".
-   **Module relationships not working:** For pages with nested modules, ensure you've created proper relationships using `CreateMixDbRelationshipFromPrompt` and are loading data with `loadNestedData: true`.
-   **Nested content not displaying:** Check that module relationships are properly established and that your page template includes the necessary rendering logic for nested modules.
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.

### MCP Response Format
MCP Tools return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and pages together.

### Workflow Example
1. Run `ListTemplates` to see available templates
2. Identify Master Layout (folderType = "Masters") for `layoutId`
3. Identify Page Template (folderType = "Pages") for `templateId`
4. Use `CreatePageContent` with the correct IDs
5. If page needs modules, create relationships with `CreateMixDbRelationshipFromPrompt`
6. Verify with `GetPageContent` or `ListPageContents` (use `loadNestedData: true` for complex pages)

### Task Documentation (CRITICAL)
**After successfully completing any page creation task, document it in your project's `project-progress.md` file:**

```markdown
## Completed Tasks
### 2025-01-XX - Homepage Creation
- **Master Layout:** Created MasterLayout.cshtml (folderType: 7)
- **Page Template:** Created HomePage.cshtml (folderType: 1) 
- **Page Content:** Created "Welcome" page with templateId: X, layoutId: Y
- **Nested Modules:** Created header, hero, features, footer modules with relationships
- **Status:** ✅ Complete - Homepage accessible and styled correctly
- **Notes:** Uses TailwindCSS, includes hero section and features grid, 4 nested modules

### 2025-01-XX - Contact Page Setup
- **Page Template:** Created ContactPage.cshtml (folderType: 1)
- **Page Content:** Created "Contact Us" page 
- **Nested Modules:** Created contact form and map modules
- **Status:** ✅ Complete - Contact form ready for integration
```

This documentation ensures other team members can build upon your work and understand the current project state.

---

## Next Steps

Once you've mastered basic page creation, explore:
- **[Working with Dynamic Data](./ai-workflows-dynamic-data.md)** - Create database-driven content
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog functionality
- **[Module Workflows](./ai-workflows-basic-modules.md)** - Learn how to create and manage modules for complex pages
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
