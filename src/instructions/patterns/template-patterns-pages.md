# Page Templates (folderType: 1)

Page templates define how individual pages are displayed within your master layout. They control the content structure and presentation for static and dynamic pages.

---

## Overview

Page templates (`folderType: 1`) are used to create and display individual pages on your website. They work within the master layout structure and focus on content presentation.

### Key Characteristics
- **Purpose:** Individual page content and structure
- **Model:** `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- **Usage:** Referenced by pages via `templateId`
- **Dependency:** Requires a master layout (`folderType: 7`)

---

## Creating Page Templates

### MCP Command
```csharp
CreateTemplate(
    folderType: 1,
    fileName: "StandardPage",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PageContentViewModel\n<div class=\"page\"><h1>@Model.Title</h1>@Html.Raw(Model.Content)</div>"
)
```

### Basic Page Template Structure
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    
    @if (!string.IsNullOrEmpty(Model.Excerpt))
    {
        <div class="page-excerpt">
            @Html.Raw(Model.Excerpt)
        </div>
    }
    
    <div class="page-body">
        @Html.Raw(Model.Content)
    </div>
</div>
```

---

## Page Template Examples

### Standard Content Page
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<article class="page-content">
    <header class="page-header">
        <h1 class="page-title">@Model.Title</h1>
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <p class="page-subtitle">@Html.Raw(Model.Excerpt)</p>
        }
    </header>
    
    <div class="page-body">
        @Html.Raw(Model.Content)
    </div>
    
    @if (!string.IsNullOrEmpty(Model.ModifiedBy))
    {
        <footer class="page-meta">
            <p>Last updated by @Model.ModifiedBy on @Model.LastModified?.ToString("MMMM dd, yyyy")</p>
        </footer>
    }
</article>

@section Styles {
    <style>
        .page-header { margin-bottom: 2rem; }
        .page-title { color: #333; margin-bottom: 0.5rem; }
        .page-subtitle { color: #666; font-size: 1.2rem; }
        .page-meta { margin-top: 2rem; color: #888; font-size: 0.9rem; }
    </style>
}
```

### Home Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<!-- Hero Section -->
<section class="hero-section bg-primary text-white py-5">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-md-6">
                <h1 class="display-4">@Model.Title</h1>
                @if (!string.IsNullOrEmpty(Model.Excerpt))
                {
                    <p class="lead">@Html.Raw(Model.Excerpt)</p>
                }
                <a href="/services" class="btn btn-light btn-lg">Learn More</a>
            </div>
            <div class="col-md-6">
                <img src="https://images.unsplash.com/photo-1551434678-e076c223a692?w=600" 
                     alt="Hero Image" class="img-fluid rounded">
            </div>
        </div>
    </div>
</section>

<!-- Main Content -->
<section class="py-5">
    <div class="container">
        @Html.Raw(Model.Content)
    </div>
</section>

<!-- Render Modules -->
@{
    var featuredModule = Model.GetModule("featured-services");
    var testimonialModule = Model.GetModule("testimonials");
}

@if(featuredModule != null)
{
    <section class="featured-section py-5 bg-light">
        <div class="container">
            @await Html.PartialAsync($"../{featuredModule.Template.FolderType.ToString()}/{featuredModule.Template.FileName}.cshtml", featuredModule, null)
        </div>
    </section>
}

@if(testimonialModule != null)
{
    <section class="testimonials-section py-5">
        <div class="container">
            @await Html.PartialAsync($"../{testimonialModule.Template.FolderType.ToString()}/{testimonialModule.Template.FileName}.cshtml", testimonialModule, null)
        </div>
    </section>
}

@section Styles {
    <style>
        .hero-section { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
        .featured-section { border-top: 1px solid #dee2e6; }
    </style>
}
```

### About Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="about-page">
    <div class="row">
        <div class="col-lg-8">
            <h1>@Model.Title</h1>
            
            @if (!string.IsNullOrEmpty(Model.Excerpt))
            {
                <div class="lead mb-4">
                    @Html.Raw(Model.Excerpt)
                </div>
            }
            
            <div class="content">
                @Html.Raw(Model.Content)
            </div>
        </div>
        
        <div class="col-lg-4">
            <div class="sidebar">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Quick Facts</h5>
                        <ul class="list-unstyled">
                            <li><strong>Founded:</strong> 2020</li>
                            <li><strong>Team Size:</strong> 25+ professionals</li>
                            <li><strong>Location:</strong> Global</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section Styles {
    <style>
        .about-page .sidebar { position: sticky; top: 20px; }
        .about-page .content h2 { margin-top: 2rem; }
    </style>
}
```

### Contact Page Template
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="contact-page">
    <div class="row">
        <div class="col-md-8">
            <h1>@Model.Title</h1>
            
            @if (!string.IsNullOrEmpty(Model.Excerpt))
            {
                <p class="lead">@Html.Raw(Model.Excerpt)</p>
            }
            
            <div class="content mb-4">
                @Html.Raw(Model.Content)
            </div>
            
            <!-- Contact Form -->
            <form class="contact-form">
                <div class="row">
                    <div class="col-md-6 mb-3">
                        <label for="name" class="form-label">Name *</label>
                        <input type="text" class="form-control" id="name" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="email" class="form-label">Email *</label>
                        <input type="email" class="form-control" id="email" required>
                    </div>
                </div>
                <div class="mb-3">
                    <label for="subject" class="form-label">Subject</label>
                    <input type="text" class="form-control" id="subject">
                </div>
                <div class="mb-3">
                    <label for="message" class="form-label">Message *</label>
                    <textarea class="form-control" id="message" rows="5" required></textarea>
                </div>
                <button type="submit" class="btn btn-primary">Send Message</button>
            </form>
        </div>
        
        <div class="col-md-4">
            <div class="contact-info">
                <h3>Get in Touch</h3>
                <div class="contact-item">
                    <h5>Email</h5>
                    <p>contact@example.com</p>
                </div>
                <div class="contact-item">
                    <h5>Phone</h5>
                    <p>+1 (555) 123-4567</p>
                </div>
                <div class="contact-item">
                    <h5>Address</h5>
                    <p>123 Business Street<br>City, State 12345</p>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        document.querySelector('.contact-form').addEventListener('submit', function(e) {
            e.preventDefault();
            alert('Thank you for your message! We will get back to you soon.');
        });
    </script>
}
```

---

## Database-Driven Page Templates

### Products Listing Page
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel
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

<div class="products-page">
    <h1>@Model.Title</h1>
    
    @if (!string.IsNullOrEmpty(Model.Content))
    {
        <div class="page-intro">
            @Html.Raw(Model.Content)
        </div>
    }
    
    <div class="products-grid">
        <div class="row">
            @foreach (var product in products)
            {
                <div class="col-md-4 mb-4">
                    <div class="card h-100">
                        @if(!string.IsNullOrEmpty(product.Value<string>("image")))
                        {
                            <img src="@(product.Value<string>("image"))" 
                                 class="card-img-top" 
                                 alt="@(product.Value<string>("name"))">
                        }
                        <div class="card-body">
                            <h5 class="card-title">@(product.Value<string>("name"))</h5>
                            <p class="card-text">@(product.Value<string>("description"))</p>
                            <div class="price">
                                <strong>$@(product.Value<decimal>("price"))</strong>
                            </div>
                        </div>
                        <div class="card-footer">
                            <a href="/product/@(product.Value<string>("slug"))" 
                               class="btn btn-primary">View Details</a>
                        </div>
                    </div>
                </div>
            }
        </div>
    </div>
</div>

@section Styles {
    <style>
        .products-grid .card { transition: transform 0.2s; }
        .products-grid .card:hover { transform: translateY(-5px); }
        .price { font-size: 1.2rem; color: #28a745; }
    </style>
}
```

### Team Page with Database Data
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel
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
        TableName = "mix_team_members",
        Queries = new List<MixQueryField>()
    };
    var teamMembers = await mixDbDataService.GetListByAsync(request);
}

<div class="team-page">
    <div class="text-center mb-5">
        <h1>@Model.Title</h1>
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <p class="lead">@Html.Raw(Model.Excerpt)</p>
        }
    </div>
    
    @if (!string.IsNullOrEmpty(Model.Content))
    {
        <div class="team-intro mb-5">
            @Html.Raw(Model.Content)
        </div>
    }
    
    <div class="team-grid">
        <div class="row">
            @foreach (var member in teamMembers)
            {
                <div class="col-lg-3 col-md-6 mb-4">
                    <div class="team-member text-center">
                        <div class="member-photo">
                            <img src="@(member.Value<string>("photo"))" 
                                 alt="@(member.Value<string>("name"))" 
                                 class="rounded-circle img-fluid">
                        </div>
                        <h4>@(member.Value<string>("name"))</h4>
                        <p class="position">@(member.Value<string>("position"))</p>
                        <p class="bio">@(member.Value<string>("bio"))</p>
                        @if(!string.IsNullOrEmpty(member.Value<string>("linkedin")))
                        {
                            <a href="@(member.Value<string>("linkedin"))" 
                               class="btn btn-outline-primary btn-sm">LinkedIn</a>
                        }
                    </div>
                </div>
            }
        </div>
    </div>
</div>

@section Styles {
    <style>
        .team-member { padding: 20px; }
        .member-photo img { width: 150px; height: 150px; object-fit: cover; }
        .position { color: #6c757d; font-weight: 600; }
        .bio { font-size: 0.9rem; }
    </style>
}
```

---

## Module Integration in Pages

### Rendering Modules
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <!-- Page Content -->
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)
    
    <!-- Render Associated Modules -->
    @{
        var heroModule = Model.GetModule("hero-banner");
        var servicesModule = Model.GetModule("services-grid");
        var testimonialModule = Model.GetModule("testimonials");
    }
    
    @if(heroModule != null)
    {
        <section class="hero-section">
            @await Html.PartialAsync($"../{heroModule.Template.FolderType.ToString()}/{heroModule.Template.FileName}.cshtml", heroModule, null)
        </section>
    }
    
    @if(servicesModule != null)
    {
        <section class="services-section py-5">
            <div class="container">
                @await Html.PartialAsync($"../{servicesModule.Template.FolderType.ToString()}/{servicesModule.Template.FileName}.cshtml", servicesModule, null)
            </div>
        </section>
    }
    
    @if(testimonialModule != null)
    {
        <section class="testimonials-section py-5 bg-light">
            <div class="container">
                @await Html.PartialAsync($"../{testimonialModule.Template.FolderType.ToString()}/{testimonialModule.Template.FileName}.cshtml", testimonialModule, null)
            </div>
        </section>
    }
</div>
```

---

## Creating Pages with Templates

### MCP Command Workflow
1. **Create the Template First:**
```csharp
CreateTemplate(
    folderType: 1,
    fileName: "HomePage",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<!-- template content -->"
)
```
---

## Advanced Page Patterns

### Conditional Content
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

@if (Model.SeoName == "home")
{
    <!-- Special home page content -->
    <div class="hero-banner">
        <h1>@Model.Title</h1>
        @Html.Raw(Model.Content)
    </div>
}
else
{
    <!-- Standard page layout -->
    <article class="standard-page">
        <h1>@Model.Title</h1>
        @Html.Raw(Model.Content)
    </article>
}
```

### Multi-Column Layouts
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-container">
    <div class="row">
        <div class="col-md-8">
            <main class="main-content">
                <h1>@Model.Title</h1>
                @Html.Raw(Model.Content)
            </main>
        </div>
        
        <div class="col-md-4">
            <aside class="sidebar">
                @await Html.PartialAsync("../Modules/Sidebar.cshtml")
            </aside>
        </div>
    </div>
</div>
```

---

## Best Practices

### Template Architecture
- **RenderBody() Usage:** Only call RenderBody() in master layout templates (folderType: 7)
- **Content Pages:** Should only define their specific content sections, not structural elements
- **Layout Inheritance:** Ensure proper hierarchy (master layout → page template → content)

### Content Structure
- **Semantic HTML:** Use proper heading hierarchy (h1, h2, h3)
- **Responsive Design:** Test on mobile devices
- **Accessibility:** Include alt text, proper labels
- **SEO-Friendly:** Use meaningful titles and meta descriptions

### Performance
- **Optimize Images:** Use appropriate sizes and formats
- **Minimize Database Calls:** Cache frequently accessed data
- **Lazy Loading:** Load non-critical content asynchronously

### Code Organization
- **Reusable Components:** Extract common patterns to modules
- **Clean Templates:** Keep logic minimal, focus on presentation
- **Error Handling:** Include null checks and fallbacks

---

## Troubleshooting

### Common Issues

**Content not rendering:**
- Check `@Html.Raw(Model.Content)` vs `@Model.Content`
- Verify model type: `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- Test with simple content first

**Modules not showing:**
- Verify module association with page
- Check module template exists and is correct type
- Test module rendering path: `$"../{folderType}/{fileName}.cshtml"`

**Database data not loading:**
- Verify table name and column names
- Check using statements and service injection
- Test database connection and permissions

### Debugging Tips
- Use browser developer tools to inspect HTML output
- Check Mix CMS logs for template errors
- Test templates with minimal content first
- Verify all IDs and references are correct

---

## Next Steps

After creating page templates:

1. **Create Content** - Use `CreatePageContent` to add pages
2. **Add Modules** - See [Modules Guide](./template-patterns-modules.md)
3. **Optimize SEO** - Add meta tags and structured data
4. **Test Performance** - Optimize loading times

---

## Related Guides

- **[Master Layouts](./template-patterns-masters.md)** - Creating the layout framework
- **[Module Templates](./template-patterns-modules.md)** - Reusable page components
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
