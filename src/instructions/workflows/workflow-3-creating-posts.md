# Workflow 3: Creating Posts and Articles in Mix CMS

> **MixDb Data Value Rendering Instruction:**
> When rendering MixDb data values in templates, always use the following pattern:
>
> ```razor
> @(item.Value<[dataType]>("[column_system_name]"))
> ```
>
> - Replace `[dataType]` with the correct .NET type (e.g., `string`, `int`, `decimal`, `bool`, `DateTime`).
> - Replace `[column_system_name]` with the exact system name of the column.
> - The entire expression must be inside brackets `@(...)` when used standalone.
>
> **Example:**
> ```razor
> <span>@(item.Value<string>("name"))</span>
> <span>@(item.Value<decimal>("price"))</span>
> <span>@(item.Value<DateTime>("published_date"))</span>
> ```

---

## PostContentViewModel Reference

`PostContentViewModel` is the main ViewModel for blog posts and articles in Mix CMS. It extends `ExtraColumnMultilingualSEOContentViewModelBase` and provides:

- **Properties:**
  - `ClassName`: Custom class name for the post.
  - `DetailUrl`: SEO-friendly URL for the post (`/post/{Id}/{SeoName}`).
  - `AdditionalData`: Holds extra data fields (dynamic columns, JSON).
  - `PostMetadata`: List of metadata objects for the post.
- **Methods:**
  - `Property<T>(string fieldName)`: Get a strongly-typed value from `AdditionalData` by field name.
  - `LoadAdditionalDataAsync(...)`: Loads extra data and metadata for the post.

**Sample Usage:**

```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<h1>@(Model.Title)</h1>
<p>@(Model.DetailUrl)</p>
<p>@(Model.Property<string>("custom_field"))</p>
<p>@(Model.AdditionalData?["another_field"])</p>
```

> **Note:** Always use `@(...)` when rendering standalone values in Razor.

> **Rendering PostContentViewModel Data:**
> ```razor
> <h1>@(Model.Title)</h1>
> <span>@(Model.Property<string>("custom_field"))</span>
> <span>@(Model.AdditionalData?["another_field"])</span>
> ```

---

## Loading Related Posts

To load related posts (e.g., for a "Related Articles" section), use the `Mixcore.Domain.Services.MixcorePostService`.

**Sample Usage in Razor Page or Controller:**

```csharp
@using Mixcore.Domain.Services
@inject MixcorePostService postService

@{
    // Example: Get latest 3 published posts except the current one
    var relatedPosts = await postService.GetRelatedPosts(Model.Id);
}

<div class="related-posts">
    <h4>Related Posts</h4>
    <ul>
    @foreach (var post in relatedPosts)
    {
        <li>
            <a href="@(post.DetailUrl)">@(post.Title)</a>
        </li>
    }
    </ul>
</div>
```

> **Note:** Inject `Mixcore.Domain.Services.MixcorePostService` in your Razor view or controller to access post loading methods. Adjust the query as needed for your scenario.
# Workflow 3: Creating Blog Posts in Mix CMS

This workflow covers creating blog posts and articles, including post templates and content management.

---

## Prerequisites

Before creating blog posts, ensure you understand:
- Post template requirements (folderType: 5)
- Post status management
- SEO and URL structure for posts

---

## Step 1: Create Post Template (if needed)

```markdown
CreateTemplate(
    folderType: 5,
    fileName: "BlogPost",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PostContentViewModel\n<article class=\"blog-post\">\n    <header class=\"post-header\">\n        <h1>@Model.Title</h1>\n        <div class=\"post-meta\">\n            <time>@Model.CreatedDateTime.ToString(\"MMMM dd, yyyy\")</time>\n        </div>\n    </header>\n    <div class=\"post-content\">\n        @Html.Raw(Model.Content)\n    </div>\n</article>"
)
```

**Template Requirements:**
- Must use `folderType: 5` for blog posts
- Should include `@model Mixcore.Domain.ViewModels.PostContentViewModel`
- Include post metadata (date, author, etc.)

---

## Step 2: Create Post Content

```markdown
CreatePostContent(
    title: "Getting Started Guide",
    content: "<p>This post explains how to get started...</p>",
    excerpt: "<p>A comprehensive guide to getting started...</p>",
    seoName: "getting-started-guide",
    templateId: {post_template_id},    // MUST be template with folderType: 5 (Posts)
    tenantId: 1
)
```

**CRITICAL**: The `templateId` must reference a template with `folderType: 5` (Posts). Use `ListTemplates()` to verify template folderType before creating post content.

---

## Step 3: Manage Post Status

Post status values:
- **0 = Preview**: For drafts and previews
- **1 = Published**: Live and public
- **2 = Draft**: Work in progress

```markdown
UpdatePostContent(
    id: {post_id},
    status: 1  // Publish the post
)
```

---

## Post Template Examples

### Basic Blog Post Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="blog-post">
    <header class="post-header">
        <h1>@Model.Title</h1>
        
        <div class="post-meta">
            <time class="post-date" datetime="@Model.CreatedDateTime.ToString("yyyy-MM-dd")">
                @Model.CreatedDateTime.ToString("MMMM dd, yyyy")
            </time>
            @if (!string.IsNullOrEmpty(Model.CreatedBy))
            {
                <span class="post-author">by @Model.CreatedBy</span>
            }
        </div>
        
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <div class="post-excerpt">
                @Html.Raw(Model.Excerpt)
            </div>
        }
    </header>
    
    <div class="post-content">
        @Html.Raw(Model.Content)
    </div>
    
    <footer class="post-footer">
        <div class="post-tags">
            <!-- Add tags if available -->
        </div>
        
        <div class="post-share">
            <p>Share this post:</p>
            <a href="https://twitter.com/intent/tweet?text=@Model.Title&url=@Request.Url" target="_blank">Twitter</a>
            <a href="https://www.facebook.com/sharer/sharer.php?u=@Request.Url" target="_blank">Facebook</a>
        </div>
    </footer>
</article>

<style>
    .blog-post {
        max-width: 800px;
        margin: 0 auto;
        padding: 20px;
    }
    .post-header {
        margin-bottom: 30px;
        padding-bottom: 20px;
        border-bottom: 1px solid #eee;
    }
    .post-meta {
        color: #666;
        font-size: 0.9em;
        margin-bottom: 15px;
    }
    .post-excerpt {
        font-style: italic;
        color: #555;
        font-size: 1.1em;
        line-height: 1.6;
        margin-top: 15px;
    }
    .post-content {
        line-height: 1.8;
        font-size: 1.1em;
    }
    .post-footer {
        margin-top: 40px;
        padding-top: 20px;
        border-top: 1px solid #eee;
    }
    .post-share a {
        margin-right: 15px;
        color: #007bff;
        text-decoration: none;
    }
</style>
```

### Featured Image Blog Post Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="blog-post with-featured-image">
    @{
        // Extract featured image from content or use default
        var featuredImage = "https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d";
    }
    
    <div class="post-featured-image">
        <img src="@featuredImage" alt="@Model.Title" class="img-fluid">
    </div>
    
    <div class="post-body">
        <header class="post-header">
            <div class="post-category">
                <span class="badge badge-primary">Technology</span>
            </div>
            
            <h1>@Model.Title</h1>
            
            <div class="post-meta">
                <div class="author-info">
                    <img src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face" alt="Author" class="author-avatar">
                    <div class="author-details">
                        <span class="author-name">@(Model.CreatedBy ?? "Admin")</span>
                        <time class="post-date">@Model.CreatedDateTime.ToString("MMM dd, yyyy")</time>
                    </div>
                </div>
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
            <div class="row">
                <div class="col-md-6">
                    <div class="post-navigation">
                        <!-- Previous/Next post navigation -->
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="post-share text-right">
                        <h6>Share this article</h6>
                        <a href="#" class="btn btn-outline-primary btn-sm">
                            <i class="fab fa-twitter"></i> Twitter
                        </a>
                        <a href="#" class="btn btn-outline-primary btn-sm">
                            <i class="fab fa-facebook"></i> Facebook
                        </a>
                        <a href="#" class="btn btn-outline-primary btn-sm">
                            <i class="fab fa-linkedin"></i> LinkedIn
                        </a>
                    </div>
                </div>
            </div>
        </footer>
    </div>
</article>

<style>
    .with-featured-image {
        margin-bottom: 50px;
    }
    .post-featured-image {
        margin-bottom: 30px;
    }
    .post-featured-image img {
        width: 100%;
        height: 400px;
        object-fit: cover;
        border-radius: 8px;
    }
    .post-body {
        max-width: 800px;
        margin: 0 auto;
        padding: 0 20px;
    }
    .post-category {
        margin-bottom: 15px;
    }
    .author-info {
        display: flex;
        align-items: center;
        margin-bottom: 20px;
    }
    .author-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 15px;
    }
    .author-name {
        font-weight: bold;
        display: block;
    }
    .post-date {
        color: #666;
        font-size: 0.9em;
    }
    .post-excerpt {
        font-size: 1.2em;
        line-height: 1.6;
        color: #555;
        font-style: italic;
        margin-bottom: 30px;
    }
</style>
```

### News Article Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="news-article">
    <header class="article-header">
        <div class="article-meta-top">
            <span class="article-category">Breaking News</span>
            <time class="article-date">@Model.CreatedDateTime.ToString("MMMM dd, yyyy HH:mm")</time>
        </div>
        
        <h1 class="article-headline">@Model.Title</h1>
        
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <p class="article-lead">@Html.Raw(Model.Excerpt)</p>
        }
        
        <div class="article-meta-bottom">
            <span class="article-author">By @(Model.CreatedBy ?? "Staff Reporter")</span>
            <span class="reading-time">5 min read</span>
        </div>
    </header>
    
    <div class="article-body">
        @Html.Raw(Model.Content)
    </div>
    
    <aside class="article-sidebar">
        <div class="related-articles">
            <h4>Related Articles</h4>
            <!-- Related articles would go here -->
        </div>
        
        <div class="newsletter-signup">
            <h4>Stay Updated</h4>
            <p>Get the latest news delivered to your inbox.</p>
            <form>
                <input type="email" placeholder="Your email" class="form-control mb-2">
                <button type="submit" class="btn btn-primary btn-block">Subscribe</button>
            </form>
        </div>
    </aside>
</article>

<style>
    .news-article {
        display: grid;
        grid-template-columns: 2fr 1fr;
        gap: 40px;
        max-width: 1200px;
        margin: 0 auto;
        padding: 20px;
    }
    .article-header {
        grid-column: 1 / -1;
        margin-bottom: 30px;
    }
    .article-meta-top {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 15px;
    }
    .article-category {
        background: #dc3545;
        color: white;
        padding: 4px 12px;
        border-radius: 4px;
        font-size: 0.8em;
        font-weight: bold;
        text-transform: uppercase;
    }
    .article-headline {
        font-size: 2.5em;
        line-height: 1.2;
        margin-bottom: 15px;
    }
    .article-lead {
        font-size: 1.3em;
        line-height: 1.5;
        color: #555;
        margin-bottom: 20px;
    }
    .article-meta-bottom {
        display: flex;
        gap: 20px;
        color: #666;
        font-size: 0.9em;
    }
    .article-body {
        line-height: 1.8;
        font-size: 1.1em;
    }
    .article-sidebar {
        background: #f8f9fa;
        padding: 20px;
        border-radius: 8px;
        height: fit-content;
    }
    @media (max-width: 768px) {
        .news-article {
            grid-template-columns: 1fr;
        }
    }
</style>
```

---

## Post Listing Templates

### Blog Index Template
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
        TableName = "mix_post",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "Status", Value = "1", Method = "Equal" }
        },
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "CreatedDateTime", Direction = "Desc" } 
        }
    };
    var posts = await mixDbDataService.GetListByAsync(request);
}

<div class="blog-index">
    <header class="blog-header">
        <h1>Latest Blog Posts</h1>
        <p>Stay updated with our latest insights and news</p>
    </header>
    
    <div class="posts-grid">
        @foreach (var post in posts.Take(9))
        {
            <article class="post-card">
                <div class="post-card-image">
                    <img src="https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d" 
                         alt="@(post.Value<string>("Title"))" 
                         class="img-fluid">
                </div>
                
                <div class="post-card-content">
                    <h3>
                        <a href="/post/@(post.Value<string>("SeoName"))">
                            @(post.Value<string>("Title"))
                        </a>
                    </h3>
                    
                    <p class="post-excerpt">
                        @(post.Value<string>("Excerpt"))
                    </p>
                    
                    <div class="post-meta">
                        <time>@(post.Value<DateTime>("CreatedDateTime").ToString("MMM dd, yyyy"))</time>
                        <a href="/post/@(post.Value<string>("SeoName"))" class="read-more">Read More</a>
                    </div>
                </div>
            </article>
        }
    </div>
</div>

<style>
    .blog-header {
        text-align: center;
        margin-bottom: 50px;
    }
    .posts-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 30px;
        margin-bottom: 50px;
    }
    .post-card {
        border: 1px solid #e9ecef;
        border-radius: 8px;
        overflow: hidden;
        transition: box-shadow 0.3s ease;
    }
    .post-card:hover {
        box-shadow: 0 4px 15px rgba(0,0,0,0.1);
    }
    .post-card-image img {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }
    .post-card-content {
        padding: 20px;
    }
    .post-card-content h3 {
        margin-bottom: 10px;
    }
    .post-card-content h3 a {
        color: #333;
        text-decoration: none;
    }
    .post-excerpt {
        color: #666;
        margin-bottom: 15px;
        line-height: 1.5;
    }
    .post-meta {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 0.9em;
    }
    .post-meta time {
        color: #888;
    }
    .read-more {
        color: #007bff;
        text-decoration: none;
        font-weight: 500;
    }
</style>
```

---

## Post Management Patterns

### Create Multiple Posts
```markdown
// Create several blog posts
CreatePostContent(
    title: "Introduction to Web Development",
    content: "<p>Web development is...</p>",
    excerpt: "<p>Learn the basics of web development</p>",
    seoName: "intro-web-development",
    templateId: {post_template_id}
)

CreatePostContent(
    title: "Advanced CSS Techniques",
    content: "<p>Advanced CSS techniques include...</p>",
    excerpt: "<p>Master advanced CSS for better designs</p>",
    seoName: "advanced-css-techniques",
    templateId: {post_template_id}
)
```

### Update Post Status
```markdown
// Publish a draft post
UpdatePostContent(
    id: {post_id},
    status: 1,  // Published
    title: "Updated Post Title"
)
```

---

## SEO Best Practices

### SEO-Optimized Post Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

@{
    ViewBag.Title = Model.Title;
    ViewBag.Description = Model.Excerpt ?? Model.Title;
    ViewBag.Keywords = "blog, article, " + Model.Title.ToLower().Replace(" ", ", ");
}

@section Seo {
    <meta name="description" content="@ViewBag.Description">
    <meta name="keywords" content="@ViewBag.Keywords">
    <meta property="og:title" content="@Model.Title">
    <meta property="og:description" content="@ViewBag.Description">
    <meta property="og:type" content="article">
    <meta property="og:url" content="@Request.Url">
    <meta property="article:published_time" content="@Model.CreatedDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")">
}

@section Schema {
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "BlogPosting",
        "headline": "@Model.Title",
        "description": "@ViewBag.Description",
        "datePublished": "@Model.CreatedDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")",
        "author": {
            "@type": "Person",
            "name": "@(Model.CreatedBy ?? "Admin")"
        }
    }
    </script>
}

<!-- Rest of template content -->
```

---

## Troubleshooting

### Common Issues:
- **Wrong template type**: Verify templateId has folderType: 5 (Posts)
- **Post not visible**: Check status (0=Preview, 1=Published, 2=Draft)
- **SEO issues**: Ensure unique seoName and proper meta tags
- **Template not found**: Verify template exists and file name is correct

### Verification Checklist:
1. ✅ Post template exists with folderType: 5
2. ✅ Post content created with correct templateId
3. ✅ seoName is unique and URL-friendly
4. ✅ Post status is set correctly (1 for published)
5. ✅ SEO meta tags are included
6. ✅ Content is properly formatted HTML

---

## Related Workflows

- **[Creating Pages](./workflow-1-creating-pages.md)** - Creating static pages
- **[Working with Database Data](./workflow-4-database-data.md)** - Dynamic post listings
- **[Post Patterns](../patterns/template-patterns-posts.md)** - Additional post template examples
