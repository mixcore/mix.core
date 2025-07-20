# Mix CMS MCP Tools Reference

Complete reference for all Model Context Protocol (MCP) tools available in Mix CMS.

## Template Management Tools

### CreateTemplate
Creates new templates for layouts, pages, or modules.

**Parameters:**
- `fileName` (string): Template file name with .cshtml extension
- `content` (string): HTML/Razor template content
- `folderType` (int): Template type
  - `7` = Master Layouts (site-wide structure)
  - `1` = Page Templates (page-specific layouts) 
  - `2` = Modules (reusable components)
- `mixThemeId` (int): Theme ID (typically `1`)
- `tenantId` (int): Tenant ID (default: `1`)

**Returns:** Template object with `id` field

### ListTemplates
Lists existing templates with optional filtering.

**Parameters:**
- `folderType` (int, optional): Filter by template type
- `keyword` (string, optional): Search keyword
- `pageIndex` (int): Page number (0-based)
- `pageSize` (int): Results per page

### UpdateTemplate / DeleteTemplate
Modify or remove existing templates using template `id`.

## Content Management Tools

### CreatePageContent
Creates new web pages.

**Parameters:**
- `title` (string): Page title
- `content` (string): Page content body
- `seoName` (string): URL-friendly name (becomes the page URL)
- `templateId` (int): Page template ID
- `layoutId` (int): Master layout ID
- `tenantId` (int): Tenant ID (default: `1`)

**Returns:** Page object with `id` field

### CreatePostContent
Creates blog posts or news articles.

**Parameters:**
- `title` (string): Post title
- `content` (string): Post content body
- `seoName` (string): URL-friendly name
- `tenantId` (int): Tenant ID (default: `1`)

### CreateModuleContent
Creates reusable content modules.

**Parameters:**
- `title` (string): Module title
- `systemName` (string): System identifier
- `tenantId` (int): Tenant ID (default: `1`)

### List/Update/Delete Operations
All content types support:
- `List[ContentType]Contents`: Get paginated lists with filtering
- `Update[ContentType]Content`: Modify existing content
- `Delete[ContentType]Content`: Remove content
- `Get[ContentType]Content`: Retrieve single item by ID
- `Get[ContentType]ContentBySeoName`: Retrieve by URL name

## Database Management Tools

### CreateDatabaseFromPrompt
Creates database tables using natural language descriptions.

**Parameters:**
- `displayName` (string): Human-readable table name
- `schemaDescription` (string): Natural language schema description
- `mixDatabaseContextId` (int): Database context (default: `1`)
- `llmServiceType` (LLMServiceType): AI service for parsing (default: DeepSeek)
- `llmModel` (string): AI model name (default: "deepseek-chat")

**Example:**
```
CreateDatabaseFromPrompt(
  displayName: "Menu Items",
  schemaDescription: "A table for restaurant menu items with name (text), description (text), price (decimal), category (text), and is_featured (boolean)"
)
```

### CreateMixDbData / CreateManyMixDbData
Add single or multiple records to database tables.

**Parameters:**
- `databaseSystemName` (string): Table system name (e.g., "mix_menu_items")
- `dataJson` (string): JSON data for records
- `createdBy` (string, optional): Creator username

### GetListMidxDbData
Retrieve records with filtering and sorting.

**Parameters:**
- `databaseSystemName` (string): Table system name
- `queryJson` (string): Query conditions in JSON format
- `sortJson` (string, optional): Sort conditions
- `selectColumns` (string, optional): Specific columns to return
- `loadNestedData` (bool): Include related data

**Query Format:**
```json
[{
  "FieldName": "category", 
  "Value": "appetizer", 
  "CompareOperator": 0
}]
```

**Compare Operators:**
- `0` = Equal
- `1` = Like (contains)
- `2` = ILike (case-insensitive contains)
- `3` = NotEqual
- `9` = GreaterThan
- `11` = LessThan

### GetPagingMixDbData
Get paginated results from database tables.

**Parameters:**
- `databaseSystemName` (string): Table system name
- `page` (int): Page number (1-based)
- `pageSize` (int): Records per page
- `queryJson` (string, optional): Filter conditions
- `sortJson` (string, optional): Sort conditions

## Schema Modification Tools

### AddColumnToDatabase
Add new columns to existing tables using natural language.

**Parameters:**
- `databaseSystemName` (string): Target table
- `schemaText` (string): Column description
- `llmServiceType` (LLMServiceType): AI service for parsing
- `llmModel` (string): AI model name

### UpdateDatabaseColumn
Modify existing table columns.

### DeleteDatabaseColumn
Remove columns from tables.
**Requires:** `confirmDropColumn: "YES"` (case sensitive)

## Response Format

All MCP tools return JSON responses with:
```json
{
  "Success": true/false,
  "Data": { "id": 123, /* object data */ },
  "Message": "Operation description"
}
```

**Important:** Always extract the `id` from successful responses - you'll need these IDs to link templates, pages, and content together.

## Common Workflows

### 1. Website Creation
1. `CreateTemplate` (folderType: 7) → Master Layout
2. `CreateTemplate` (folderType: 1) → Page Template  
3. `CreatePageContent` → Actual Page

### 2. Dynamic Content
1. `CreateDatabaseFromPrompt` → Data Table
2. `CreateManyMixDbData` → Populate Data
3. Use template code to display data

### 3. Module Development
1. `CreateTemplate` (folderType: 2) → Module Template
2. `CreateModuleContent` → Module Instance
3. Reference in page templates with `@await Html.PartialAsync("Modules/ModuleName")`
