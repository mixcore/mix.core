# Mix CMS Enums and Constants Reference

This reference guide contains important enums and constants used throughout Mix CMS for template creation, content management, and development.

---


## Template Folder Types (MixTemplateFolderType)

The `folderType` parameter is required when creating or updating templates. It determines the template's category and rendering path. Use the correct value from the `MixTemplateFolderType` enum:

| FolderType Value | Enum Name  | Description                        | Typical Usage Example           |
|------------------|------------|------------------------------------|---------------------------------|
| 0                | Layouts    | Layout templates (base layouts)    | Shared layout for all pages     |
| 1                | Pages      | Page templates                     | Home, About, Contact            |
| 2                | Modules    | Module templates (reusable blocks) | ServiceCard, FeatureList        |
| 3                | Forms      | Form templates                     | ContactForm, SignupForm         |
| 4                | Edms       | Document management templates      | DocumentList, FileViewer        |
| 5                | Posts      | Post/blog templates                | BlogPost, NewsArticle           |
| 6                | Widgets    | Widget templates                   | SidebarWidget, FooterWidget     |
| 7                | Masters    | Master layout templates            | MasterLayout (main site shell)  |

**Enum Definition:**
```csharp
public enum MixTemplateFolderType
{
    Layouts = 0,    // Shared layout templates (base for all pages)
    Pages = 1,      // Page templates (full pages)
    Modules = 2,    // Module templates (reusable content blocks)
    Forms = 3,      // Form templates (user input forms)
    Edms = 4,       // Document management templates
    Posts = 5,      // Post/blog templates (articles, news)
    Widgets = 6,    // Widget templates (small UI components)
    Masters = 7,    // Master layout templates (site-wide shell)
}
```

### How to Use Folder Types

When creating templates with MCP commands, always set the correct `folderType`:

```csharp
// Master Layout (site shell)
CreateTemplate(content, fileName: "MasterLayout", extension: ".cshtml", folderType: 7, mixThemeId: 1)

// Page Template (full page)
CreateTemplate(content, fileName: "HomePage", extension: ".cshtml", folderType: 1, mixThemeId: 1)

// Module Template (reusable block)
CreateTemplate(content, fileName: "ServiceCard", extension: ".cshtml", folderType: 2, mixThemeId: 1)

// Post Template (blog/news)
CreateTemplate(content, fileName: "BlogPost", extension: ".cshtml", folderType: 5, mixThemeId: 1)
```

### Template Rendering Path Patterns

The folder type controls where the template is rendered from:

```razor
// Module (folderType: 2)
@await Html.PartialAsync("../Modules/ServiceCard.cshtml", model)

// Page (folderType: 1)
@await Html.PartialAsync("../Pages/HomePage.cshtml", model)

// Post (folderType: 5)
@await Html.PartialAsync("../Posts/BlogPost.cshtml", model)

// Master Layout (folderType: 7)
@await Html.PartialAsync("../Masters/MasterLayout.cshtml", model)
```

> **Tip:** Always match the folder type to the template's intended use. This ensures correct rendering and organization.

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
