# AI Workflows: Creating Blog Posts and Articles

This guide provides step-by-step workflows for creating blog posts, news articles, and other post-based content using Mix CMS MCP Tools for CRUD operations.

---

## How to Create Blog Posts

Follow these steps to create a fully functional blog post using MCP Tools.

### Step 1: Check Existing Templates
Use `ListTemplates` to see all available templates and avoid creating duplicates. You'll need to identify:
- **Master Layout Template**: A template with `folderType` = "Masters" (for layoutId)
- **Post Template**: A template with `folderType` = "Posts" (for templateId)

### Step 2: Create Templates (if needed)
If you don't have the required templates, create them first:
- **Master Layout**: Use `CreateTemplate` with `folderType: 7` (Masters)
- **Post Template**: Use `CreateTemplate` with `folderType: 5` (Posts)

### Step 3: Create the Post Content

Finally, create the post itself using MCP Tools and link it to the templates you identified/created.

-   **MCP Tool:** `CreatePostContent`
-   **Parameters:**
    -   `title`: "Getting Started with Our Platform"
    -   `content`: The HTML content for the post body (e.g., `"<p>This is the main content of our post...</p>"`)
    -   `excerpt`: Brief HTML summary for the post (e.g., `"<p>A short summary of this post...</p>"`)
    -   `seoName`: "getting-started-platform" (this becomes the URL)
    -   `tenantId`: 1

---

## CRUD Operations for Post Content

Use these MCP Tools for managing post content:

### Create Post Content
-   **MCP Tool:** `CreatePostContent`
-   **Purpose:** Create new blog posts and articles

### Read Post Content
-   **MCP Tool:** `GetPostContent` (by ID)
-   **MCP Tool:** `ListPostContents` (list multiple posts with filtering)
-   **Purpose:** Retrieve existing post data

### Update Post Content
-   **MCP Tool:** `UpdatePostContent`
-   **Purpose:** Modify existing posts
-   **Required:** `id` parameter (from create/list operations)

### Delete Post Content
-   **MCP Tool:** `DeletePostContent`
-   **Purpose:** Remove posts
-   **Required:** `id` parameter and `confirmDelete: "YES"`

---

## Template Naming Best Practices

-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"BlogPost"`, not `"BlogPost.cshtml"`)
    - The system will automatically combine them to create the full filename

-   **Template Identification:**
    - **layoutId**: Must be the ID of a template with `folderType` = "Masters" (folderType: 7)
    - **templateId**: Must be the ID of a template with `folderType` = "Posts" (folderType: 5)
    - Use `ListTemplates` MCP Tool to find existing templates and their folderType values

-   **Check for Existing Templates:** Use `ListTemplates` MCP Tool to avoid creating duplicates.

---

## Post Template Model Structure

When creating post templates, start with this model structure:

```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="post-content">
    <h1>@Model.Title</h1>
    <div class="post-meta">
        <span class="date">@Model.CreatedDateTime.ToString("MMMM dd, yyyy")</span>
        <span class="author">By @Model.CreatedBy</span>
    </div>
    <div class="post-excerpt">@Html.Raw(Model.Excerpt)</div>
    <div class="post-body">@Html.Raw(Model.Content)</div>
</article>
```

---

## Managing Post-Category Relationships

When posts need to be organized with categories or tags, use MCP Tools for CRUD relationship operations to properly link and manage the connections between posts and categories.

### Create Relationships
-   **MCP Tool:** `CreateMixDbRelationshipFromPrompt`
-   **Purpose:** Create relationships between posts and categories
-   **Parameters:**
    -   `sourceTableName`: "Post" (the post content)
    -   `destinateTableName`: "Category" (the category content)
    -   `displayName`: "Post Categories" (relationship display name)
    -   `propertyName`: "categories" (property name for loading related data)
    -   `relationshipType`: 0 (one-to-many relationship)

### Managing Categorized Content
When working with posts that have multiple categories:

1. **Create the post first** using `CreatePostContent`
2. **Create individual categories** using database operations
3. **Establish relationships** using `CreateMixDbRelationshipFromPrompt`
4. **Load related data** by setting `loadNestedData: true` in read operations

### Example Workflow for Categorized Posts
```markdown
1. Create blog post with `CreatePostContent`
2. Create categories (Technology, News, Tutorials)
3. Link posts to categories using relationship tools
4. Verify categorized structure with `GetPostContent` (loadNestedData: true)
```

---

## Post Content Structure

### Content vs Excerpt

- **`content`**: The full HTML content for the post body that will be rendered in the template
- **`excerpt`**: Brief HTML summary for the post, often used in post listings or previews

### SEO Name Guidelines

The `seoName` parameter becomes the URL slug:
- Use lowercase letters
- Replace spaces with hyphens
- Keep it descriptive but concise
- Avoid special characters

**Examples:**
- "Getting Started Guide" → `seoName: "getting-started-guide"`
- "New Product Launch 2025" → `seoName: "new-product-launch-2025"`
- "How to Use Our API" → `seoName: "how-to-use-our-api"`

---

## Partial Rendering Patterns

When rendering partial templates, always follow the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

For posts, this would be: `"../Posts/BlogPost.cshtml"`

---

## Rendering Posts in Templates

### Using Post Templates

When you create post content, it uses the post template you specify. The template receives a `PostContentViewModel` model with these key properties:

- `Title`: Post title
- `Content`: Main post content (HTML)
- `Excerpt`: Post summary (HTML)
- `SeoName`: URL-friendly name
- `CreatedDateTime`: When the post was created
- `CreatedBy`: Who created the post
- `Status`: Publication status (0=Preview, 1=Published, 2=Draft)

### Post Template Example

```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="blog-post">
    <header class="post-header">
        <h1 class="post-title">@Model.Title</h1>
        <div class="post-meta">
            <time datetime="@Model.CreatedDateTime.ToString("yyyy-MM-dd")">
                @Model.CreatedDateTime.ToString("MMMM dd, yyyy")
            </time>
            @if (!string.IsNullOrEmpty(Model.CreatedBy))
            {
                <span class="author">by @Model.CreatedBy</span>
            }
        </div>
    </header>
    
    @if (!string.IsNullOrEmpty(Model.Excerpt))
    {
        <div class="post-excerpt">
            @Html.Raw(Model.Excerpt)
        </div>
    }
    
    <div class="post-content">
        @Html.Raw(Model.Content)
    </div>
    
    <footer class="post-footer">
        <div class="post-tags">
            <!-- Add tags if needed -->
        </div>
        <div class="post-sharing">
            <!-- Add sharing buttons if needed -->
        </div>
    </footer>
</article>
```

---

## Managing Post Content with MCP Tools

### Creating Multiple Posts

Use `CreatePostContent` MCP Tool for each post:

```markdown
// First post
CreatePostContent(
    title: "Welcome to Our Blog",
    content: "<p>We're excited to share our thoughts and updates with you...</p>",
    excerpt: "<p>Welcome to our new blog where we'll share updates and insights.</p>",
    seoName: "welcome-to-our-blog",
    tenantId: 1
)

// Second post
CreatePostContent(
    title: "Product Update: New Features",
    content: "<p>We've added several new features to improve your experience...</p>",
    excerpt: "<p>Discover the latest features we've added to our platform.</p>",
    seoName: "product-update-new-features",
    tenantId: 1
)
```

### Updating Posts

Use `UpdatePostContent` MCP Tool to modify existing posts:

```markdown
UpdatePostContent(
    id: {post_id},
    title: "Updated Post Title",
    content: "<p>Updated content...</p>",
    excerpt: "<p>Updated excerpt...</p>",
    status: 1  // 0=Preview, 1=Published, 2=Draft
)
```

### Listing Posts

Use `ListPostContents` MCP Tool to see all your posts:

```markdown
ListPostContents(
    pageIndex: 0,
    pageSize: 10,
    status: 1,  // Only published posts
    keyword: "search term"  // Optional search
)
```

---

## Common Issues and Solutions

### Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` MCP Tool to check first.
-   **Post not displaying:** Check that the status is set to 1 (Published) and verify template linking.
-   **Template linking issues:** Ensure `templateId` points to a template with `folderType` = "Posts" and `layoutId` points to a template with `folderType` = "Masters".
-   **Category relationships not working:** For posts with categories, ensure you've created proper relationships using `CreateMixDbRelationshipFromPrompt` and are loading data with `loadNestedData: true`.
-   **Content not rendering:** Check that your post template includes proper HTML structure and uses `@Html.Raw()` for content.
-   **SEO name conflicts:** Use unique SEO names for each post.

### MCP Response Format
MCP Tools return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and posts together.

### Workflow Example
1. Run `ListTemplates` to see available templates
2. Identify Master Layout (folderType = "Masters") for `layoutId`
3. Identify Post Template (folderType = "Posts") for `templateId`
4. Use `CreatePostContent` with the correct configuration
5. If post needs categories, create relationships with `CreateMixDbRelationshipFromPrompt`
6. Verify with `GetPostContent` or `ListPostContents` (use `loadNestedData: true` for categorized posts)

---

## Post Status Management

Posts have three status levels:
- **0 = Preview**: Visible for preview but not publicly published
- **1 = Published**: Live and visible to all visitors
- **2 = Draft**: Work in progress, not visible publicly

Set the appropriate status when creating or updating posts.

---

## Best Practices for Posts

### Content Structure
- Use semantic HTML in your content
- Include proper headings (h2, h3) for structure
- Add alt text for images
- Keep excerpts concise but informative

### SEO Considerations
- Choose descriptive, keyword-rich titles
- Write compelling excerpts for search results
- Use meaningful SEO names that reflect the content
- Include relevant meta information

### Content Management
- Use drafts for work-in-progress content
- Preview posts before publishing
- Update publication status as needed
- Organize content with consistent naming conventions

---

## Troubleshooting Posts

### Common Issues

- **Post not displaying**: Check that the status is set to 1 (Published)
- **Missing template**: Ensure you've created a post template with `folderType: 5`
- **Broken layout**: Verify your post template includes proper HTML structure
- **SEO name conflicts**: Use unique SEO names for each post

### Template Issues

- **Missing model**: Ensure your post template starts with `@model Mixcore.Domain.ViewModels.PostContentViewModel`
- **Content not rendering**: Use `@Html.Raw(Model.Content)` to render HTML content
- **Styling problems**: Include proper CSS classes and structure in your template

### Task Documentation (CRITICAL)
**After successfully completing any post creation task, document it in your project's `project-progress.md` file:**

```markdown
## Completed Tasks
### 2025-01-XX - Blog System Setup
- **Master Layout:** Using existing MasterLayout.cshtml (folderType: 7)
- **Post Template:** Created BlogPost.cshtml (folderType: 5)
- **Initial Posts:** Created 3 sample blog posts with proper SEO names
- **Content Status:** All posts set to Published (status: 1)
- **Category Relationships:** Linked posts to Technology and News categories
- **Status:** ✅ Complete - Blog system fully functional
- **Notes:** Uses PostContentViewModel, includes meta information and excerpts

### 2025-01-XX - News Section Addition  
- **Post Template:** Created NewsArticle.cshtml for news content
- **Content:** Added 5 news articles with recent dates
- **Integration:** Added news section to homepage
- **Categories:** Created and linked News, Updates, Announcements categories
- **Status:** ✅ Complete - News section displaying correctly with categorization
```

This documentation helps team members understand what blog functionality is available and how it's configured.

---

## Next Steps

Once you've mastered blog posts:
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
- **[Working with Dynamic Data](./ai-workflows-mixdb-data.md)** - Add dynamic elements to posts
- **[Page Workflows](./ai-workflows-basic-pages.md)** - Learn how to create and manage pages
- **[Module Workflows](./ai-workflows-basic-modules.md)** - Create reusable content modules
- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
