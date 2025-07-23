# Workflow 4: Working with Database Data in Mix CMS

This workflow covers creating databases, managing data, and creating data-driven templates and modules.

---

## Prerequisites

Before working with database data, ensure you understand:
- Mix CMS database creation patterns
- Data-driven template service injection
- Database naming conventions
- Documentation requirements

---

## Step 1: Create Database Table

```markdown
CreateDatabaseFromPrompt(
    displayName: "Products",
    schemaDescription: "A table for products with name (text), description (text), price (decimal), image (text), category (text), featured (boolean)"
)
```

**Best Practices for Schema Description:**
- Be specific about data types (text, decimal, boolean, date)
- Include all required fields
- Mention relationships to other tables if needed
- Consider indexing for frequently queried fields

---

## Step 2: Document Schema (CRITICAL)

Update `database-schema.md`:
```markdown
## mix_products
- **id** (int) - Primary key, auto-increment
- **name** (nvarchar(255)) - Product name, required
- **description** (nvarchar(max)) - Product description
- **price** (decimal(18,2)) - Product price in USD
- **image** (nvarchar(500)) - Full image URL (e.g., https://images.unsplash.com/...)
- **category** (nvarchar(100)) - Product category
- **featured** (bit) - Whether product is featured (0/1)
- **CreatedDateTime** (datetime2) - Auto-generated creation timestamp
- **ModifiedDateTime** (datetime2) - Auto-generated modification timestamp
- **Status** (int) - Record status (0=Inactive, 1=Active)

### Sample Data:
- Electronics: Laptop ($999.99), Smartphone ($599.99)
- Clothing: T-Shirt ($19.99), Jeans ($49.99)
- Books: Programming Guide ($29.99), Design Handbook ($39.99)
```

---

## Step 3: Add Sample Data

### Single Record
```markdown
CreateMixDbData(
    databaseSystemName: "mix_products",
    dataJson: '{"name":"Wireless Headphones","description":"High-quality audio with noise cancellation","price":99.99,"image":"https://images.unsplash.com/photo-1505740420928-5e560c06d30e","category":"Electronics","featured":true}'
)
```

### Multiple Records
```markdown
CreateManyMixDbData(
    databaseSystemName: "mix_products",
    dataJson: '[
        {"name":"Laptop Pro","description":"Professional laptop for developers","price":1299.99,"image":"https://images.unsplash.com/photo-1496181133206-80ce9b88a853","category":"Electronics","featured":true},
        {"name":"Cotton T-Shirt","description":"Comfortable cotton t-shirt","price":19.99,"image":"https://images.unsplash.com/photo-1521572163474-6864f9cf17ab","category":"Clothing","featured":false},
        {"name":"Programming Book","description":"Complete guide to modern programming","price":29.99,"image":"https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c","category":"Books","featured":true}
    ]'
)
```

---

## Step 4: Create Data-Driven Templates

### Basic Data Display Template
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

<div class="products-grid">
    <h2>Our Products</h2>
    
    <div class="row">
        @foreach (var product in products)
        {
            <div class="col-md-4 mb-4">
                <div class="product-card">
                    @if(!string.IsNullOrEmpty(product.Value<string>("image")))
                    {
                        <img src="@(product.Value<string>("image"))" 
                             alt="@(product.Value<string>("name"))" 
                             class="product-image">
                    }
                    <div class="product-info">
                        <h4>@(product.Value<string>("name"))</h4>
                        <p class="product-description">@(product.Value<string>("description"))</p>
                        <div class="product-meta">
                            <span class="product-price">$@(product.Value<decimal>("price"))</span>
                            <span class="product-category">@(product.Value<string>("category"))</span>
                        </div>
                        @if(product.Value<bool>("featured"))
                        {
                            <span class="featured-badge">Featured</span>
                        }
                    </div>
                </div>
            </div>
        }
    </div>
</div>

<style>
    .products-grid {
        margin: 40px 0;
    }
    .product-card {
        border: 1px solid #ddd;
        border-radius: 8px;
        overflow: hidden;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        height: 100%;
    }
    .product-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 5px 20px rgba(0,0,0,0.1);
    }
    .product-image {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }
    .product-info {
        padding: 20px;
        position: relative;
    }
    .product-description {
        color: #666;
        margin-bottom: 15px;
    }
    .product-meta {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .product-price {
        font-size: 1.2em;
        font-weight: bold;
        color: #28a745;
    }
    .product-category {
        background: #f8f9fa;
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 0.8em;
        color: #666;
    }
    .featured-badge {
        position: absolute;
        top: 10px;
        right: 10px;
        background: #ff6b6b;
        color: white;
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 0.7em;
        font-weight: bold;
    }
</style>
```

### Filtered Data Template
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
    
    // Get featured products only
    var featuredRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "featured", Value = "true", Method = "Equal" }
        },
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "price", Direction = "Desc" } 
        }
    };
    var featuredProducts = await mixDbDataService.GetListByAsync(featuredRequest);
    
    // Get products by category
    var electronicsRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "category", Value = "Electronics", Method = "Equal" }
        }
    };
    var electronics = await mixDbDataService.GetListByAsync(electronicsRequest);
}

<div class="filtered-products">
    <!-- Featured Products Section -->
    <section class="featured-section">
        <h2>Featured Products</h2>
        <div class="row">
            @foreach (var product in featuredProducts.Take(3))
            {
                <div class="col-md-4 mb-4">
                    <div class="featured-product-card">
                        <div class="featured-badge">⭐ Featured</div>
                        <img src="@(product.Value<string>("image"))" 
                             alt="@(product.Value<string>("name"))" 
                             class="img-fluid">
                        <div class="product-content">
                            <h4>@(product.Value<string>("name"))</h4>
                            <p>@(product.Value<string>("description"))</p>
                            <div class="price-tag">$@(product.Value<decimal>("price"))</div>
                        </div>
                    </div>
                </div>
            }
        </div>
    </section>
    
    <!-- Electronics Category -->
    <section class="category-section">
        <h2>Electronics</h2>
        <div class="row">
            @foreach (var product in electronics)
            {
                <div class="col-md-6 mb-3">
                    <div class="category-product-card">
                        <div class="row no-gutters">
                            <div class="col-md-4">
                                <img src="@(product.Value<string>("image"))" 
                                     alt="@(product.Value<string>("name"))" 
                                     class="category-product-image">
                            </div>
                            <div class="col-md-8">
                                <div class="card-body">
                                    <h5>@(product.Value<string>("name"))</h5>
                                    <p>@(product.Value<string>("description"))</p>
                                    <p class="price">$@(product.Value<decimal>("price"))</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            }
        </div>
    </section>
</div>

<style>
    .featured-section, .category-section {
        margin-bottom: 50px;
    }
    .featured-product-card {
        position: relative;
        border: 2px solid #ffd700;
        border-radius: 10px;
        overflow: hidden;
        background: white;
    }
    .featured-badge {
        position: absolute;
        top: 15px;
        left: 15px;
        background: #ffd700;
        color: #333;
        padding: 5px 10px;
        border-radius: 20px;
        font-size: 0.8em;
        font-weight: bold;
        z-index: 10;
    }
    .product-content {
        padding: 20px;
    }
    .price-tag {
        background: #28a745;
        color: white;
        padding: 8px 15px;
        border-radius: 20px;
        display: inline-block;
        font-weight: bold;
    }
    .category-product-card {
        border: 1px solid #ddd;
        border-radius: 8px;
        overflow: hidden;
    }
    .category-product-image {
        width: 100%;
        height: 120px;
        object-fit: cover;
    }
</style>
```

### Paginated Data Template
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
    
    int currentPage = int.TryParse(Context.Request.Query["page"], out int page) ? page : 1;
    int pageSize = 6;
    
    var paginatedRequest = new GetPagingMixDbRequestModel
    {
        TableName = "mix_products",
        Page = currentPage,
        PageSize = pageSize,
        Queries = new List<MixQueryField>(),
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "CreatedDateTime", Direction = "Desc" } 
        }
    };
    
    var result = await mixDbDataService.GetPagingByAsync(paginatedRequest);
    var products = result.Items;
    var totalPages = (int)Math.Ceiling((double)result.TotalItems / pageSize);
}

<div class="paginated-products">
    <div class="products-header">
        <h2>All Products</h2>
        <p>Showing @((currentPage - 1) * pageSize + 1) - @(Math.Min(currentPage * pageSize, result.TotalItems)) of @result.TotalItems products</p>
    </div>
    
    <div class="row">
        @foreach (var product in products)
        {
            <div class="col-md-4 col-sm-6 mb-4">
                <div class="product-card">
                    <img src="@(product.Value<string>("image"))" 
                         alt="@(product.Value<string>("name"))" 
                         class="product-image">
                    <div class="product-body">
                        <h5>@(product.Value<string>("name"))</h5>
                        <p class="product-category">@(product.Value<string>("category"))</p>
                        <p class="product-description">@(product.Value<string>("description"))</p>
                        <div class="product-price">$@(product.Value<decimal>("price"))</div>
                    </div>
                </div>
            </div>
        }
    </div>
    
    <!-- Pagination -->
    @if (totalPages > 1)
    {
        <nav aria-label="Product pagination">
            <ul class="pagination justify-content-center">
                @if (currentPage > 1)
                {
                    <li class="page-item">
                        <a class="page-link" href="?page=@(currentPage - 1)">Previous</a>
                    </li>
                }
                
                @for (int i = Math.Max(1, currentPage - 2); i <= Math.Min(totalPages, currentPage + 2); i++)
                {
                    <li class="page-item @(i == currentPage ? "active" : "")">
                        <a class="page-link" href="?page=@i">@i</a>
                    </li>
                }
                
                @if (currentPage < totalPages)
                {
                    <li class="page-item">
                        <a class="page-link" href="?page=@(currentPage + 1)">Next</a>
                    </li>
                }
            </ul>
        </nav>
    }
</div>

<style>
    .products-header {
        text-align: center;
        margin-bottom: 40px;
    }
    .product-card {
        border: 1px solid #e9ecef;
        border-radius: 8px;
        overflow: hidden;
        height: 100%;
        transition: transform 0.2s ease;
    }
    .product-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }
    .product-image {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }
    .product-body {
        padding: 20px;
    }
    .product-category {
        color: #6c757d;
        font-size: 0.8em;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        margin-bottom: 10px;
    }
    .product-description {
        font-size: 0.9em;
        color: #666;
        margin-bottom: 15px;
    }
    .product-price {
        font-size: 1.2em;
        font-weight: bold;
        color: #28a745;
    }
</style>
```

---

## Database Query Patterns

### Basic Query (All Records)
```csharp
var request = new SearchMixDbRequestModel
{
    TableName = "mix_products",
    Queries = new List<MixQueryField>()
};
var products = await mixDbDataService.GetListByAsync(request);
```

### Filtered Query
```csharp
var request = new SearchMixDbRequestModel
{
    TableName = "mix_products",
    Queries = new List<MixQueryField>
    {
        new MixQueryField { FieldName = "category", Value = "Electronics", Method = "Equal" },
        new MixQueryField { FieldName = "price", Value = "100", Method = "GreaterThan" }
    }
};
```

### Sorted Query
```csharp
var request = new SearchMixDbRequestModel
{
    TableName = "mix_products",
    Sorts = new List<MixSort> 
    { 
        new MixSort { FieldName = "price", Direction = "Desc" },
        new MixSort { FieldName = "name", Direction = "Asc" }
    }
};
```

### Search Query
```csharp
var request = new SearchMixDbRequestModel
{
    TableName = "mix_products",
    Queries = new List<MixQueryField>
    {
        new MixQueryField { FieldName = "name", Value = "laptop", Method = "Contains" }
    }
};
```

---

## Data Management Operations

### Update Data
```markdown
UpdateMixDbData(
    databaseSystemName: "mix_products",
    strId: "1",
    dataJson: '{"name":"Updated Product Name","price":149.99,"featured":true}'
)
```

### Delete Data
```markdown
DeleteMixDbData(
    databaseSystemName: "mix_products",
    id: 1
)
```

### Get Single Record
```markdown
GetMixDbDataById(
    databaseSystemName: "mix_products",
    id: 1
)
```

---

## Advanced Database Patterns

### Dynamic Category Filtering
```razor
@{
    // Get all unique categories
    var categoryRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        SelectColumns = "category",
        Queries = new List<MixQueryField>()
    };
    var allProducts = await mixDbDataService.GetListByAsync(categoryRequest);
    var categories = allProducts.Select(p => p.Value<string>("category")).Distinct().ToList();
    
    // Get selected category from query string
    var selectedCategory = Context.Request.Query["category"].ToString();
    
    // Filter products by category
    var productRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = string.IsNullOrEmpty(selectedCategory) ? 
            new List<MixQueryField>() :
            new List<MixQueryField>
            {
                new MixQueryField { FieldName = "category", Value = selectedCategory, Method = "Equal" }
            }
    };
    var products = await mixDbDataService.GetListByAsync(productRequest);
}

<div class="category-filter">
    <h2>Products @(!string.IsNullOrEmpty(selectedCategory) ? $"in {selectedCategory}" : "")</h2>
    
    <!-- Category Filter -->
    <div class="filter-buttons">
        <a href="?" class="btn @(string.IsNullOrEmpty(selectedCategory) ? "btn-primary" : "btn-outline-primary")">All</a>
        @foreach (var category in categories)
        {
            <a href="?category=@category" class="btn @(selectedCategory == category ? "btn-primary" : "btn-outline-primary")">@category</a>
        }
    </div>
    
    <!-- Products Display -->
    <div class="row mt-4">
        @foreach (var product in products)
        {
            <div class="col-md-4 mb-4">
                <!-- Product card content -->
            </div>
        }
    </div>
</div>
```

### Price Range Filtering
```razor
@{
    // Get price range from query
    decimal.TryParse(Context.Request.Query["minPrice"], out decimal minPrice);
    decimal.TryParse(Context.Request.Query["maxPrice"], out decimal maxPrice);
    
    var queries = new List<MixQueryField>();
    if (minPrice > 0)
        queries.Add(new MixQueryField { FieldName = "price", Value = minPrice.ToString(), Method = "GreaterThanOrEqual" });
    if (maxPrice > 0)
        queries.Add(new MixQueryField { FieldName = "price", Value = maxPrice.ToString(), Method = "LessThanOrEqual" });
    
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_products",
        Queries = queries
    };
    var products = await mixDbDataService.GetListByAsync(request);
}

<div class="price-filter">
    <form method="get" class="filter-form">
        <div class="row">
            <div class="col-md-3">
                <input type="number" name="minPrice" placeholder="Min Price" value="@minPrice" class="form-control">
            </div>
            <div class="col-md-3">
                <input type="number" name="maxPrice" placeholder="Max Price" value="@maxPrice" class="form-control">
            </div>
            <div class="col-md-3">
                <button type="submit" class="btn btn-primary">Filter</button>
            </div>
        </div>
    </form>
    
    <!-- Products display -->
</div>
```

---

## Troubleshooting

### Common Issues:
- **Data not displaying**: Check table name spelling and service injection
- **Wrong data types**: Verify Value<T>() type matches database column type
- **Query errors**: Check field names and query methods
- **Performance issues**: Add appropriate sorting and pagination

### Verification Checklist:
1. ✅ Database table exists with correct name
2. ✅ Sample data has been added
3. ✅ Service injection is properly configured
4. ✅ Field names match database schema exactly
5. ✅ Data types in Value<T>() calls are correct
6. ✅ Error handling is implemented for null values

### Query Methods Reference:
- `Equal` - Exact match
- `Contains` - Text contains value
- `GreaterThan` - Numeric/date greater than
- `LessThan` - Numeric/date less than
- `GreaterThanOrEqual` - Numeric/date greater than or equal
- `LessThanOrEqual` - Numeric/date less than or equal

---

## Best Practices

### Database Design:
- Use clear, descriptive table and field names
- Include appropriate data types and constraints
- Document schema thoroughly
- Plan for future scalability

### Query Optimization:
- Use specific field selection when possible
- Implement pagination for large datasets
- Add appropriate sorting
- Cache frequently accessed data

### Error Handling:
- Always check for null values
- Handle database connection errors
- Provide fallback content
- Log errors appropriately

---

## Related Workflows

- **[Creating Modules](./src/instructions/workflow-2-creating-modules.md)** - Data-driven modules
- **[Creating Relationships](./src/instructions/workflow-5-creating-relationships.md)** - Linking data between tables
- **[Template Patterns](../patterns/)** - Template examples using database data
