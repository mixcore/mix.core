# AI Workflows: Working with MixDb Data

This guide covers creating and working with database-driven content, modules, and mixdb data in Mix CMS.

---

## Creating Reusable Modules with MixDb Data

For reusable components that display mixdb content from database tables:

### Step 1: Create Database Table
```csharp
CreateDatabaseFromPrompt(
    displayName: "Services",
    schemaDescription: "A table for services with fields: name (text), description (text), icon (text)"
)
```

### Step 1.1: Document Database Schema
**CRITICAL:** After creating the database, always document the schema in `database-schema.md`:
```markdown
## mix_services
- **id** (int) - Primary key, auto-increment
- **name** (nvarchar) - Service name
- **description** (nvarchar) - Service description  
- **icon** (nvarchar) - CSS icon class
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp

## Completed Tasks
### 2025-01-XX - Services Database Created
- Created `mix_services` table with fields: name, description, icon
- Added sample data with 5 service records
- Created ServiceCard.cshtml template in Modules folder
- Template renders services in card layout with icons
- **Status:** ✅ Complete - Ready for use in pages
```
Use `GetTableSchema` to verify the exact column names and data types before documenting.

**Note:** Always update the "Completed Tasks" section after successful task execution to help other team members understand what's been implemented.

### Step 2: Create Module Template
- **Tool:** `CreateTemplate`
- **Parameters:**
  - `folderType: 2` (Modules)
  - `fileName`: "ServiceCard"
  - `extension`: ".cshtml"
  - `mixThemeId: 1`
  - `content`: Use this pattern for database-driven module templates:

```razor
@model mixdb
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
    <div class="card bg-base-100 shadow-xl">
        <div class="card-body">
            <i class="@(service.Value<string>("icon")")></i>
            <h3 class="card-title">@(service.Value<string>("name"))</h3>
            <p>@(service.Value<string>("description"))</p>
        </div>
    </div>
}
```

### Step 3: Include Module in Page Template
```razor
@await Html.PartialAsync("../Modules/ServiceCard.cshtml")
```

---

## Handling Lists of Items (Product Catalogs, etc.)

If your page has a list of similar items, don't hard-code them. Create a database table for them.

### Step 1: Create a Database Table
Use a simple prompt to define the structure of your data.

-   **Tool:** `CreateDatabaseFromPrompt`
-   **Example:** `CreateDatabaseFromPrompt(displayName: "Products", schemaDescription: "A table for products with fields for name (text), description (text), and price (decimal)")`

### Step 1.1: Document the Database Schema
After creating the database, always document the schema in `database-schema.md` to ensure you have the correct column names for templates:

```markdown
## mix_products
- **id** (int) - Primary key, auto-increment
- **name** (nvarchar) - Product name
- **description** (nvarchar) - Product description
- **price** (decimal) - Product price
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp

## Completed Tasks
### 2025-01-XX - Products Database & Templates
- Created `mix_products` table with name, description, price fields
- Added 10 sample products with proper image URLs
- Created ProductCard.cshtml template for product display
- Integrated products into homepage
- **Status:** ✅ Complete - Products displaying correctly
```

Use `GetTableSchema(tableName: "mix_products")` to verify the exact column names and data types before documenting.

### Step 2: Add Your Data
Populate the table with your items.

-   **Tool:** `CreateManyMixDbData`
-   **Image URLs:** When using images in sample data, always use full, public URLs (e.g., from Unsplash like `https://images.unsplash.com/photo-...`). Do not use local file paths like `/images/photo.jpg` or relative paths.
-   **Parameters:**
    -   `databaseSystemName`: "mix_products" (the name from Step 1)
    -   `dataJson`: A JSON array of your items with full image URLs. (single line)

**Example with proper image URLs:**
```json
[
  {
    "name": "Wireless Headphones",
    "description": "High-quality wireless headphones",
    "price": 99.99,
    "image": "https://images.unsplash.com/photo-1505740420928-5e560c06d30e"
  },
  {
    "name": "Smart Watch",
    "description": "Feature-rich smartwatch",
    "price": 199.99,
    "image": "https://images.unsplash.com/photo-1523275335684-37898b6baf30"
  }
]
```

### Step 3: Display the Data in a Template
Modify your Page or Module template to fetch and display the data.

#### Required Setup in Templates

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

#### Fetching and Displaying Data

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
        var products = await mixDbDataService.GetListByAsync(request);
    }
    ```

#### Common Query Examples

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
            @if(!string.IsNullOrEmpty(item.Value<string>("image")))
            {
                <img src="@item.Value<string>("image")" alt="@item.Value<string>("name")" class="product-image" />
            }
            <h3>@(item.Value<string>("name"))</h3>
            <p>@(item.Value<string>("description"))</p>
            <span>$@(item.Value<string>("price"))</span>
        </div>
    }
    ```

**Note:** Always include null checks for optional fields like images, and use full URLs for image sources.

---

## Module Rendering Pattern

### Creating and Using Modules

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

### Complete Example: Services Page
```razor
@using Mixcore.Domain.ViewModels
@model mixdb

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

---

## Best Practices for MixDb Content

### Database Schema Documentation
- When creating new tables with MCP tools, **ALWAYS** document the schema in your project's `database-schema.md` file immediately after creation
- Include table name, columns, data types, and relationships
- Use `GetTableSchema` to retrieve exact schema details and verify column names before documenting
- This ensures you use the correct field names when rendering data in templates

### Task Completion Documentation
**CRITICAL:** After successfully executing any task, always document it in the appropriate markdown file:

**Required Documentation Format:**
```markdown
## Completed Tasks
### YYYY-MM-DD - [Task Description]
- **Action:** Brief description of what was done
- **Tables Created:** List any database tables
- **Templates Created:** List template files and their purpose
- **Data Added:** Summary of sample data added
- **Integration:** Where the feature was integrated
- **Status:** ✅ Complete / ⚠️ Needs Testing / ❌ Issues Found
- **Notes:** Any important details for future reference
```

**Why Documentation Matters:**
- Enables team collaboration and knowledge sharing
- Prevents duplicate work and conflicts
- Provides context for future modifications
- Helps troubleshooting when issues arise
- Maintains project history and decision tracking

### Schema Verification for Content Rendering
- Before creating content that loads data from MixDb, always check the database schema using `GetTableSchema` or refer to your `database-schema.md` documentation
- Ensure you understand the structure and use correct field names when rendering data in templates
- Field names are case-sensitive

### Image URL Best Practices
**Always use full, public URLs for images in sample data:**

✅ **Correct - Full public URLs:**
- `https://images.unsplash.com/photo-1505740420928-5e560c06d30e`
- `https://picsum.photos/300/200`
- `https://via.placeholder.com/300x200`

❌ **Incorrect - Local or relative paths:**
- `/images/photo.jpg`
- `./assets/image.png`
- `images/team-member.jpg`
- `photo.jpg`

**Why full URLs matter:**
- Templates render correctly in all environments
- No broken image links
- Content works immediately without file uploads
- Easier testing and development

### When to Use MixDb Data
Always use MixDb tables for repetitive or list-based content instead of hard-coding data directly into templates:

- Product listings
- Team member profiles  
- Testimonials or reviews
- Service offerings
- Portfolio items
- FAQ entries
- Any content that might need frequent updates

---

## Troubleshooting MixDb Data

### Common Issues

-   **Data not displaying:** Check that you have the correct using statements, service injection, and that your `SearchMixDbRequestModel` uses the right property names (`FieldName`, `CompareOperator`).
-   **Incorrect field names in templates:** If you're seeing null values or errors when rendering data, verify the database schema using `GetTableSchema` to ensure you're using the correct field names. Field names are case-sensitive.
-   **Wrong comparison operator:** Use `MixCompareOperator.Equal`, `MixCompareOperator.Like`, `MixCompareOperator.LessThan`, etc. (not `ExpressionMethod`).
-   **Broken images:** Always use full, public URLs (e.g., `https://images.unsplash.com/photo-...`) instead of local paths (`/images/photo.jpg`). Include null checks for optional image fields.

### Required Using Statements
When using MixDb data in templates, always include these using statements at the top:
```razor
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@inject IMixDbDataService MixDbDataService
```

---

## Next Steps

Once you've mastered mixdb data:
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog functionality
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
- **[Mix CMS Reference](./mix-cms-reference.md)** - Enums, constants, and technical reference
- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
