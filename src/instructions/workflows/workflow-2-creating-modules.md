
# Workflow 2: Creating Modules in Mix CMS

> **MixDb Data Value Rendering Instruction:**
> When rendering MixDb data values in templates, always use the following pattern:
>
> ```razor
> @(item.Value<[dataType]>("[column_system_name]"))
> ```
>
> - Replace `[dataType]` with the correct .NET type (e.g., `string`, `int`, `decimal`, `bool`, `DateTime`).
> - Replace `[column_system_name]` with the exact system name of the column.
> - The entire expression must be inside brackets `@(...)` when used standalone.
>
> **Example:**
> ```razor
> <span>@(item.Value<string>("name"))</span>
> <span>@(item.Value<decimal>("price"))</span>
> <span>@(item.Value<DateTime>("published_date"))</span>
> ```

---

## ModuleContentViewModel Reference

`ModuleContentViewModel` is the main ViewModel for reusable modules in Mix CMS. It provides:

- **Properties:**
  - `Title`: The module title.
  - `Excerpt`: Short summary or description.
  - `Content`: Main HTML content of the module.
  - `AdditionalData`: Holds extra data fields (dynamic columns, JSON).
- **Methods:**
  - `Property<T>(string fieldName)`: Get a strongly-typed value from `AdditionalData` by field name.

**Sample Usage:**

```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<h2>@(Model.Title)</h2>
<p>@(Model.Property<string>("custom_field"))</p>
<p>@(Model.AdditionalData?["another_field"])</p>
```

> **Note:** Always use `@(...)` when rendering standalone values in Razor.

> **Rendering ModuleContentViewModel Data:**
> ```razor
> <h2>@(Model.Title)</h2>
> <span>@(Model.Property<string>("custom_field"))</span>
> <span>@(Model.AdditionalData?["another_field"])</span>
> ```

This workflow covers creating reusable module components that can be embedded in pages, posts, or other modules.

---

## Prerequisites

Before creating modules, ensure you understand:
- Module template requirements (folderType: 2)
- Module content vs module templates
- How modules are rendered in pages

---

## Step 1: Create Module Content

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

**Key Parameters:**
- `title`: Display name for the module
- `excerpt`: Module description or configuration
- `systemName`: Unique identifier for the module
- `pageSize`: Number of items to display (if applicable)
- `type`: Module type (0 = standard)

---

## Step 2: Create Module Template

```markdown
CreateTemplate(
    folderType: 2,
    fileName: "FeaturedProducts",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.ModuleContentViewModel\n<div class=\"module featured-products\">\n    <h2>@Model.Title</h2>\n    @if (!string.IsNullOrEmpty(Model.Excerpt))\n    {\n        <div class=\"module-description\">\n            @Html.Raw(Model.Excerpt)\n        </div>\n    }\n</div>"
)
```

**CRITICAL**: When linking module content to a template, the `templateId` must reference a template with `folderType: 2` (Modules).

---

## Step 3: Use Module in Pages

### Direct Rendering
```razor
@await Html.PartialAsync("../Modules/FeaturedProducts.cshtml", moduleModel)
```

### Conditional Rendering
```razor
@{
    var featuredModule = Model.GetModule("featured-products");
}

@if(featuredModule != null)
{
    <section class="featured-section">
        @await Html.PartialAsync($"../{featuredModule.Template.FolderType}/{featuredModule.Template.FileName}.cshtml", featuredModule)
    </section>
}
```

---

## Module Template Examples

### Simple Content Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="module content-module">
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

### Database-Driven Module
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

<div class="products-module">
    <h2>Featured Products</h2>
    
    <div class="row">
        @foreach (var product in products.Take(3))
        {
            <div class="col-md-4 mb-4">
                <div class="product-card">
                    @if(!string.IsNullOrEmpty(product.Value<string>("image")))
                    {
                        <img src="@(product.Value<string>("image"))" 
                             alt="@(product.Value<string>("name"))" 
                             class="img-fluid">
                    }
                    <h4>@(product.Value<string>("name"))</h4>
                    <p>@(product.Value<string>("description"))</p>
                    <span class="price">$@(product.Value<decimal>("price"))</span>
                </div>
            </div>
        }
    </div>
</div>

<style>
    .product-card {
        border: 1px solid #ddd;
        border-radius: 8px;
        padding: 15px;
        text-align: center;
        height: 100%;
    }
    .product-card img {
        max-height: 200px;
        object-fit: cover;
        margin-bottom: 15px;
    }
    .price {
        font-weight: bold;
        color: #28a745;
        font-size: 1.2em;
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
            <a href="/products" class="btn btn-primary btn-lg">Shop Now</a>
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
        margin-bottom: 40px;
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

## Advanced Module Patterns

### Configurable Module
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

@{
    // Parse configuration from Excerpt (JSON format)
    dynamic config = null;
    try 
    {
        config = Newtonsoft.Json.JsonConvert.DeserializeObject(Model.Excerpt ?? "{}");
    }
    catch 
    {
        config = new { showImages = true, itemCount = 3, layout = "grid" };
    }
    
    var showImages = config?.showImages ?? true;
    var itemCount = config?.itemCount ?? 3;
    var layout = config?.layout ?? "grid";
}

<div class="configurable-module @layout-layout">
    <h3>@Model.Title</h3>
    
    @if (layout == "grid")
    {
        <div class="row">
            <!-- Grid layout content -->
        </div>
    }
    else if (layout == "list")
    {
        <div class="list-layout">
            <!-- List layout content -->
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
<div class="parameterized-module">
    <h3>@category.ToUpper() Items (Showing @limit)</h3>
    <!-- Dynamic content based on parameters -->
</div>
```

---

## Module Integration Patterns

### In Page Templates
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-with-modules">
    <!-- Page header -->
    <header>
        <h1>@Model.Title</h1>
    </header>
    
    <!-- Main content -->
    <main>
        @Html.Raw(Model.Content)
    </main>
    
    <!-- Module sections -->
    @await Html.PartialAsync("../Modules/FeaturedProducts.cshtml")
    @await Html.PartialAsync("../Modules/ContactInfo.cshtml")
</div>
```

### Dynamic Module Loading
```razor
@{
    var modules = Model.GetModules(); // Get all associated modules
}

@foreach(var module in modules)
{
    <section class="module-section">
        @await Html.PartialAsync($"../Modules/{module.Template.FileName}.cshtml", module)
    </section>
}
```

---

## Troubleshooting

### Common Issues:
- **Module not rendering**: Check partial path and folderType
- **Wrong template type**: Verify templateId has folderType: 2 (Modules)
- **Data not loading**: Check database connections and service injection
- **Styling conflicts**: Use scoped CSS or unique class names

### Verification Checklist:
1. ✅ Module content created with correct systemName
2. ✅ Module template exists with folderType: 2
3. ✅ Template file name matches usage in partial calls
4. ✅ Database queries are working (if using dynamic data)
5. ✅ CSS styling is scoped to module

---

## Best Practices

### Module Design:
- Keep modules self-contained with their own styles
- Use descriptive, unique systemName values
- Include error handling for database operations
- Make modules responsive for all devices

### Performance:
- Cache frequently accessed data
- Optimize database queries
- Minimize external resource loading
- Use efficient rendering patterns

### Reusability:
- Design for multiple contexts
- Avoid hard-coded dependencies
- Use configuration through Excerpt field
- Document module parameters

---

## Related Workflows

- **[Creating Pages](./workflow-1-creating-pages.md)** - Embedding modules in pages
- **[Working with Database Data](./workflow-4-database-data.md)** - Creating data-driven modules
- **[Module Patterns](../patterns/template-patterns-modules.md)** - Additional module examples
