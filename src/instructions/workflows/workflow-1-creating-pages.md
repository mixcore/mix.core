# Workflow 1: Creating Pages in Mix CMS

This workflow covers the complete process of creating pages in Mix CMS, including templates and content.

---

## Prerequisites

Before creating pages, ensure you understand:
- Mix CMS template folderType system
- Required template relationships (Master Layout + Page Template)
- Content creation patterns

---

## Step 1: Check Prerequisites

```markdown
// Check existing templates and verify folderTypes
ListTemplates()

// Required: Master Layout (folderType: 7) and Page Template (folderType: 1)
// Verify templateId has correct folderType before using in CreatePageContent
```

**What to look for:**
- Existing Master Layout with `folderType: 7`
- Existing Page Template with `folderType: 1`
- Note template IDs for content creation

---

## Step 2: Create Templates (if needed)

### Master Layout Template (Required First)
```markdown
CreateTemplate(
    folderType: 7,
    fileName: "MasterLayout",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<!DOCTYPE html>\n<html>\n<head>\n    <title>@ViewBag.Title</title>\n    @RenderSection(\"Schema\", false)\n    @RenderSection(\"Seo\", false)\n    <!--[STYLES]-->\n    @RenderSection(\"Styles\", false)\n</head>\n<body>\n    @RenderBody()\n    @RenderSection(\"Scripts\", false)\n</body>\n</html>"
)
```

### Page Template
```markdown
CreateTemplate(
    folderType: 1,
    fileName: "StandardPage",
    extension: ".cshtml", 
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PageContentViewModel\n<div class=\"page-container\">\n    <h1>@Model.Title</h1>\n    <div class=\"page-content\">\n        @Html.Raw(Model.Content)\n    </div>\n</div>"
)
```

**Required Master Layout Sections:**
```razor
@RenderSection("Schema", false)
@RenderSection("Seo", false)
<!--[STYLES]-->
@RenderSection("Styles", false)
@RenderSection("Scripts", false)
```

---

## Step 3: Create Page Content

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

**CRITICAL Requirements:**
- The `templateId` must reference a template with `folderType: 1` (Pages)
- The `layoutId` must reference a template with `folderType: 7` (Masters)
- Use `ListTemplates()` to verify template folderType before creating page content

---

## Step 4: Document Success

Update `project-progress.md`:
```markdown
### 2025-07-23 - Welcome Page Created
- **Master Layout**: MasterLayout.cshtml (ID: {layout_id})
- **Page Template**: StandardPage.cshtml (ID: {template_id})
- **Page Content**: "Welcome" page (ID: {page_id})
- **Status**: ✅ Complete - Page accessible at /welcome
- **Note**: Layout is automatically loaded by CMS based on theme settings
```

---

## Page Template Examples

### Basic Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-wrapper">
    <header class="page-header">
        <h1>@Model.Title</h1>
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <p class="page-excerpt">@Html.Raw(Model.Excerpt)</p>
        }
    </header>
    
    <main class="page-content">
        @Html.Raw(Model.Content)
    </main>
    
    <footer class="page-footer">
        <p>Published: @Model.CreatedDateTime.ToString("MMMM dd, yyyy")</p>
    </footer>
</div>
```

### Bootstrap Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="container">
    <div class="row">
        <div class="col-lg-8 offset-lg-2">
            <article class="page">
                <header class="page-header mb-4">
                    <h1 class="display-4">@Model.Title</h1>
                    @if (!string.IsNullOrEmpty(Model.Excerpt))
                    {
                        <p class="lead">@Html.Raw(Model.Excerpt)</p>
                    }
                </header>
                
                <div class="page-body">
                    @Html.Raw(Model.Content)
                </div>
            </article>
        </div>
    </div>
</div>
```

---

## Common Page Creation Patterns

### Homepage Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="homepage">
    <!-- Hero Section -->
    <section class="hero bg-primary text-white py-5">
        <div class="container">
            <div class="row">
                <div class="col-lg-8">
                    <h1 class="display-3">@Model.Title</h1>
                    @if (!string.IsNullOrEmpty(Model.Excerpt))
                    {
                        <p class="lead">@Html.Raw(Model.Excerpt)</p>
                    }
                    <a href="#content" class="btn btn-light btn-lg">Learn More</a>
                </div>
            </div>
        </div>
    </section>
    
    <!-- Main Content -->
    <section id="content" class="py-5">
        <div class="container">
            @Html.Raw(Model.Content)
        </div>
    </section>
</div>
```

### About Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="about-page">
    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <h1>@Model.Title</h1>
                @if (!string.IsNullOrEmpty(Model.Excerpt))
                {
                    <p class="lead">@Html.Raw(Model.Excerpt)</p>
                }
            </div>
            <div class="col-lg-6">
                <img src="https://images.unsplash.com/photo-1522071820081-009f0129c71c" 
                     alt="About Us" class="img-fluid rounded">
            </div>
        </div>
        
        <div class="row mt-5">
            <div class="col-12">
                @Html.Raw(Model.Content)
            </div>
        </div>
    </div>
</div>
```

---

## Troubleshooting

### Common Issues:
- **"Template already exists"**: Use `ListTemplates()` to check first
- **Missing layout**: Ensure Master Layout (folderType: 7) exists
- **Wrong template type**: Verify templateId has folderType: 1 (Pages) and layoutId has folderType: 7 (Masters)
- **Page not accessible**: Check seoName format and ensure no conflicts

### Verification Checklist:
1. ✅ Master Layout template exists (folderType: 7)
2. ✅ Page template exists (folderType: 1)
3. ✅ Template IDs are correct in CreatePageContent
4. ✅ seoName is unique and URL-friendly
5. ✅ Content is properly formatted HTML
6. ✅ Page is documented in project-progress.md

---

## Best Practices

### Template Design:
- Use semantic HTML structure
- Include responsive design classes
- Add proper meta information
- Use consistent CSS class naming

### Content Creation:
- Use descriptive, unique seoName values
- Include meaningful page titles
- Add excerpt for SEO and summary purposes
- Use proper HTML formatting in content

### Image Usage:
- ✅ Use: `https://images.unsplash.com/photo-...`
- ❌ Never: `/images/photo.jpg` or local paths

---

## Related Workflows

- **[Creating Modules](./workflow-2-creating-modules.md)** - Add dynamic components to pages
- **[Master Layouts](../patterns/template-patterns-masters.md)** - Master layout patterns
- **[Page Patterns](../patterns/template-patterns-pages.md)** - Additional page template examples
