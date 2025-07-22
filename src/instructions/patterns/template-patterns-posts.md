# Post Templates (folderType: 5)

Post templates define how blog posts, articles, and news content are displayed. They handle content formatting, metadata display, and reader engagement features.

---

## Overview

Post templates (`folderType: 5`) are specialized for displaying blog posts, articles, news, and other time-based content. They provide rich formatting and metadata display capabilities.

### Key Characteristics
- **Purpose:** Blog posts, articles, and news content
- **Model:** `@model Mixcore.Domain.ViewModels.PostContentViewModel`
- **Usage:** Referenced by posts via `templateId`
- **Features:** Metadata display, categories, tags, author information

---

## Creating Post Templates

### MCP Command
```csharp
CreateTemplate(
    folderType: 5,
    fileName: "BlogPost",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "@model Mixcore.Domain.ViewModels.PostContentViewModel\n<article><h1>@Model.Title</h1>@Html.Raw(Model.Content)</article>"
)
```

### Basic Post Template Structure
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="blog-post">
    <header class="post-header">
        <h1 class="post-title">@Model.Title</h1>
        <div class="post-meta">
            <span class="date">@Model.CreatedDateTime.ToString("MMMM dd, yyyy")</span>
            <span class="author">By @Model.CreatedBy</span>
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
</article>
```

---

## Post Template Examples

### Standard Blog Post Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="blog-post">
    <!-- Post Header -->
    <header class="post-header">
        <h1 class="post-title">@Model.Title</h1>
        
        <div class="post-meta">
            <div class="meta-item">
                <i class="fas fa-calendar"></i>
                <span>@Model.CreatedDateTime.ToString("MMMM dd, yyyy")</span>
            </div>
            <div class="meta-item">
                <i class="fas fa-user"></i>
                <span>@Model.CreatedBy</span>
            </div>
            @if (Model.LastModified.HasValue)
            {
                <div class="meta-item">
                    <i class="fas fa-edit"></i>
                    <span>Updated @Model.LastModified.Value.ToString("MMM dd, yyyy")</span>
                </div>
            }
        </div>
        
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <div class="post-excerpt">
                <p>@Html.Raw(Model.Excerpt)</p>
            </div>
        }
    </header>
    
    <!-- Featured Image -->
    @if (!string.IsNullOrEmpty(Model.FeaturedImage))
    {
        <div class="featured-image">
            <img src="@Model.FeaturedImage" alt="@Model.Title" class="img-fluid">
        </div>
    }
    
    <!-- Post Content -->
    <div class="post-content">
        @Html.Raw(Model.Content)
    </div>
    
    <!-- Post Footer -->
    <footer class="post-footer">
        <div class="tags">
            @if (!string.IsNullOrEmpty(Model.Tags))
            {
                <div class="post-tags">
                    <strong>Tags:</strong>
                    @foreach (var tag in Model.Tags.Split(','))
                    {
                        <span class="tag">@tag.Trim()</span>
                    }
                </div>
            }
        </div>
        
        <div class="share-buttons">
            <strong>Share:</strong>
            <a href="https://twitter.com/intent/tweet?text=@Uri.EscapeDataString(Model.Title)&url=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
               target="_blank" class="btn btn-sm btn-outline-primary">
                <i class="fab fa-twitter"></i> Twitter
            </a>
            <a href="https://www.facebook.com/sharer/sharer.php?u=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
               target="_blank" class="btn btn-sm btn-outline-primary">
                <i class="fab fa-facebook"></i> Facebook
            </a>
            <a href="https://www.linkedin.com/sharing/share-offsite/?url=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
               target="_blank" class="btn btn-sm btn-outline-primary">
                <i class="fab fa-linkedin"></i> LinkedIn
            </a>
        </div>
    </footer>
</article>

@section Styles {
    <style>
        .blog-post { max-width: 800px; margin: 0 auto; }
        .post-header { margin-bottom: 2rem; text-align: center; }
        .post-title { font-size: 2.5rem; margin-bottom: 1rem; }
        .post-meta { 
            display: flex; 
            justify-content: center; 
            gap: 20px; 
            margin-bottom: 1rem;
            color: #666;
        }
        .meta-item { display: flex; align-items: center; gap: 5px; }
        .post-excerpt { 
            font-size: 1.2rem; 
            color: #666; 
            margin-bottom: 1rem;
            font-style: italic;
        }
        .featured-image { margin-bottom: 2rem; }
        .post-content { 
            line-height: 1.8; 
            font-size: 1.1rem;
            margin-bottom: 2rem;
        }
        .post-content h2 { margin-top: 2rem; margin-bottom: 1rem; }
        .post-content h3 { margin-top: 1.5rem; margin-bottom: 0.75rem; }
        .post-footer { 
            border-top: 1px solid #eee; 
            padding-top: 1rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .tag { 
            background: #f8f9fa; 
            padding: 3px 8px; 
            border-radius: 4px; 
            margin-right: 5px;
            font-size: 0.9rem;
        }
        .share-buttons .btn { margin-left: 10px; }
    </style>
}
```

### News Article Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="news-article">
    <!-- Article Header -->
    <header class="article-header">
        @if (!string.IsNullOrEmpty(Model.Category))
        {
            <div class="category-badge">
                <span class="badge bg-primary">@Model.Category</span>
            </div>
        }
        
        <h1 class="article-title">@Model.Title</h1>
        
        <div class="article-meta">
            <div class="meta-left">
                <span class="date">@Model.CreatedDateTime.ToString("dddd, MMMM dd, yyyy")</span>
                <span class="time">@Model.CreatedDateTime.ToString("h:mm tt")</span>
            </div>
            <div class="meta-right">
                <span class="author">By @Model.CreatedBy</span>
                @if (!string.IsNullOrEmpty(Model.Source))
                {
                    <span class="source">Source: @Model.Source</span>
                }
            </div>
        </div>
        
        @if (!string.IsNullOrEmpty(Model.Excerpt))
        {
            <div class="article-lead">
                @Html.Raw(Model.Excerpt)
            </div>
        }
    </header>
    
    <!-- Featured Image with Caption -->
    @if (!string.IsNullOrEmpty(Model.FeaturedImage))
    {
        <figure class="featured-image">
            <img src="@Model.FeaturedImage" alt="@Model.Title" class="img-fluid">
            @if (!string.IsNullOrEmpty(Model.ImageCaption))
            {
                <figcaption>@Model.ImageCaption</figcaption>
            }
        </figure>
    }
    
    <!-- Article Content -->
    <div class="article-content">
        @Html.Raw(Model.Content)
    </div>
    
    <!-- Article Footer -->
    <footer class="article-footer">
        <div class="last-updated">
            @if (Model.LastModified.HasValue)
            {
                <small class="text-muted">Last updated: @Model.LastModified.Value.ToString("MMM dd, yyyy h:mm tt")</small>
            }
        </div>
    </footer>
</article>

@section Styles {
    <style>
        .news-article { max-width: 900px; margin: 0 auto; }
        .category-badge { margin-bottom: 1rem; }
        .article-title { 
            font-size: 2.8rem; 
            font-weight: bold; 
            line-height: 1.2; 
            margin-bottom: 1rem;
        }
        .article-meta { 
            display: flex; 
            justify-content: space-between; 
            margin-bottom: 1.5rem;
            padding-bottom: 1rem;
            border-bottom: 1px solid #eee;
        }
        .meta-left, .meta-right { display: flex; flex-direction: column; }
        .article-lead { 
            font-size: 1.3rem; 
            font-weight: 500; 
            color: #444;
            margin-bottom: 2rem;
        }
        .featured-image { margin-bottom: 2rem; }
        .featured-image figcaption { 
            text-align: center; 
            font-style: italic; 
            color: #666; 
            margin-top: 0.5rem;
        }
        .article-content { 
            font-size: 1.1rem; 
            line-height: 1.8; 
            margin-bottom: 2rem;
        }
        .article-footer { 
            border-top: 1px solid #eee; 
            padding-top: 1rem;
            text-align: center;
        }
    </style>
}
```

### Blog Post with Related Posts
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel
@using Mix.Mixdb.Interfaces
@using Mix.Shared.Models
@using Mix.Shared.Dtos
@using Mix.Constant.Enums
@using Mix.Constant.Constants

@inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
@inject IMixDbDataServiceFactory mixDbDataServiceFactory

@{
    // Get related posts
    var mixDbDataService = mixDbDataServiceFactory.Create(dbSrv.DatabaseProvider, dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION));
    var relatedRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_post",
        Queries = new List<MixQueryField>
        {
            new MixQueryField 
            { 
                FieldName = "Category", 
                Value = Model.Category, 
                CompareOperator = MixCompareOperator.Equal 
            },
            new MixQueryField 
            { 
                FieldName = "Id", 
                Value = Model.Id.ToString(), 
                CompareOperator = MixCompareOperator.NotEqual 
            }
        }
    };
    var relatedPosts = await mixDbDataService.GetListByAsync(relatedRequest);
}

<div class="blog-post-container">
    <div class="row">
        <!-- Main Content -->
        <div class="col-lg-8">
            <article class="blog-post">
                <header class="post-header">
                    <h1 class="post-title">@Model.Title</h1>
                    <div class="post-meta">
                        <span class="date">@Model.CreatedDateTime.ToString("MMMM dd, yyyy")</span>
                        <span class="author">By @Model.CreatedBy</span>
                        <span class="reading-time">5 min read</span>
                    </div>
                </header>
                
                @if (!string.IsNullOrEmpty(Model.FeaturedImage))
                {
                    <div class="featured-image">
                        <img src="@Model.FeaturedImage" alt="@Model.Title" class="img-fluid">
                    </div>
                }
                
                <div class="post-content">
                    @Html.Raw(Model.Content)
                </div>
                
                <footer class="post-footer">
                    <div class="tags">
                        @if (!string.IsNullOrEmpty(Model.Tags))
                        {
                            <strong>Tags:</strong>
                            @foreach (var tag in Model.Tags.Split(','))
                            {
                                <span class="tag">@tag.Trim()</span>
                            }
                        }
                    </div>
                </footer>
            </article>
            
            <!-- Comments Section -->
            <section class="comments-section">
                <h3>Comments</h3>
                <div class="comment-form">
                    <form>
                        <div class="mb-3">
                            <label for="comment-name" class="form-label">Name</label>
                            <input type="text" class="form-control" id="comment-name">
                        </div>
                        <div class="mb-3">
                            <label for="comment-email" class="form-label">Email</label>
                            <input type="email" class="form-control" id="comment-email">
                        </div>
                        <div class="mb-3">
                            <label for="comment-text" class="form-label">Comment</label>
                            <textarea class="form-control" id="comment-text" rows="4"></textarea>
                        </div>
                        <button type="submit" class="btn btn-primary">Post Comment</button>
                    </form>
                </div>
            </section>
        </div>
        
        <!-- Sidebar -->
        <div class="col-lg-4">
            <aside class="blog-sidebar">
                <!-- Author Bio -->
                <div class="sidebar-widget author-bio">
                    <h4>About the Author</h4>
                    <div class="author-info">
                        <img src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=100" 
                             alt="@Model.CreatedBy" class="author-avatar">
                        <div class="author-details">
                            <h5>@Model.CreatedBy</h5>
                            <p>Writer and content creator passionate about technology and innovation.</p>
                        </div>
                    </div>
                </div>
                
                <!-- Related Posts -->
                @if (relatedPosts.Any())
                {
                    <div class="sidebar-widget related-posts">
                        <h4>Related Posts</h4>
                        @foreach (var relatedPost in relatedPosts.Take(3))
                        {
                            <div class="related-post">
                                <h6><a href="/post/@(relatedPost.Value<string>("SeoName"))">@(relatedPost.Value<string>("Title"))</a></h6>
                                <small>@(relatedPost.Value<DateTime>("CreatedDateTime").ToString("MMM dd, yyyy"))</small>
                            </div>
                        }
                    </div>
                }
                
                <!-- Categories -->
                <div class="sidebar-widget categories">
                    <h4>Categories</h4>
                    <ul class="category-list">
                        <li><a href="/category/technology">Technology</a></li>
                        <li><a href="/category/business">Business</a></li>
                        <li><a href="/category/design">Design</a></li>
                    </ul>
                </div>
            </aside>
        </div>
    </div>
</div>

@section Styles {
    <style>
        .blog-post-container { margin-top: 2rem; }
        .post-meta { display: flex; gap: 15px; color: #666; margin-bottom: 1.5rem; }
        .featured-image { margin-bottom: 2rem; }
        .post-content { margin-bottom: 2rem; line-height: 1.8; }
        .tag { background: #f8f9fa; padding: 3px 8px; border-radius: 4px; margin-right: 5px; }
        .comments-section { margin-top: 3rem; padding-top: 2rem; border-top: 1px solid #eee; }
        .blog-sidebar { padding-left: 2rem; }
        .sidebar-widget { margin-bottom: 2rem; padding: 1.5rem; background: #f8f9fa; border-radius: 8px; }
        .author-info { display: flex; align-items: center; gap: 15px; }
        .author-avatar { width: 60px; height: 60px; border-radius: 50%; }
        .related-post { margin-bottom: 1rem; }
        .related-post h6 { margin-bottom: 0.25rem; }
        .category-list { list-style: none; padding: 0; }
        .category-list li { margin-bottom: 0.5rem; }
    </style>
}
```

### Product Review Template
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<article class="product-review">
    <!-- Review Header -->
    <header class="review-header">
        <div class="product-info">
            <h1 class="product-name">@Model.Title</h1>
            <div class="review-meta">
                <div class="rating">
                    @{
                        var rating = ViewBag.Rating ?? 4.5;
                        var fullStars = (int)rating;
                        var hasHalfStar = rating % 1 >= 0.5;
                    }
                    @for (int i = 1; i <= 5; i++)
                    {
                        if (i <= fullStars)
                        {
                            <i class="fas fa-star text-warning"></i>
                        }
                        else if (i == fullStars + 1 && hasHalfStar)
                        {
                            <i class="fas fa-star-half-alt text-warning"></i>
                        }
                        else
                        {
                            <i class="far fa-star text-warning"></i>
                        }
                    }
                    <span class="rating-score">@rating/5</span>
                </div>
                <div class="review-date">Reviewed on @Model.CreatedDateTime.ToString("MMMM dd, yyyy")</div>
                <div class="reviewer">By @Model.CreatedBy</div>
            </div>
        </div>
        
        @if (!string.IsNullOrEmpty(Model.FeaturedImage))
        {
            <div class="product-image">
                <img src="@Model.FeaturedImage" alt="@Model.Title" class="img-fluid">
            </div>
        }
    </header>
    
    <!-- Quick Summary -->
    @if (!string.IsNullOrEmpty(Model.Excerpt))
    {
        <div class="review-summary">
            <h3>Summary</h3>
            @Html.Raw(Model.Excerpt)
        </div>
    }
    
    <!-- Pros and Cons -->
    <div class="pros-cons">
        <div class="row">
            <div class="col-md-6">
                <div class="pros">
                    <h4><i class="fas fa-thumbs-up text-success"></i> Pros</h4>
                    <ul>
                        <li>Excellent build quality</li>
                        <li>Great value for money</li>
                        <li>Easy to use</li>
                        <li>Responsive customer support</li>
                    </ul>
                </div>
            </div>
            <div class="col-md-6">
                <div class="cons">
                    <h4><i class="fas fa-thumbs-down text-danger"></i> Cons</h4>
                    <ul>
                        <li>Limited color options</li>
                        <li>Could be more portable</li>
                        <li>Setup instructions unclear</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Detailed Review -->
    <div class="review-content">
        <h3>Detailed Review</h3>
        @Html.Raw(Model.Content)
    </div>
    
    <!-- Review Footer -->
    <footer class="review-footer">
        <div class="recommendation">
            <h4>Would I recommend this product?</h4>
            <p class="recommendation-text">
                <strong>Yes!</strong> This product offers excellent value and performs well in most scenarios. 
                It's particularly good for beginners and professionals alike.
            </p>
        </div>
        
        <div class="affiliate-notice">
            <small class="text-muted">
                <i class="fas fa-info-circle"></i>
                This review may contain affiliate links. We may earn a commission if you purchase through these links.
            </small>
        </div>
    </footer>
</article>

@section Styles {
    <style>
        .product-review { max-width: 900px; margin: 0 auto; }
        .review-header { margin-bottom: 2rem; }
        .product-name { font-size: 2.5rem; margin-bottom: 1rem; }
        .review-meta { display: flex; gap: 20px; align-items: center; margin-bottom: 1rem; }
        .rating { display: flex; align-items: center; gap: 10px; }
        .rating-score { font-weight: bold; }
        .product-image { margin-top: 1rem; }
        .review-summary { 
            background: #f8f9fa; 
            padding: 1.5rem; 
            border-radius: 8px; 
            margin-bottom: 2rem;
        }
        .pros-cons { margin-bottom: 2rem; }
        .pros, .cons { 
            background: #fff; 
            padding: 1.5rem; 
            border-radius: 8px; 
            border: 1px solid #eee;
        }
        .pros h4 { color: #28a745; }
        .cons h4 { color: #dc3545; }
        .review-content { margin-bottom: 2rem; line-height: 1.8; }
        .recommendation { 
            background: #e7f3ff; 
            padding: 1.5rem; 
            border-radius: 8px; 
            margin-bottom: 1rem;
        }
        .affiliate-notice { text-align: center; }
    </style>
}
```

---

## Creating Post Content

### MCP Command Workflow
1. **Create the Post Template:**
```csharp
CreateTemplate(
    folderType: 5,
    fileName: "BlogPost",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<!-- post template content -->"
)
```

2. **Create the Post Content:**
```csharp
CreatePostContent(
    title: "10 Tips for Better Web Design",
    content: "<h2>Introduction</h2><p>Web design is crucial...</p>",
    excerpt: "Discover essential tips to improve your web design skills and create better user experiences.",
    seoName: "10-tips-better-web-design"
)
```

---

## Advanced Post Features

### Post Categories and Tags
```razor
@model Mixcore.Domain.ViewModels.PostContentViewModel

<!-- Category Display -->
@if (!string.IsNullOrEmpty(Model.Category))
{
    <div class="post-categories">
        <span class="category-label">Category:</span>
        <a href="/category/@Model.Category.ToLower()" class="category-link">@Model.Category</a>
    </div>
}

<!-- Tags Display -->
@if (!string.IsNullOrEmpty(Model.Tags))
{
    <div class="post-tags">
        <span class="tags-label">Tags:</span>
        @foreach (var tag in Model.Tags.Split(',').Select(t => t.Trim()))
        {
            <a href="/tag/@tag.ToLower()" class="tag-link">#@tag</a>
        }
    </div>
}
```

### Reading Time Estimation
```razor
@{
    var wordCount = Model.Content?.Split(' ').Length ?? 0;
    var readingTime = Math.Max(1, wordCount / 200); // Average reading speed
}

<div class="reading-time">
    <i class="fas fa-clock"></i>
    <span>@readingTime min read</span>
</div>
```

### Social Sharing
```razor
<div class="social-share">
    <h5>Share this post:</h5>
    <div class="share-buttons">
        <a href="https://twitter.com/intent/tweet?text=@Uri.EscapeDataString(Model.Title)&url=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
           class="btn btn-twitter">
            <i class="fab fa-twitter"></i> Twitter
        </a>
        <a href="https://www.facebook.com/sharer/sharer.php?u=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
           class="btn btn-facebook">
            <i class="fab fa-facebook"></i> Facebook
        </a>
        <a href="https://www.linkedin.com/sharing/share-offsite/?url=@Uri.EscapeDataString(ViewBag.CurrentUrl)" 
           class="btn btn-linkedin">
            <i class="fab fa-linkedin"></i> LinkedIn
        </a>
    </div>
</div>
```

---

## Best Practices

### Content Structure
- **Clear hierarchy:** Use proper heading structure (h1, h2, h3)
- **Scannable content:** Break up text with subheadings and lists
- **Call-to-action:** Include engagement elements
- **Mobile-first:** Ensure readability on all devices

### SEO Optimization
- **Meta descriptions:** Use excerpt for meta descriptions
- **Structured data:** Include article schema markup
- **Internal linking:** Link to related content
- **Image optimization:** Use alt text and proper sizing

### User Experience
- **Fast loading:** Optimize images and content
- **Social sharing:** Make content easily shareable
- **Related content:** Show similar posts
- **Author information:** Build trust with author bios

---

## Troubleshooting

### Common Issues

**Post not displaying:**
- Verify `templateId` references correct post template (`folderType: 5`)
- Check post status (published vs draft)
- Ensure template content is valid Razor syntax

**Metadata not showing:**
- Check `@model Mixcore.Domain.ViewModels.PostContentViewModel`
- Verify property names: `Title`, `CreatedDateTime`, `CreatedBy`
- Test with minimal template first

**Related posts not loading:**
- Verify database table name and column names
- Check category/tag matching logic
- Test database query independently

**Styling issues:**
- Check CSS conflicts with main layout
- Test post template in isolation
- Verify responsive design on mobile

### Debugging Tips
- Start with simple post template
- Test each feature incrementally
- Use browser developer tools
- Check Mix CMS logs for errors

---

## Next Steps

After creating post templates:

1. **Create Post Content** - Use `CreatePostContent`
2. **Add Categories** - Organize posts by topics
3. **Implement Comments** - Add reader engagement
4. **SEO Optimization** - Add structured data

---

## Related Guides

- **[Page Templates](./template-patterns-pages.md)** - Creating static pages
- **[Module Templates](./template-patterns-modules.md)** - Reusable components
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
