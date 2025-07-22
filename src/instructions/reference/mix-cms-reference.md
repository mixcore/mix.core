# Mix CMS Enums and Constants Reference

This reference guide contains important enums and constants used throughout Mix CMS for template creation, content management, and development.

---

## Template Folder Types

The `folderType` parameter in template creation corresponds to the `MixTemplateFolderType` enum:

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

### Usage Examples

When creating templates using MCP commands:

```csharp
// Master Layout (most common)
CreateTemplate(content, fileName: "MasterLayout", extension: ".cshtml", folderType: 7, mixThemeId: 1)

// Page Template
CreateTemplate(content, fileName: "HomePage", extension: ".cshtml", folderType: 1, mixThemeId: 1)

// Module Template
CreateTemplate(content, fileName: "ServiceCard", extension: ".cshtml", folderType: 2, mixThemeId: 1)

// Post Template
CreateTemplate(content, fileName: "BlogPost", extension: ".cshtml", folderType: 5, mixThemeId: 1)
```

### Template Rendering Paths

The folder type determines the rendering path pattern:

```razor
// Modules (FolderType.Modules) - renders as "../Modules/ServiceCard.cshtml"
@await Html.PartialAsync($"../{template.FolderType.ToString()}/ServiceCard.cshtml", model)

// Pages (FolderType.Pages) - renders as "../Pages/HomePage.cshtml"
@await Html.PartialAsync($"../{template.FolderType.ToString()}/HomePage.cshtml", model)

// Posts (FolderType.Posts) - renders as "../Posts/BlogPost.cshtml"
@await Html.PartialAsync($"../{template.FolderType.ToString()}/BlogPost.cshtml", model)

// Master Layouts (FolderType.Masters) - renders as "../Masters/MasterLayout.cshtml"
@await Html.PartialAsync($"../{template.FolderType.ToString()}/MasterLayout.cshtml", model)
```

---

## Content Status Types

Content items (pages, posts, modules) have status values:

```csharp
// Content Status Values
public const int PREVIEW = 0;    // Visible for preview but not published
public const int PUBLISHED = 1;  // Live and visible to all visitors  
public const int DRAFT = 2;      // Work in progress, not visible publicly
```

### Usage in Content Creation

```csharp
// Create published content
CreatePageContent(title: "Home", content: "<h1>Welcome</h1>", status: 1, ...)

// Create draft content
CreatePostContent(title: "Draft Post", content: "<p>Work in progress...</p>", status: 2, ...)

// Update content status
UpdatePageContent(id: pageId, status: 1)  // Publish the page
```

---

## Query Comparison Operators

When querying MixDb data, use these comparison operators:

```csharp
// From Mix.Constant.Enums.MixCompareOperator
Equal               // Exact match
NotEqual           // Not equal to
GreaterThan        // Greater than
GreaterThanOrEqual // Greater than or equal
LessThan           // Less than  
LessThanOrEqual    // Less than or equal
Like               // Contains (SQL LIKE)
NotLike            // Does not contain
In                 // Value in list
NotIn              // Value not in list
```

### Query Examples

```csharp
// Exact match
new MixQueryField { FieldName = "category", Value = "electronics", CompareOperator = MixCompareOperator.Equal }

// Contains text
new MixQueryField { FieldName = "name", Value = "smartphone", CompareOperator = MixCompareOperator.Like }

// Numeric comparison
new MixQueryField { FieldName = "price", Value = 100, CompareOperator = MixCompareOperator.LessThan }

// Boolean values
new MixQueryField { FieldName = "is_featured", Value = true, CompareOperator = MixCompareOperator.Equal }
```

---

## Database Field Types

Common database field types when creating tables:

```csharp
// Text Fields
"text"           // Short text (nvarchar)
"long text"      // Long text (ntext)
"email"          // Email address
"url"            // Website URL
"phone"          // Phone number

// Numeric Fields  
"int"            // Integer
"decimal"        // Decimal number
"money"          // Currency amount
"float"          // Floating point

// Date/Time Fields
"datetime"       // Date and time
"date"           // Date only
"time"           // Time only

// Boolean Fields
"boolean"        // True/false
"checkbox"       // Checkbox (boolean)

// Special Fields
"json"           // JSON data
"file"           // File upload
"image"          // Image upload
```

### Database Schema Example

```csharp
CreateDatabaseFromPrompt(
    displayName: "Products",
    schemaDescription: "A table for products with name (text), description (long text), price (decimal), is_featured (boolean), created_date (datetime)"
)
```

---

## Theme and Tenant Constants

```csharp
// Default Values
public const int DEFAULT_THEME_ID = 1;    // Default theme ID
public const int DEFAULT_TENANT_ID = 1;   // Default tenant ID

// Connection Constants
public const string CONST_CMS_CONNECTION = "MixCmsConnection";
```

### Usage

```csharp
// Creating templates
CreateTemplate(..., mixThemeId: 1, tenantId: 1)

// Creating content
CreatePageContent(..., tenantId: 1)

// Database connections
dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)
```

---

## File Extensions

```csharp
// Template Extensions
".cshtml"        // Razor template (required)
".html"          // Static HTML
".css"           // Stylesheet
".js"            // JavaScript

// Content Extensions  
".json"          // JSON data
".xml"           // XML data
".txt"           // Plain text
```

---

## Best Practices

### Template Creation
- Always use `folderType: 7` for master layouts
- Use `folderType: 1` for page templates
- Use `folderType: 2` for reusable modules
- Use `folderType: 5` for blog posts

### Content Management
- Use `status: 2` (Draft) for work in progress
- Use `status: 1` (Published) for live content
- Use `status: 0` (Preview) for testing

### Database Queries
- Use `MixCompareOperator.Equal` for exact matches
- Use `MixCompareOperator.Like` for text searches
- Use appropriate operators for numeric comparisons
- Always specify field names exactly as they appear in the database

### Naming Conventions
- Use descriptive, URL-friendly SEO names
- Keep file names clear and consistent
- Use proper casing for enum values
- Follow Mix CMS naming patterns

---

## Related Documentation

- **[AI Getting Started](./ai-getting-started.md)** - Core concepts using these enums
- **[AI Workflows - Basic Pages](./ai-workflows-basic-pages.md)** - Template creation examples
- **[AI Workflows - Dynamic Data](./ai-workflows-dynamic-data.md)** - Database and query examples
- **[AI Template Patterns](./ai-template-patterns.md)** - Advanced usage patterns
- **[Developer Guide](./developer-guide.md)** - Technical implementation details
