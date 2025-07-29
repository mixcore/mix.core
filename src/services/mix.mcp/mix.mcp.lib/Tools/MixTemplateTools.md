# Mixcore CMS Template MCP Tools

This implementation provides MCP (Model Context Protocol) tools for interacting with Mixcore CMS templates, allowing LLMs like Claude to create and manage theme templates.

## Available Tools

### Core Template Operations

1. **GetTemplates** - List all templates with filtering
   - Optional theme ID filter
   - Optional folder type filter (0-7)
   - Keyword search in filename and content
   - Pagination support
   
2. **GetTemplateById** - Get a single template by ID
   - Returns complete template data including content, scripts, styles
   - Expands view with file system content

3. **CreateTemplate** - Create a new template
   - Requires filename, extension, folder type, theme ID
   - Optional content, scripts, styles
   - Validates uniqueness and theme existence

4. **UpdateTemplate** - Update existing template
   - Partial updates supported
   - Can update filename, extension, content, scripts, styles

5. **DeleteTemplate** - Delete a template by ID
   - Safe deletion with existence validation

6. **CopyTemplate** - Copy existing template
   - Creates duplicate with "Copy_" prefix
   - Returns new template ID and filename

### Helper Operations

7. **GetDefaultTemplate** - Get default template structure
   - Provides template skeleton for different folder types
   - Includes default scripts and styles blocks

8. **FilterTemplates** - Advanced filtering with JSON criteria
   - Flexible search with multiple criteria
   - Supports complex filtering scenarios

9. **GetAvailableThemes** - List all themes
   - Shows available themes for template creation
   - Includes theme metadata

10. **GetTemplateFolderTypes** - Get folder type information
    - Lists all available folder types with descriptions
    - Helps understand template organization

## Folder Types

The following folder types are supported:

- **0 - Layouts**: Layout templates that define the overall structure of pages
- **1 - Pages**: Page templates for specific pages  
- **2 - Modules**: Module templates for specific functionality
- **3 - Forms**: Form templates for user input
- **4 - Edms**: Electronic Document Management System templates
- **5 - Posts**: Post templates for blog/news content
- **6 - Widgets**: Widget templates for reusable components
- **7 - Masters**: Master templates for hierarchical structures

## Usage Examples

### Creating a New Layout Template

```json
{
  "tool": "CreateTemplate",
  "arguments": {
    "fileName": "main-layout",
    "extension": "cshtml",
    "folderType": 0,
    "themeId": 1,
    "content": "<!DOCTYPE html>\n<html>\n<head>\n    <title>@ViewBag.Title</title>\n</head>\n<body>\n    @RenderBody()\n</body>\n</html>",
    "scripts": "<script>\n// Layout scripts\n</script>",
    "styles": "<style>\n/* Layout styles */\n</style>"
  }
}
```

### Searching Templates

```json
{
  "tool": "GetTemplates", 
  "arguments": {
    "themeId": 1,
    "folderType": 0,
    "keyword": "layout",
    "pageIndex": 0,
    "pageSize": 10
  }
}
```

### Advanced Filtering

```json
{
  "tool": "FilterTemplates",
  "arguments": {
    "searchCriteria": "{\"keyword\":\"blog\",\"themeId\":1,\"folderType\":5,\"pageIndex\":0,\"pageSize\":20}"
  }
}
```

## Integration

The tools are automatically registered in the MCP server startup and are available at the `/mcp` endpoint. They use the existing Mixcore CMS infrastructure and maintain consistency with the web API endpoints.

## API Mapping

These MCP tools map to the following Mixcore REST API endpoints:

- `GetTemplates` → `GET /api/v2/rest/mix-portal/mix-template`
- `GetTemplateById` → `GET /api/v2/rest/mix-portal/mix-template/{id}`
- `CreateTemplate` → `POST /api/v2/rest/mix-portal/mix-template`
- `UpdateTemplate` → `PUT /api/v2/rest/mix-portal/mix-template/{id}`
- `DeleteTemplate` → `DELETE /api/v2/rest/mix-portal/mix-template/{id}`
- `CopyTemplate` → `GET /api/v2/rest/mix-portal/mix-template/copy/{id}`
- `GetDefaultTemplate` → `GET /api/v2/rest/mix-portal/mix-template/default`
- `FilterTemplates` → `POST /api/v2/rest/mix-portal/mix-template/filter`

This ensures LLMs can perform all the same operations available through the web interface while maintaining data consistency and security.