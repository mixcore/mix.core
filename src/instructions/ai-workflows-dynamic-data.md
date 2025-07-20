# AI Workflows: Working with Dynamic Data

This guide covers creating and working with database-driven content, modules, and dynamic data in Mix CMS.

---

## Creating Reusable Modules with Dynamic Data

For reusable components that display dynamic content from database tables:

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
- **objId** (int) - Primary key, auto-increment
- **name** (nvarchar) - Service name
- **description** (nvarchar) - Service description  
- **icon** (nvarchar) - CSS icon class
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp
```
Use `GetTableSchema` to verify the exact column names and data types before documenting.

### Step 2: Create Module Template
- **Tool:** `CreateTemplate`
- **Parameters:**
  - `folderType: 2` (Modules)
  - `fileName`: "ServiceCard"
  - `extension`: ".cshtml"
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
- **objId** (int) - Primary key, auto-increment
- **name** (nvarchar) - Product name
- **description** (nvarchar) - Product description
- **price** (decimal) - Product price
- **createdDateTime** (datetime) - Record creation timestamp
- **modifiedDateTime** (datetime) - Last modification timestamp
```

Use `GetTableSchema(tableName: "mix_products")` to verify the exact column names and data types before documenting.

### Step 2: Add Your Data
Populate the table with your items.

-   **Tool:** `CreateManyMixDbData`
-   **Parameters:**
    -   `databaseSystemName`: "mix_products" (the name from Step 1)
    -   `dataJson`: A JSON array of your items.

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
        var products = await mixDbDataServiceFactory.GetListByAsync(request);
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
            <h3>@(item.Value<string>("name"))</h3>
            <p>@(item.Value<string>("description"))</p>
            <span>$@(item.Value<string>("price"))</span>
        </div>
    }
    ```

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

---

## Best Practices for Dynamic Content

### Database Schema Documentation
- When creating new tables with MCP tools, **ALWAYS** document the schema in your project's `database-schema.md` file immediately after creation
- Include table name, columns, data types, and relationships
- Use `GetTableSchema` to retrieve exact schema details and verify column names before documenting
- This ensures you use the correct field names when rendering data in templates

### Schema Verification for Content Rendering
- Before creating content that loads data from MixDb, always check the database schema using `GetTableSchema` or refer to your `database-schema.md` documentation
- Ensure you understand the structure and use correct field names when rendering data in templates
- Field names are case-sensitive

### When to Use Dynamic Data
Always use MixDb tables for repetitive or list-based content instead of hard-coding data directly into templates:

- Product listings
- Team member profiles  
- Testimonials or reviews
- Service offerings
- Portfolio items
- FAQ entries
- Any content that might need frequent updates

---

## Troubleshooting Dynamic Data

### Common Issues

-   **Data not displaying:** Check that you have the correct using statements, service injection, and that your `SearchMixDbRequestModel` uses the right property names (`FieldName`, `CompareOperator`).
-   **Incorrect field names in templates:** If you're seeing null values or errors when rendering data, verify the database schema using `GetTableSchema` to ensure you're using the correct field names. Field names are case-sensitive.
-   **Wrong comparison operator:** Use `MixCompareOperator.Equal`, `MixCompareOperator.Like`, `MixCompareOperator.LessThan`, etc. (not `ExpressionMethod`).

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

Once you've mastered dynamic data:
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog functionality
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
- **[Mix CMS Reference](./mix-cms-reference.md)** - Enums, constants, and technical reference
- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
