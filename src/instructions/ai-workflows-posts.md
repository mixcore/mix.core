# AI Workflows: Creating Blog Posts and Articles

This guide covers creating blog posts, news articles, and other post-based content in Mix CMS.

---

## How to Create Blog Posts

For blog entries or news articles:

### Step 1: Create a Post Template

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 5` (Posts)
    -   `fileName`: "BlogPost"
    -   `extension`: ".cshtml"
    -   `mixThemeId: 1`
    -   `content`: Start with `@model Mixcore.Domain.ViewModels.PostContentViewModel` and add your post layout HTML

### Template Model for Posts

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

### Step 2: Create the Post Content

-   **Tool:** `CreatePostContent`
-   **Parameters:**
    -   `title`: "Getting Started with Our Platform"
    -   `content`: The HTML content for the post body (e.g., `"<p>This is the main content of our post...</p>"`)
    -   `excerpt`: Brief HTML summary for the post (e.g., `"<p>A short summary of this post...</p>"`)
    -   `seoName`: "getting-started-platform" (this becomes the URL)
    -   `tenantId: 1`

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

## Managing Post Content

### Creating Multiple Posts

Use `CreatePostContent` for each post:

```csharp
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

Use `UpdatePostContent` to modify existing posts:

```csharp
UpdatePostContent(
    id: {post_id},
    title: "Updated Post Title",
    content: "<p>Updated content...</p>",
    excerpt: "<p>Updated excerpt...</p>",
    status: 1  // 0=Preview, 1=Published, 2=Draft
)
```

### Listing Posts

Use `ListPostContents` to see all your posts:

```csharp
ListPostContents(
    pageIndex: 0,
    pageSize: 10,
    status: 1,  // Only published posts
    keyword: "search term"  // Optional search
)
```

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
**After successfully completing blog/post setup, document it in your project's `project-progress.md` file:**

```markdown
## Completed Tasks
### 2025-01-XX - Blog System Setup
- **Post Template:** Created BlogPost.cshtml (folderType: 5)
- **Initial Posts:** Created 3 sample blog posts with proper SEO names
- **Content Status:** All posts set to Published (status: 1)
- **Templates:** Blog listing and individual post pages working
- **Status:** ✅ Complete - Blog system fully functional
- **Notes:** Uses PostContentViewModel, includes meta information and excerpts

### 2025-01-XX - News Section Addition  
- **Post Template:** Created NewsArticle.cshtml for news content
- **Content:** Added 5 news articles with recent dates
- **Integration:** Added news section to homepage
- **Status:** ✅ Complete - News section displaying correctly
```

This documentation helps team members understand what blog functionality is available and how it's configured.

---

## Next Steps

Once you've mastered blog posts:
- **[Template Patterns & Best Practices](./ai-template-patterns.md)** - Advanced template techniques
- **[Working with Dynamic Data](./ai-workflows-dynamic-data.md)** - Add dynamic elements to posts
- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete command reference
