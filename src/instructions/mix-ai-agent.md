# Mix CMS AI Agent: Your Content Creation Assistant

Welcome! You're working with an AI assistant designed to help you build and manage websites using Mix CMS. This guide will walk you through the essential concepts and tools you'll need.

---

## 1. Core Concepts: Building Blocks of Your Website

Your website is made of two main things: **Templates** (the design and layout) and **Content** (the text and images).

### Templates: The Blueprint for Your Pages

Templates define how your content looks. Think of them as reusable blueprints. We have different types for different jobs:

-   **Master Layouts (`folderType: 7`):** The main skeleton of your site. This is where your site-wide header, footer, and navigation live. **Every page needs one.**
-   **Page Templates (`folderType: 1`):** The layout for a specific type of page, like a blog post or a contact page. It defines the content area within the Master Layout.
-   **Modules (`folderType: 2`):** Reusable blocks of content, like a contact form or an image gallery that can be placed on any page.

### Content: The Information on Your Pages

-   **Pages (`CreatePageContent`):** The actual webpages that your visitors see, like "Home" or "About". Each page uses a Page Template and a Master Layout to display its content. The `content` parameter should include HTML that will be rendered in the template.
-   **Posts (`CreatePostContent`):** Used for blog entries or news articles. The `content` parameter should include HTML for the post body that will be rendered in the template.
-   **Custom Data (`CreateDatabaseFromPrompt`):** For lists of things, like products, team members, or service offerings. Instead of hard-coding them into a page, you can store them in a database table and display them dynamically.

---

## 2. Your Toolbox: The MCP Commands

You have a set of powerful tools (MCP commands) to create and manage your site.

### For Managing Templates

-   `CreateTemplate`: Make a new template.
-   `ListTemplates`: See all existing templates.
-   `UpdateTemplate`: Change an existing template.
-   `DeleteTemplate`: Remove a template.

### For Managing Content

-   `CreatePageContent`: Create a new webpage.
-   `CreatePostContent`: Create a new blog post or article.
-   `CreateModuleContent`: Create a new reusable module.
-   `ListPageContents`: See all your pages.
-   `ListPostContents`: See all your posts.
-   `ListModuleContents`: See all your modules.
-   `UpdatePageContent`: Change a page's content or settings.
-   `UpdatePostContent`: Change a post's content or settings.
-   `UpdateModuleContent`: Change a module's content or settings.
-   `DeletePageContent`: Remove a page.
-   `DeletePostContent`: Remove a post.
-   `DeleteModuleContent`: Remove a module.

### For Managing Data

-   `CreateDatabaseFromPrompt`: Create a new database table from a simple description.
-   `CreateManyMixDbData`: Add multiple records (e.g., products, services) to a table at once.
-   `GetListMidxDbData`: Fetch data from a table to display on a page.

---

## 3. How-To Guides: Common Workflows

Here are step-by-step guides for the most common tasks.

### How to Create a New Webpage

Follow these three steps to create a fully functional webpage.

**Step 1: Create the Master Layout (Do this once per site)** First, create the main layout that all your pages will share.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 7` (This is important!)
    -   `fileName`: "MasterLayout.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the full HTML structure, including placeholders for navigation and the main body. **Must include the required Razor sections.**

**Step 2: Create the Page Template** Next, create the specific layout for the content of your new page.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 1`
    -   `fileName`: "HomePage.cshtml" or "ContactPage.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the HTML for the page's content area. Use a public URL for any sample images.

**Step 3: Create the Page Content** Finally, create the page itself and link it to the templates you just made.

-   **Tool:** `CreatePageContent`
-   **Parameters:**
    -   `title`: "Welcome to Our Website"
    -   `content`: The HTML content that will be rendered in the template (e.g., `"<h1>Welcome</h1><p>Our company provides excellent services...</p>"`)
    -   `seoName`: "home" (this becomes the URL, e.g., `yoursite.com/home`)
    -   `templateId`: The ID of the **Page Template** from Step 2.
    -   `layoutId`: The ID of the **Master Layout** from Step 1.
    -   `tenantId: 1`

### How to Create Reusable Modules with Dynamic Data

For reusable components that display dynamic content from database tables:

**Step 1: Create Database Table**
```csharp
CreateDatabaseFromPrompt(
    displayName: "Services",
    schemaDescription: "A table for services with fields: name (text), description (text), icon (text)"
)
```

**Step 1.1: Document Database Schema**
After creating the database, always document the schema in `database-schema.md`:
```markdown
## mix_services
- **objId** (int) - Primary key, auto-increment
- **name** (nvarchar) - Service name
- **description** (nvarchar) - Service description  
- **icon** (nvarchar) - CSS icon class
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp
```
Use `GetTableSchema` to verify the exact column names and data types before documenting.

**Step 2: Create Module Template**
- **Tool:** `CreateTemplate`
- **Parameters:**
  - `folderType: 2` (Modules)
  - `fileName`: "ServiceCard"
  - `mixThemeId: 1`
  - `content`: Use this pattern:
```razor
@using Mix.Lib.ViewModels
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
@foreach (var service in services)
{
  <div class="card">
    <div class="card-body">
      <i class="@service["icon"]"></i>
      <h3>@service["name"]</h3>
      <p>@service["description"]</p>
    </div>
  </div>
}
```

**Step 3: Include Module in Page Template**
```razor
@await Html.PartialAsync("../Modules/ServiceCard.cshtml")
```

**Complete Example: Services Page**
```razor
@using Mix.Lib.ViewModels
@model dynamic

<div class="container">
    <!-- Services Section -->
    <section>
        <h2>Our Services</h2>
        <div class="grid">
            @await Html.PartialAsync("../Modules/ServiceCard.cshtml")
        </div>
    </section>
    
    <!-- Testimonials Section --> 
    <section>
        <h2>Testimonials</h2>
        <div class="grid">
            @await Html.PartialAsync("../Modules/Testimonial.cshtml")
        </div>
    </section>
</div>
```

### How to Create Blog Posts

For blog entries or news articles:

**Step 1: Create a Post Template**
-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 5` (Posts)
    -   `fileName`: "BlogPost.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Start with `@model Mixcore.Domain.ViewModels.PostContentViewModel` and add your post layout HTML

**Step 2: Create the Post Content**
-   **Tool:** `CreatePostContent`
-   **Parameters:**
    -   `title`: "Getting Started with Our Platform"
    -   `content`: The HTML content for the post body (e.g., `"<p>This is the main content of our post...</p>"`)
    -   `excerpt`: Brief HTML summary for the post (e.g., `"<p>A short summary of this post...</p>"`)
    -   `seoName`: "getting-started-platform" (this becomes the URL)
    -   `tenantId: 1`

### How to Handle Lists of Items (e.g., Product Catalogs)

If your page has a list of similar items, don't hard-code them. Create a database table for them.

**Step 1: Create a Database Table** Use a simple prompt to define the structure of your data.

-   **Tool:** `CreateDatabaseFromPrompt`
-   **Example:** `CreateDatabaseFromPrompt(displayName: "Products", schemaDescription: "A table for products with fields for name (text), description (text), and price (decimal)")`

**Step 1.1: Document the Database Schema** After creating the database, always document the schema in `database-schema.md` to ensure you have the correct column names for templates:

```markdown
## mix_products
- **objId** (int) - Primary key, auto-increment
- **name** (nvarchar) - Product name
- **description** (nvarchar) - Product description
- **price** (decimal) - Product price
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp
```

Use `GetTableSchema(tableName: "mix_products")` to verify the exact column names and data types before documenting.

**Step 2: Add Your Data** Populate the table with your items.

-   **Tool:** `CreateManyMixDbData`
-   **Parameters:**
    -   `databaseSystemName`: "mix_products" (the name from Step 1)
    -   `dataJson`: A JSON array of your items.

**Step 3: Display the Data in a Template** Modify your Page or Module template to fetch and display the data.

1.  **Add the required using statements** at the top of your `.cshtml` file:
    ```razor
    @using Mix.Mixdb.Interfaces
    @using Mix.Shared.Models
    @using Mix.Shared.Dtos
    @using Mix.Constant.Enums
    @using Mix.Constant.Constants
    ```
2.  **Inject the service** after the using statements:
    ```razor
    @inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
    @inject IMixDbDataServiceFactory mixDbDataServiceFactory
    ```
3.  **Fetch the data** inside a code block:
    ```csharp
    @{
        var mixDbDataService = mixDbDataServiceFactory.Create(dbSrv.DatabaseProvider, dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION));
        var request = new SearchMixDbRequestModel
        {
            TableName = "mix_products",
            Queries = new List<MixQueryField>
            {
                new MixQueryField { FieldName = "is_featured", Value = true, CompareOperator = MixCompareOperator.Equal }
            }
        };
        var products = await mixDbDataServiceFactory.GetListByAsync(request);
    }
    ```
    
    **Common Query Examples:**
    ```csharp
    // Get all items (no filter)
    var request = new SearchMixDbRequestModel { TableName = "mix_products" };
    
    // Filter by category
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "category", Value = "electronics", CompareOperator = MixCompareOperator.Equal }
        }
    };
    
    // Search by name (contains)
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "name", Value = "smartphone", CompareOperator = MixCompareOperator.Like }
        }
    };
    
    // Multiple conditions (AND)
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "is_featured", Value = true, CompareOperator = MixCompareOperator.Equal },
            new MixQueryField { FieldName = "price", Value = 100, CompareOperator = MixCompareOperator.LessThan }
        }
    };
    ```
4.  **Loop through and display** the data:
    ```razor
    @foreach(var item in products)
    {
        <div class="product-item">
            <h3>@(item.Value<string>("name"))</h3>
            <p>@(item.Value<string>("description"))</p>
            <span>$@(item.Value<string>("price"))</span>
        </div>
    }
    ```

---

## 4. Best Practices & Key Reminders

-   **Check MCP Tool Support First:** Before executing any task, check if there's an existing MCP tool that can help accomplish it. Use `ListSections` to explore available tools and resources.
-   **Database Schema Documentation:** When creating new tables with MCP tools, ALWAYS document the schema in your project's `database-schema.md` file immediately after creation. Include table name, columns, data types, and relationships. Use `GetTableSchema` to retrieve exact schema details and verify column names before documenting. This ensures you use the correct field names when rendering data in templates.
-   **Schema Verification for Content Rendering:** Before creating content that loads data from MixDb, always check the database schema using `GetTableSchema` or refer to your `database-schema.md` documentation to ensure you understand the structure. This ensures you use the correct field names when rendering data in templates.
-   **Master Layouts First:** Always create your `folderType: 7` Master Layout before creating pages.
-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
    - The system will automatically combine them to create the full filename
-   **Module Rendering Pattern:**
    - Always create module content first using `CreateModuleContent`
    - Then create template with `.cshtml` extension
    - Associate posts/content using `CreateModulePostAssociation`
    - Render in templates using the naming pattern `$"../{folderType}/{templateName}.cshtml"`:
      ```razor
      @{
          var module = Model.GetModule("moduleSystemName");
      }
      @if(module != null){
          @await Html.PartialAsync($"../2/{module.Template.FileName}.cshtml", module, null);
      }
      ```
-   **Required Using Statements:** When using MixDb data in templates, always include these using statements at the top:
    ```razor
    @using Mix.Mixdb.Interfaces
    @using Mix.Shared.Models
    @using Mix.Shared.Dtos
    @using Mix.Constant.Enums
    @inject IMixDbDataService MixDbDataService
    ```
-   **Partial Rendering Pattern:** When rendering partial templates, always follow the naming pattern `$"../{folderType}/{templateName}.cshtml"`:
    - **Modules (folderType: 2):** `$"../2/{templateName}.cshtml"`
    - **Pages (folderType: 1):** `$"../1/{templateName}.cshtml"`
    - **Posts (folderType: 5):** `$"../5/{templateName}.cshtml"`
    - **Master Layouts (folderType: 7):** `$"../7/{templateName}.cshtml"`
    ```razor
    // Example for modules
    @await Html.PartialAsync($"../2/{module.Template.FileName}.cshtml", module, null);
    
    // Example for posts
    @await Html.PartialAsync($"../5/{post.Template.FileName}.cshtml", post, null);
    ```
-   **Public Image URLs:** When using images in templates, always use full, public URLs (e.g., from Unsplash). Do not use local file paths.
-   **Required Razor Sections:** Your Master Layout template **must** include these lines for styles and scripts to work correctly:
    ```razor
    @RenderSection("Schema", false)     
    @RenderSection("Seo", false)     
    <!--[STYLES]-->
    @RenderSection("Styles", false)   
    @RenderSection("Scripts", false)   
    ```
-   **Check for Existing Templates:** Use `ListTemplates` to avoid creating duplicates.

---

## 5. Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` to check first.
-   **Page is missing header/footer:** You likely forgot to assign the `layoutId` when you created the page with `CreatePageContent`. You can fix this with `UpdatePageContent`.
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.
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

## 6. Important Distinction: MCP Commands vs Template Code

**MCP Commands** are the tools you use to create and manage your site structure (like `CreateTemplate`, `CreatePageContent`, `GetListMidxDbData`). These are called through the AI assistant.

**Template Code** is the Razor/C# code you write inside your `.cshtml` templates to display data dynamically. This code uses the Mix CMS services directly.

### Template Models

Each template type has a specific model that provides access to content and data:

- **Page Templates:** Use `@model Mixcore.Domain.ViewModels.PageContentViewModel`
- **Module Templates:** Use `@model Mixcore.Domain.ViewModels.ModuleContentViewModel` 
- **Post Templates:** Use `@model Mixcore.Domain.ViewModels.PostContentViewModel`

These models provide access to the content properties, metadata, and related data for each content type.

### Rendering Content in Templates

When you create content using MCP commands (like `CreatePageContent`, `CreatePostContent`, `CreateModuleContent`), the `content` parameter should contain HTML that will be rendered in your templates:

**Page Templates:**
```razor
@model Mixcore.Domain.ViewModels.PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    @Html.Raw(Model.Content)  <!-- This renders the HTML content -->
</div>
```

**Module Templates:**
```razor
@model Mixcore.Domain.ViewModels.ModuleContentViewModel

<div class="module-content">
    <h2>@Model.Title</h2>
    @Html.Raw(Model.Excerpt)  <!-- This renders the HTML content for modules -->
</div>
```

**Post Templates:**
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="post-content">
    <h1>@Model.Title</h1>
    <div class="post-excerpt">@Html.Raw(Model.Excerpt)</div>
    <div class="post-body">@Html.Raw(Model.Content)</div>
</article>
```

**Important:** Use `@Html.Raw(Model.Content)` to render HTML content, or `@Model.Content` for plain text display.

### When to Use Which:

- **Use MCP Commands to:**
  - Create templates and pages
  - Set up database tables
  - Add initial data to tables
  - Manage your site structure

- **Use Template Code to:**
  - Display data from database tables on your web pages
  - Create dynamic content that changes based on data
  - Implement search, filtering, and pagination

### Example Workflow:
1. **MCP Command:** `CreateDatabaseFromPrompt` to create a "products" table
2. **Documentation:** Use `GetTableSchema` and document the schema in `database-schema.md`
3. **MCP Command:** `CreateManyMixDbData` to add product records
4. **Template Code:** Use `SearchMixDbRequestModel` and `IMixDbDataService` in your `.cshtml` file to display the products, referencing the documented column names

---

## 7. Quick Reference: Essential Commands

Here are the most common commands for quick access.

### Template & Page Creation Workflow
1.  **Create Master Layout:**
    `CreateTemplate(content, fileName: "MasterLayout.cshtml", folderType: 7, mixThemeId: 1)`
    *Returns the `master_layout_id`.*

2.  **Create Page Template:**
    `CreateTemplate(content, fileName: "MyPageTemplate.cshtml", folderType: 1, mixThemeId: 1)`
    *Returns the `page_template_id`.*

3.  **Create Page Content:**
    `CreatePageContent(title, content, seoName, excerpt, templateId: {page_template_id}, layoutId: {master_layout_id}, tenantId: 1)`

### Finding Your Content
-   **List Templates:** `ListTemplates(folderType: 7)`
-   **Get Page by URL:** `GetPageContentBySeoName(seoName: "home")`

---

## Additional Resources

- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete MCP command documentation with parameters and examples
- **[Developer Guide](./developer-guide.md)** - Technical guide for C# developers building MCP tools
- **[Instructions README](./README.md)** - Overview of all documentation files
