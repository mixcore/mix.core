# Module Templates (folderType: 2)

Module templates create reusable components that can be embedded in pages, posts, or other modules. They provide dynamic content blocks and interactive features.

---

## Overview

Module templates (`folderType: 2`) are reusable components that can display dynamic content, database data, or provide specific functionality. They can be embedded anywhere in your website.

### Key Characteristics
- **Purpose:** Reusable content components and widgets
- **Model:** `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` or `@model dynamic`
- **Usage:** Embedded in pages via `@await Html.PartialAsync()`
- **Flexibility:** Can display static content or dynamic database data

---

## Creating Module Templates

### MCP Command
```csharp
CreateTemplate(
    folderType: 2,
    fileName: "ServiceCard",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.ModuleContentViewModel\n<div class=\"module\"><h3>@Model.Title</h3>@Html.Raw(Model.Excerpt)</div>"
)
```

### Basic Module Template Structure
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="module-container">
    <h3>@Model.Title</h3>
    
    @if (!string.IsNullOrEmpty(Model.Excerpt))
    {
        <div class="module-content">
            @Html.Raw(Model.Excerpt)
        </div>
    }
</div>
```

---

## Module Template Examples

### Simple Content Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="content-module">
    <div class="module-header">
        <h3>@Model.Title</h3>
    </div>
    <div class="module-body">
        @Html.Raw(Model.Excerpt)
    </div>
</div>

<style>
    .content-module {
        padding: 20px;
        border: 1px solid #e9ecef;
        border-radius: 8px;
        margin-bottom: 20px;
    }
    .module-header h3 {
        margin-bottom: 15px;
        color: #495057;
    }
</style>
```

### Hero Banner Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="hero-banner">
    <div class="hero-content">
        <h1 class="hero-title">@Model.Title</h1>
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <p class="hero-subtitle">@Html.Raw(Model.Excerpt)</p>
        }
        <div class="hero-actions">
            <a href="/services" class="btn btn-primary btn-lg">Get Started</a>
            <a href="/about" class="btn btn-outline-light btn-lg ml-3">Learn More</a>
        </div>
    </div>
</div>

<style>
    .hero-banner {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 80px 0;
        text-align: center;
    }
    .hero-title {
        font-size: 3.5rem;
        font-weight: bold;
        margin-bottom: 1rem;
    }
    .hero-subtitle {
        font-size: 1.3rem;
        margin-bottom: 2rem;
        opacity: 0.9;
    }
    .hero-actions .btn {
        margin: 0 10px;
    }
</style>
```

### Services Grid Module
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
        TableName = "mix_services",
        Queries = new List<MixQueryField>()
    };
    var services = await mixDbDataService.GetListByAsync(request);
}

<div class="services-module">
    <div class="text-center mb-5">
        <h2>Our Services</h2>
        <p class="lead">Comprehensive solutions for your business needs</p>
    </div>
    
    <div class="row">
        @foreach (var service in services)
        {
            <div class="col-md-4 mb-4">
                <div class="service-card">
                    @if(!string.IsNullOrEmpty(service.Value<string>("icon")))
                    {
                        <div class="service-icon">
                            <i class="@(service.Value<string>("icon"))"></i>
                        </div>
                    }
                    <h4>@(service.Value<string>("name"))</h4>
                    <p>@(service.Value<string>("description"))</p>
                    @if(!string.IsNullOrEmpty(service.Value<string>("link")))
                    {
                        <a href="@(service.Value<string>("link"))" class="btn btn-outline-primary">Learn More</a>
                    }
                </div>
            </div>
        }
    </div>
</div>

<style>
    .service-card {
        text-align: center;
        padding: 30px 20px;
        border: 1px solid #e9ecef;
        border-radius: 10px;
        height: 100%;
        transition: transform 0.3s ease;
    }
    .service-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 5px 20px rgba(0,0,0,0.1);
    }
    .service-icon {
        font-size: 3rem;
        color: #007bff;
        margin-bottom: 20px;
    }
    .service-card h4 {
        margin-bottom: 15px;
        color: #333;
    }
</style>
```

### Testimonials Module
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
        TableName = "mix_testimonials",
        Queries = new List<MixQueryField>()
    };
    var testimonials = await mixDbDataService.GetListByAsync(request);
}

<div class="testimonials-module">
    <div class="text-center mb-5">
        <h2>What Our Clients Say</h2>
        <p class="lead">Don't just take our word for it</p>
    </div>
    
    <div class="testimonials-carousel">
        <div class="row">
            @foreach (var testimonial in testimonials.Take(3))
            {
                <div class="col-md-4 mb-4">
                    <div class="testimonial-card">
                        <div class="testimonial-content">
                            <p>"@(testimonial.Value<string>("content"))"</p>
                        </div>
                        <div class="testimonial-author">
                            @if(!string.IsNullOrEmpty(testimonial.Value<string>("avatar")))
                            {
                                <img src="@(testimonial.Value<string>("avatar"))" 
                                     alt="@(testimonial.Value<string>("name"))" 
                                     class="author-avatar">
                            }
                            <div class="author-info">
                                <h5>@(testimonial.Value<string>("name"))</h5>
                                <p>@(testimonial.Value<string>("position")), @(testimonial.Value<string>("company"))</p>
                            </div>
                        </div>
                    </div>
                </div>
            }
        </div>
    </div>
</div>

<style>
    .testimonial-card {
        background: #fff;
        padding: 30px;
        border-radius: 10px;
        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        height: 100%;
    }
    .testimonial-content {
        margin-bottom: 20px;
    }
    .testimonial-content p {
        font-style: italic;
        font-size: 1.1rem;
        line-height: 1.6;
    }
    .testimonial-author {
        display: flex;
        align-items: center;
    }
    .author-avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 15px;
    }
    .author-info h5 {
        margin: 0;
        font-size: 1rem;
    }
    .author-info p {
        margin: 0;
        font-size: 0.9rem;
        color: #666;
    }
</style>
```

### News/Blog Module
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
        TableName = "mix_post",
        Queries = new List<MixQueryField>(),
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "CreatedDateTime", Direction = "Desc" } 
        }
    };
    var posts = await mixDbDataService.GetListByAsync(request);
}

<div class="news-module">
    <div class="section-header mb-4">
        <h2>Latest News</h2>
        <a href="/blog" class="btn btn-outline-primary">View All Posts</a>
    </div>
    
    <div class="row">
        @foreach (var post in posts.Take(3))
        {
            <div class="col-md-4 mb-4">
                <article class="news-card">
                    @if(!string.IsNullOrEmpty(post.Value<string>("FeaturedImage")))
                    {
                        <div class="news-image">
                            <img src="@(post.Value<string>("FeaturedImage"))" 
                                 alt="@(post.Value<string>("Title"))" 
                                 class="img-fluid">
                        </div>
                    }
                    <div class="news-content">
                        <h4>@(post.Value<string>("Title"))</h4>
                        <p class="news-excerpt">@(post.Value<string>("Excerpt"))</p>
                        <div class="news-meta">
                            <span class="date">@(post.Value<DateTime>("CreatedDateTime").ToString("MMM dd, yyyy"))</span>
                            <a href="/post/@(post.Value<string>("SeoName"))" class="read-more">Read More</a>
                        </div>
                    </div>
                </article>
            </div>
        }
    </div>
</div>

<style>
    .news-module .section-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .news-card {
        border: 1px solid #e9ecef;
        border-radius: 8px;
        overflow: hidden;
        height: 100%;
        transition: box-shadow 0.3s ease;
    }
    .news-card:hover {
        box-shadow: 0 4px 15px rgba(0,0,0,0.1);
    }
    .news-image img {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }
    .news-content {
        padding: 20px;
    }
    .news-content h4 {
        margin-bottom: 10px;
    }
    .news-excerpt {
        color: #666;
        margin-bottom: 15px;
    }
    .news-meta {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 0.9rem;
    }
    .date {
        color: #888;
    }
    .read-more {
        color: #007bff;
        text-decoration: none;
    }
</style>
```

### Contact Info Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="contact-info-module">
    <h3>@Model.Title</h3>
    
    <div class="contact-items">
        <div class="contact-item">
            <i class="fas fa-map-marker-alt"></i>
            <div class="contact-details">
                <h5>Address</h5>
                <p>123 Business Street<br>City, State 12345</p>
            </div>
        </div>
        
        <div class="contact-item">
            <i class="fas fa-phone"></i>
            <div class="contact-details">
                <h5>Phone</h5>
                <p>+1 (555) 123-4567</p>
            </div>
        </div>
        
        <div class="contact-item">
            <i class="fas fa-envelope"></i>
            <div class="contact-details">
                <h5>Email</h5>
                <p>contact@example.com</p>
            </div>
        </div>
        
        <div class="contact-item">
            <i class="fas fa-clock"></i>
            <div class="contact-details">
                <h5>Business Hours</h5>
                <p>Mon - Fri: 9:00 AM - 6:00 PM<br>Sat: 10:00 AM - 4:00 PM</p>
            </div>
        </div>
    </div>
    
    @if (!string.IsNullOrEmpty(Model.Excerpt))
    {
        <div class="additional-info">
            @Html.Raw(Model.Excerpt)
        </div>
    }
</div>

<style>
    .contact-info-module {
        background: #f8f9fa;
        padding: 30px;
        border-radius: 10px;
    }
    .contact-items {
        margin-top: 20px;
    }
    .contact-item {
        display: flex;
        align-items: flex-start;
        margin-bottom: 20px;
    }
    .contact-item i {
        font-size: 1.2rem;
        color: #007bff;
        margin-right: 15px;
        margin-top: 5px;
    }
    .contact-details h5 {
        margin-bottom: 5px;
        font-size: 1rem;
    }
    .contact-details p {
        margin: 0;
        color: #666;
    }
</style>
```



---

## Rendering Modules in Pages

### Direct Rendering
```razor
<!-- In a page template -->
@await Html.PartialAsync("../Modules/ServicesGrid.cshtml")
```

### Conditional Rendering
```razor
@{
    var servicesModule = Model.GetModule("services-grid");
}

@if(servicesModule != null)
{
    <section class="services-section">
        @await Html.PartialAsync($"../{servicesModule.Template.FolderType.ToString()}/{servicesModule.Template.FileName}.cshtml", servicesModule, null)
    </section>
}
```

### Dynamic Module Loading
```razor
@{
    var modules = Model.GetModules(); // Get all associated modules
}

@foreach(var module in modules)
{
    <section class="module-section">
        @await Html.PartialAsync($"../{module.Template.FolderType.ToString()}/{module.Template.FileName}.cshtml", module, null)
    </section>
}
```

---

## Advanced Module Patterns

### Configurable Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

@{
    // Parse configuration from Excerpt (JSON format)
    var config = Json.Deserialize<dynamic>(Model.Excerpt ?? "{}");
    var showImages = config?.showImages ?? true;
    var itemCount = config?.itemCount ?? 3;
    var layout = config?.layout ?? "grid";
}

<div class="configurable-module @layout-layout">
    <h3>@Model.Title</h3>
    
    <!-- Render based on configuration -->
    @if (layout == "grid")
    {
        <div class="row">
            <!-- Grid layout -->
        </div>
    }
    else if (layout == "list")
    {
        <div class="list-layout">
            <!-- List layout -->
        </div>
    }
</div>
```

### Module with Parameters
```razor
@model dynamic

@{
    var category = ViewBag.Category ?? "all";
    var limit = ViewBag.Limit ?? 6;
}

<!-- Module content filtered by parameters -->
```

### Nested Modules
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="complex-module">
    <h3>@Model.Title</h3>
    
    <!-- Render child modules -->
    @await Html.PartialAsync("../Modules/ChildModule1.cshtml")
    @await Html.PartialAsync("../Modules/ChildModule2.cshtml")
</div>
```

---

## Template Structure Reference

### Basic Model Usage
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<!-- Access standard properties -->
@Model.Title
@Model.Excerpt
@Model.Status
@Model.CreatedDateTime
```

### Dynamic Model Usage
```razor
@model dynamic

<!-- For database-driven modules -->
@{
    var data = await GetDataAsync();
}
```

---

## Related Guides

- **[Page Templates](./template-patterns-pages.md)** - Embedding modules in pages
- **[Master Layouts](./template-patterns-masters.md)** - Overall site structure
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
