# Master Layout Templates (folderType: 7)

Master layouts define the overall HTML structure for your website, including the `<html>`, `<head>`, and `<body>` tags. All other page templates are rendered within a master layout.

---

## Key Characteristics
- **Purpose:** Site-wide layout and structure.
- **Model:** `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- **Usage:** Referenced by pages via the `layoutId` property when creating page content.
- **Priority:** **Always create your master layout first**, before creating any pages that will use it.

---

## Creating a Master Layout

### MCP Command

Use the `CreateTemplate` command to create a new master layout.

```csharp
CreateTemplate(
    folderType: 7,
    fileName: "MainLayout",
    extension: ".cshtml",
    mixThemeId: 1, // Or your specific theme ID
    content: @"
        <!-- Paste the full master layout code here -->
    "
)
```

### Master Layout Requirements

A master layout **must** contain the following structure, including the specified `@RenderSection` calls. These sections are critical for SEO, styling, and script management.

**Example `MainLayout.cshtml`:**

```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@(!string.IsNullOrEmpty(Model.Title) ? Model.Title + " - " : "")My Awesome Site</title>
    <meta name="description" content="@Model.Excerpt">
    
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    
    <!-- REQUIRED SECTIONS -->
    @RenderSection("Schema", required: false)
    @RenderSection("Seo", required: false)
    <!--[STYLES]-->
    @RenderSection("Styles", required: false)
    
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
        .navbar-brand { font-weight: bold; }
        footer { background-color: #f8f9fa; padding: 20px 0; margin-top: 50px; }
    </style>
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <div class="container">
                <a class="navbar-brand" href="/">My Awesome Site</a>
            </div>
        </nav>
    </header>
    
    <main class="container mt-4">
        @RenderBody()  <!-- This renders the specific page content -->
    </main>
    
    <footer class="text-center">
        <p>&copy; @DateTime.Now.Year My Awesome Site. All Rights Reserved.</p>
    </footer>
    
    <!-- REQUIRED SCRIPT SECTION -->
    @RenderSection("Scripts", required: false)
    
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

### Explanation of Required Sections

-   `@RenderSection("Schema", false)`: Used for structured data (e.g., JSON-LD) to improve search engine understanding.
-   `@RenderSection("Seo", false)`: For additional SEO meta tags specific to a page.
-   `@RenderSection("Styles", false)`: Allows individual pages to inject their own CSS stylesheets into the `<head>`. The `<!--[STYLES]-->` marker is a placeholder for system-injected styles.
-   `@RenderSection("Scripts", false)`: Allows pages to add JavaScript files or inline scripts at the end of the `<body>`.

---

## Using the Master Layout

When you create a page using `CreatePageContent`, specify the `layoutId` of your master layout template. The system will then render that page's content inside the `@RenderBody()` section of your master layout.
</head>
<body>
    <!-- Navigation -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container">
            <a class="navbar-brand" href="/">My Website</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="/">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/about">About</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/services">Services</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/contact">Contact</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <!-- Main Content Area -->
    <main class="container my-4">
        @RenderBody()
    </main>

    <!-- Footer -->
    <footer class="bg-light py-4">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <p>&copy; 2024 My Website. All rights reserved.</p>
                </div>
                <div class="col-md-6 text-end">
                    <p>Built with Mix CMS</p>
                </div>
            </div>
        </div>
    </footer>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- Required Scripts Section -->
    @RenderSection("Scripts", false)
</body>
</html>
```

---

## Advanced Master Layout Patterns

### Multi-Layout Support
Create different master layouts for different sections:

```csharp
// Main site layout
CreateTemplate(folderType: 7, fileName: "MainLayout", ...)

// Admin layout
CreateTemplate(folderType: 7, fileName: "AdminLayout", ...)

// Blog layout
CreateTemplate(folderType: 7, fileName: "BlogLayout", ...)
```

### Dynamic Navigation
```razor
<!-- Dynamic navigation based on content -->
<nav class="navbar navbar-expand-lg navbar-dark bg-primary">
    <div class="container">
        <a class="navbar-brand" href="/">@ViewBag.SiteName</a>
        <div class="navbar-nav ms-auto">
            @* Generate navigation items dynamically *@
            @foreach(var navItem in ViewBag.NavigationItems ?? new List<object>())
            {
                <a class="nav-link" href="@navItem.Url">@navItem.Title</a>
            }
        </div>
    </div>
</nav>
```

### Conditional Content Areas
```razor
<!-- Optional sidebar area -->
@if(IsSectionDefined("Sidebar"))
{
    <div class="row">
        <div class="col-md-8">
            @RenderBody()
        </div>
        <div class="col-md-4">
            @RenderSection("Sidebar", false)
        </div>
    </div>
}
else
{
    @RenderBody()
}
```

---

## Required Razor Sections

These sections are **mandatory** for proper Mix CMS functionality:

```razor
@RenderSection("Schema", false)     <!-- For structured data -->
@RenderSection("Seo", false)        <!-- For SEO meta tags -->
<!--[STYLES]-->                     <!-- Mix CMS styles injection point -->
@RenderSection("Styles", false)     <!-- For page-specific styles -->
@RenderSection("Scripts", false)    <!-- For page-specific scripts -->
```

**Critical:** The `<!--[STYLES]-->` comment is required for Mix CMS to inject system styles.

---

## Using Master Layouts

### Assign to Pages
When creating pages, reference the master layout:

```csharp
CreatePageContent(
    title: "Home Page",
    content: "<h1>Welcome</h1>",
    seoName: "home",
    templateId: pageTemplateId,
    layoutId: masterLayoutId  // Reference the master layout ID
)
```

### Page Templates Reference Layouts
Page templates automatically inherit the master layout structure:

```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<!-- This content will be rendered inside @RenderBody() of the master layout -->
<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)
</div>

@section Styles {
    <style>
        .page-content { padding: 20px; }
    </style>
}

@section Scripts {
    <script>
        console.log('Page loaded');
    </script>
}
```

---

## Best Practices

### Design Principles
- **Keep it simple:** Master layouts should focus on structure, not complex logic
- **Responsive design:** Use Bootstrap or similar framework for mobile-first design
- **Performance:** Load critical CSS inline, defer non-critical resources
- **Accessibility:** Include proper ARIA labels and semantic HTML

### Code Organization
- **Minimal inline styles:** Use external stylesheets when possible
- **Modular navigation:** Consider using partial views for complex navigation
- **Error handling:** Include error boundaries for robust user experience

### SEO Optimization
```razor
<head>
    <title>@(!string.IsNullOrEmpty(Model.Title) ? Model.Title + " - " : "")@ViewBag.SiteName</title>
    <meta name="description" content="@Model.Excerpt">
    <meta property="og:title" content="@Model.Title">
    <meta property="og:description" content="@Model.Excerpt">
    <link rel="canonical" href="@ViewBag.CanonicalUrl">
    
    @RenderSection("Schema", false)
    @RenderSection("Seo", false)
</head>
```

---

## Troubleshooting

### Common Issues

**"Layout not found" error:**
- Verify the master layout was created with `folderType: 7`
- Check that the `layoutId` in `CreatePageContent` matches the layout's ID
- Use `ListTemplates` to verify the layout exists

**Styles not loading:**
- Ensure `<!--[STYLES]-->` comment is present in the `<head>` section
- Check that `@RenderSection("Styles", false)` is included
- Verify external CSS links are correct

**Scripts not working:**
- Confirm `@RenderSection("Scripts", false)` is at the bottom of `<body>`
- Check browser console for JavaScript errors
- Ensure script URLs are accessible

**Navigation issues:**
- Verify navigation links use correct URLs
- Check that Bootstrap JS is loaded for responsive navigation
- Test navigation on mobile devices

### Debugging Tips
- Use browser developer tools to inspect generated HTML
- Check the Mix CMS admin panel for template errors
- Verify all external resources (CSS/JS) are loading correctly
- Test with different browsers and devices

---

## Next Steps

After creating your master layout:

1. **Create Page Templates** - See [Pages Guide](./template-patterns-pages.md)
2. **Add Navigation Logic** - Implement dynamic menus
3. **Optimize Performance** - Minify CSS/JS, optimize images
4. **Test Responsiveness** - Ensure mobile-friendly design

---

## Related Guides

- **[Page Templates](./template-patterns-pages.md)** - Creating pages that use master layouts
- **[Module Templates](./template-patterns-modules.md)** - Reusable components for layouts
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
