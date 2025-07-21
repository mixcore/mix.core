# AI Workflows: Creating Basic Pages and Templates

This guide provides step-by-step workflows for creating basic website pages and templates using Mix CMS AI tools.

---

## How to Create a New Webpage

Follow these three steps to create a fully functional webpage.

### Step 1: Create the Master Layout (Do this once per site)

First, create the main layout that all your pages will share.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 7` (This is important!)
    -   `fileName`: "MasterLayout"
    -   `extension`: ".cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the full HTML structure, including placeholders for navigation and the main body. **Must include the required Razor sections.**

**Required Razor Sections in Master Layout:**
Your Master Layout template **must** include these lines for styles and scripts to work correctly:
```razor
@RenderSection("Schema", false)     
@RenderSection("Seo", false)     
<!--[STYLES]-->
@RenderSection("Styles", false)   
@RenderSection("Scripts", false)   
```

### Step 2: Create the Page Template

Next, create the specific layout for the content of your new page.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 1`
    -   `fileName`: "HomePage" or "ContactPage"
    -   `extension`: ".cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the HTML for the page's content area. Use a public URL for any sample images.

**Template Models for Pages:**
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)  <!-- This renders the HTML content -->
</div>
```

### Step 3: Create the Page Content

Finally, create the page itself and link it to the templates you just made.

-   **Tool:** `CreatePageContent`
-   **Parameters:**
    -   `title`: "Welcome to Our Website"
    -   `content`: The HTML content that will be rendered in the template (e.g., `"<h1>Welcome</h1><p>Our company provides excellent services...</p>"`)
    -   `seoName`: "home" (this becomes the URL, e.g., `yoursite.com/home`)
    -   `templateId`: The ID of the **Page Template** from Step 2.
    -   `layoutId`: The ID of the **Master Layout** from Step 1.
    -   `tenantId: 1`

---

## Template Naming Best Practices

-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
    - The system will automatically combine them to create the full filename

-   **Check for Existing Templates:** Use `ListTemplates` to avoid creating duplicates.

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

- **Modules (FolderType.Modules):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
- **Pages (FolderType.Pages):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
- **Posts (FolderType.Posts):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`
- **Master Layouts (FolderType.Masters):** `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`

```razor
// Example for modules (renders as "../Modules/{templateName}.cshtml")
@await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);

// Example for posts (renders as "../Posts/{templateName}.cshtml")
@await Html.PartialAsync($"../{post.Template.FolderType.ToString()}/{post.Template.FileName}.cshtml", post, null);
```

```razor
// Example for modules (renders as "../Modules/{templateName}.cshtml")
@await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null);

// Example for posts (renders as "../Posts/{templateName}.cshtml")
@await Html.PartialAsync($"../{post.Template.FolderType.ToString()}/{post.Template.FileName}.cshtml", post, null);
```

---

## Quick Reference Commands

### Template & Page Creation Workflow
1.  **Create Master Layout:**
    `CreateTemplate(content, fileName: "MasterLayout", extension: ".cshtml", folderType: 7, mixThemeId: 1)`
    *Returns the `master_layout_id`.*

2.  **Create Page Template:**
    `CreateTemplate(content, fileName: "MyPageTemplate", extension: ".cshtml", folderType: 1, mixThemeId: 1)`
    *Returns the `page_template_id`.*

3.  **Create Page Content:**
    `CreatePageContent(title, content, seoName, excerpt, templateId: {page_template_id}, layoutId: {master_layout_id}, tenantId: 1)`

### Finding Your Content
-   **List Templates:** `ListTemplates(folderType: 7)`
-   **Get Page by URL:** `GetPageContentBySeoName(seoName: "home")`

---

## Common Issues and Solutions

### Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` to check first.
-   **Page is missing header/footer:** You likely forgot to assign the `layoutId` when you created the page with `CreatePageContent`. You can fix this with `UpdatePageContent`.
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.

### MCP Response Format
MCP commands return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and pages together.

### Task Documentation (CRITICAL)
**After successfully completing any page creation task, document it in your project's `project-progress.md` file:**

```markdown
## Completed Tasks
### 2025-01-XX - Homepage Creation
- **Master Layout:** Created MasterLayout.cshtml (folderType: 7)
- **Page Template:** Created HomePage.cshtml (folderType: 1) 
- **Page Content:** Created "Welcome" page with templateId: X, layoutId: Y
- **Status:** ✅ Complete - Homepage accessible and styled correctly
- **Notes:** Uses TailwindCSS, includes hero section and features grid

### 2025-01-XX - Contact Page Setup
- **Page Template:** Created ContactPage.cshtml (folderType: 1)
- **Page Content:** Created "Contact Us" page 
- **Status:** ✅ Complete - Contact form ready for integration
```

This documentation ensures other team members can build upon your work and understand the current project state.

---

## Next Steps

Once you've mastered basic page creation, explore:
- **[Working with Dynamic Data](./ai-workflows-dynamic-data.md)** - Create database-driven content
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog functionality
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
