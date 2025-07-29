# Mixcore CMS Template MCP Tools - Usage Examples for LLMs

This document shows how LLMs (like Claude) can use the MCP tools to create and manage Mixcore CMS templates.

## Example Workflow: Creating a Blog Template Theme

### 1. First, discover available themes and folder types

```json
{
  "tool": "GetAvailableThemes",
  "arguments": {}
}
```

Response:
```json
[
  {
    "Id": 1,
    "SystemName": "default-theme",
    "Title": "Default Theme",
    "CreatedDateTime": "2024-01-01T00:00:00Z"
  }
]
```

```json
{
  "tool": "GetTemplateFolderTypes",
  "arguments": {}
}
```

Response:
```json
[
  {
    "Value": 0,
    "Name": "Layouts",
    "Description": "Layout templates that define the overall structure of pages"
  },
  {
    "Value": 5,
    "Name": "Posts", 
    "Description": "Post templates for blog/news content"
  }
]
```

### 2. Create a main layout template

```json
{
  "tool": "CreateTemplate",
  "arguments": {
    "fileName": "blog-layout",
    "extension": "cshtml",
    "folderType": 0,
    "themeId": 1,
    "content": "<!DOCTYPE html>\n<html>\n<head>\n    <meta charset=\"utf-8\" />\n    <title>@ViewBag.Title - My Blog</title>\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    @RenderSection(\"Styles\", false)\n</head>\n<body>\n    <header>\n        <nav class=\"navbar\">\n            <div class=\"container\">\n                <a href=\"/\" class=\"logo\">My Blog</a>\n                <ul class=\"nav-links\">\n                    <li><a href=\"/\">Home</a></li>\n                    <li><a href=\"/posts\">Posts</a></li>\n                    <li><a href=\"/about\">About</a></li>\n                </ul>\n            </div>\n        </nav>\n    </header>\n    <main class=\"container\">\n        @RenderBody()\n    </main>\n    <footer>\n        <div class=\"container\">\n            <p>&copy; 2024 My Blog. All rights reserved.</p>\n        </div>\n    </footer>\n    @RenderSection(\"Scripts\", false)\n</body>\n</html>",
    "styles": "<style>\n.navbar {\n    background: #333;\n    padding: 1rem 0;\n}\n.navbar .container {\n    display: flex;\n    justify-content: space-between;\n    align-items: center;\n    max-width: 1200px;\n    margin: 0 auto;\n    padding: 0 2rem;\n}\n.logo {\n    color: white;\n    font-size: 1.5rem;\n    font-weight: bold;\n    text-decoration: none;\n}\n.nav-links {\n    display: flex;\n    list-style: none;\n    margin: 0;\n    padding: 0;\n    gap: 2rem;\n}\n.nav-links a {\n    color: white;\n    text-decoration: none;\n}\n.container {\n    max-width: 1200px;\n    margin: 0 auto;\n    padding: 0 2rem;\n}\nmain {\n    min-height: 70vh;\n    padding: 2rem 0;\n}\nfooter {\n    background: #f8f9fa;\n    padding: 2rem 0;\n    margin-top: 4rem;\n}\n</style>",
    "scripts": "<script>\n// Add any layout-specific JavaScript here\nconsole.log('Blog layout loaded');\n</script>"
  }
}
```

### 3. Create a blog post template

```json
{
  "tool": "CreateTemplate",
  "arguments": {
    "fileName": "blog-post",
    "extension": "cshtml",
    "folderType": 5,
    "themeId": 1,
    "content": "@{\n    Layout = \"~/Views/Shared/blog-layout.cshtml\";\n    ViewBag.Title = Model.Title;\n}\n\n<article class=\"blog-post\">\n    <header class=\"post-header\">\n        <h1 class=\"post-title\">@Model.Title</h1>\n        <div class=\"post-meta\">\n            <span class=\"post-date\">@Model.CreatedDateTime.ToString(\"MMMM dd, yyyy\")</span>\n            @if (!string.IsNullOrEmpty(Model.CreatedBy))\n            {\n                <span class=\"post-author\">by @Model.CreatedBy</span>\n            }\n        </div>\n    </header>\n    \n    @if (!string.IsNullOrEmpty(Model.Image))\n    {\n        <div class=\"post-image\">\n            <img src=\"@Model.Image\" alt=\"@Model.Title\" />\n        </div>\n    }\n    \n    <div class=\"post-content\">\n        @Html.Raw(Model.Content)\n    </div>\n    \n    <footer class=\"post-footer\">\n        <div class=\"post-tags\">\n            @if (Model.Tags != null)\n            {\n                foreach (var tag in Model.Tags)\n                {\n                    <span class=\"tag\">@tag.Name</span>\n                }\n            }\n        </div>\n        \n        <div class=\"post-navigation\">\n            @if (ViewBag.PreviousPost != null)\n            {\n                <a href=\"@ViewBag.PreviousPost.Url\" class=\"nav-previous\">&larr; @ViewBag.PreviousPost.Title</a>\n            }\n            @if (ViewBag.NextPost != null)\n            {\n                <a href=\"@ViewBag.NextPost.Url\" class=\"nav-next\">@ViewBag.NextPost.Title &rarr;</a>\n            }\n        </div>\n    </footer>\n</article>",
    "styles": "<style>\n.blog-post {\n    max-width: 800px;\n    margin: 0 auto;\n}\n.post-header {\n    margin-bottom: 2rem;\n    text-align: center;\n}\n.post-title {\n    font-size: 2.5rem;\n    margin-bottom: 1rem;\n    color: #333;\n}\n.post-meta {\n    color: #666;\n    font-size: 0.9rem;\n}\n.post-meta .post-author {\n    margin-left: 1rem;\n}\n.post-image {\n    margin: 2rem 0;\n    text-align: center;\n}\n.post-image img {\n    max-width: 100%;\n    height: auto;\n    border-radius: 8px;\n}\n.post-content {\n    line-height: 1.6;\n    font-size: 1.1rem;\n    margin-bottom: 3rem;\n}\n.post-footer {\n    border-top: 1px solid #eee;\n    padding-top: 2rem;\n}\n.post-tags {\n    margin-bottom: 2rem;\n}\n.tag {\n    display: inline-block;\n    background: #e9ecef;\n    color: #495057;\n    padding: 0.25rem 0.75rem;\n    border-radius: 20px;\n    font-size: 0.85rem;\n    margin-right: 0.5rem;\n}\n.post-navigation {\n    display: flex;\n    justify-content: space-between;\n}\n.nav-previous,\n.nav-next {\n    color: #007bff;\n    text-decoration: none;\n    font-weight: 500;\n}\n.nav-previous:hover,\n.nav-next:hover {\n    text-decoration: underline;\n}\n</style>"
  }
}
```

### 4. Create a blog post list template

```json
{
  "tool": "CreateTemplate", 
  "arguments": {
    "fileName": "blog-list",
    "extension": "cshtml",
    "folderType": 5,
    "themeId": 1,
    "content": "@{\n    Layout = \"~/Views/Shared/blog-layout.cshtml\";\n    ViewBag.Title = \"Blog Posts\";\n}\n\n<div class=\"blog-list\">\n    <header class=\"page-header\">\n        <h1>Latest Blog Posts</h1>\n        <p>Discover our latest thoughts and insights</p>\n    </header>\n    \n    <div class=\"posts-grid\">\n        @foreach (var post in Model.Items)\n        {\n            <article class=\"post-card\">\n                @if (!string.IsNullOrEmpty(post.Image))\n                {\n                    <div class=\"card-image\">\n                        <img src=\"@post.Image\" alt=\"@post.Title\" />\n                    </div>\n                }\n                \n                <div class=\"card-content\">\n                    <h2 class=\"card-title\">\n                        <a href=\"@post.Url\">@post.Title</a>\n                    </h2>\n                    \n                    <div class=\"card-meta\">\n                        <span class=\"post-date\">@post.CreatedDateTime.ToString(\"MMM dd, yyyy\")</span>\n                    </div>\n                    \n                    <div class=\"card-excerpt\">\n                        @Html.Raw(post.Excerpt ?? post.Content.Substring(0, Math.Min(150, post.Content.Length)) + \"...\")\n                    </div>\n                    \n                    <a href=\"@post.Url\" class=\"read-more\">Read More</a>\n                </div>\n            </article>\n        }\n    </div>\n    \n    @if (Model.TotalPages > 1)\n    {\n        <nav class=\"pagination\">\n            @if (Model.PageIndex > 0)\n            {\n                <a href=\"?page=@(Model.PageIndex - 1)\" class=\"page-link\">&larr; Previous</a>\n            }\n            \n            @for (int i = 0; i < Model.TotalPages; i++)\n            {\n                <a href=\"?page=@i\" class=\"page-link @(i == Model.PageIndex ? \"active\" : \"\")\">@(i + 1)</a>\n            }\n            \n            @if (Model.PageIndex < Model.TotalPages - 1)\n            {\n                <a href=\"?page=@(Model.PageIndex + 1)\" class=\"page-link\">Next &rarr;</a>\n            }\n        </nav>\n    }\n</div>",
    "styles": "<style>\n.blog-list {\n    max-width: 1200px;\n    margin: 0 auto;\n}\n.page-header {\n    text-align: center;\n    margin-bottom: 3rem;\n}\n.page-header h1 {\n    font-size: 2.5rem;\n    margin-bottom: 1rem;\n}\n.posts-grid {\n    display: grid;\n    grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));\n    gap: 2rem;\n    margin-bottom: 3rem;\n}\n.post-card {\n    background: white;\n    border-radius: 12px;\n    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);\n    overflow: hidden;\n    transition: transform 0.2s;\n}\n.post-card:hover {\n    transform: translateY(-4px);\n}\n.card-image img {\n    width: 100%;\n    height: 200px;\n    object-fit: cover;\n}\n.card-content {\n    padding: 1.5rem;\n}\n.card-title {\n    margin-bottom: 1rem;\n}\n.card-title a {\n    color: #333;\n    text-decoration: none;\n    font-size: 1.25rem;\n    font-weight: 600;\n}\n.card-title a:hover {\n    color: #007bff;\n}\n.card-meta {\n    color: #666;\n    font-size: 0.9rem;\n    margin-bottom: 1rem;\n}\n.card-excerpt {\n    color: #555;\n    line-height: 1.5;\n    margin-bottom: 1.5rem;\n}\n.read-more {\n    color: #007bff;\n    text-decoration: none;\n    font-weight: 500;\n}\n.read-more:hover {\n    text-decoration: underline;\n}\n.pagination {\n    display: flex;\n    justify-content: center;\n    gap: 0.5rem;\n}\n.page-link {\n    padding: 0.5rem 1rem;\n    background: white;\n    border: 1px solid #dee2e6;\n    color: #007bff;\n    text-decoration: none;\n    border-radius: 4px;\n}\n.page-link:hover,\n.page-link.active {\n    background: #007bff;\n    color: white;\n}\n</style>"
  }
}
```

### 5. List all created templates to verify

```json
{
  "tool": "GetTemplates",
  "arguments": {
    "themeId": 1,
    "pageSize": 20
  }
}
```

### 6. Make updates to a template

```json
{
  "tool": "UpdateTemplate",
  "arguments": {
    "id": 1,
    "content": "<!DOCTYPE html>\n<html>\n<head>\n    <meta charset=\"utf-8\" />\n    <title>@ViewBag.Title - My Awesome Blog</title>\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap\" rel=\"stylesheet\">\n    @RenderSection(\"Styles\", false)\n</head>\n<body>\n    <!-- Updated content with better typography -->\n</body>\n</html>"
  }
}
```

### 7. Create a copy for a different theme variation

```json
{
  "tool": "CopyTemplate",
  "arguments": {
    "id": 1
  }
}
```

## Advanced Usage

### Search templates by content

```json
{
  "tool": "FilterTemplates",
  "arguments": {
    "searchCriteria": "{\"keyword\":\"blog\",\"pageSize\":50}"
  }
}
```

### Get a specific template's full details

```json
{
  "tool": "GetTemplateById",
  "arguments": {
    "id": 1
  }
}
```

This shows how LLMs can programmatically create complete theme templates for Mixcore CMS, making it easy to generate sophisticated website templates through natural language instructions.