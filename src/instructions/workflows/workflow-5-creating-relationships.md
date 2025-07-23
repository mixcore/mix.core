# Workflow 5: Creating Database Relationships in Mix CMS

This workflow covers creating relationships between database tables and using them in templates.

---

## Prerequisites

Before creating relationships, ensure you understand:
- Database table creation in Mix CMS
- Primary and foreign key concepts
- Relationship types (One-to-Many, Many-to-Many)
- Template data loading patterns

---

## Step 1: Plan Your Relationships

### Common Relationship Patterns

**Blog System Example:**
- `mix_categories` (One) → `mix_posts` (Many)
- `mix_authors` (One) → `mix_posts` (Many)
- `mix_posts` (Many) ← → `mix_tags` (Many) [Many-to-Many]

**E-commerce Example:**
- `mix_categories` (One) → `mix_products` (Many)
- `mix_customers` (One) → `mix_orders` (Many)
- `mix_orders` (One) → `mix_order_items` (Many)

**Healthcare Example:**
- `mix_doctors` (One) → `mix_appointments` (Many)
- `mix_departments` (One) → `mix_doctors` (Many)
- `mix_patients` (One) → `mix_appointments` (Many)

---

## Step 2: Create Parent Tables First

### Create Categories Table
```markdown
CreateDatabaseFromPrompt(
    displayName: "Categories",
    schemaDescription: "A table for categories with name (text), description (text), color (text), icon (text), slug (text), order_index (integer)"
)
```

### Create Authors Table
```markdown
CreateDatabaseFromPrompt(
    displayName: "Authors",
    schemaDescription: "A table for authors with name (text), bio (text), email (text), avatar (text), website (text), social_links (text)"
)
```

---

## Step 3: Create Child Tables with Foreign Keys

### Create Posts Table with Relationships
```markdown
CreateDatabaseFromPrompt(
    displayName: "Posts",
    schemaDescription: "A table for blog posts with title (text), content (text), excerpt (text), featured_image (text), category_id (integer), author_id (integer), published_date (date), views (integer), featured (boolean)"
)
```

---

## Step 4: Define Relationships

### One-to-Many: Category to Posts
```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_categories",
    destinateTableName: "mix_posts", 
    sourceColumnName: "objId",
    destinateColumnName: "category_id",
    displayName: "Posts",
    propertyName: "Posts",
    relationshipType: 0
)
```

### One-to-Many: Author to Posts
```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_authors",
    destinateTableName: "mix_posts",
    sourceColumnName: "objId", 
    destinateColumnName: "author_id",
    displayName: "Author Posts",
    propertyName: "AuthorPosts",
    relationshipType: 0
)
```

### Many-to-One: Posts to Category
```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_posts",
    destinateTableName: "mix_categories",
    sourceColumnName: "category_id",
    destinateColumnName: "objId",
    displayName: "Category",
    propertyName: "Category",
    relationshipType: 0
)
```

---

## Step 5: Add Sample Data

### Add Categories
```markdown
CreateManyMixDbData(
    databaseSystemName: "mix_categories",
    dataJson: '[
        {"name":"Technology","description":"Latest tech news and tutorials","color":"#007bff","icon":"fas fa-laptop","slug":"technology","order_index":1},
        {"name":"Health","description":"Health and wellness articles","color":"#28a745","icon":"fas fa-heartbeat","slug":"health","order_index":2},
        {"name":"Business","description":"Business insights and tips","color":"#ffc107","icon":"fas fa-briefcase","slug":"business","order_index":3},
        {"name":"Travel","description":"Travel guides and experiences","color":"#17a2b8","icon":"fas fa-plane","slug":"travel","order_index":4}
    ]'
)
```

### Add Authors
```markdown
CreateManyMixDbData(
    databaseSystemName: "mix_authors",
    dataJson: '[
        {"name":"Dr. Sarah Johnson","bio":"Medical expert with 15 years experience","email":"sarah@example.com","avatar":"https://images.unsplash.com/photo-1559839734-2b71ea197ec2","website":"https://drsarah.com","social_links":"twitter:@drsarah,linkedin:sarah-johnson"},
        {"name":"Mark Tech","bio":"Technology enthusiast and software developer","email":"mark@example.com","avatar":"https://images.unsplash.com/photo-1472099645785-5658abf4ff4e","website":"https://marktech.dev","social_links":"twitter:@marktech,github:marktech"},
        {"name":"Lisa Travel","bio":"Professional travel blogger","email":"lisa@example.com","avatar":"https://images.unsplash.com/photo-1544005313-94ddf0286df2","website":"https://lisatravel.com","social_links":"instagram:@lisatravel,youtube:lisatravel"}
    ]'
)
```

### Add Posts with Relationships
```markdown
CreateManyMixDbData(
    databaseSystemName: "mix_posts",
    dataJson: '[
        {"title":"10 Health Benefits of Regular Exercise","content":"Regular exercise provides numerous health benefits...","excerpt":"Discover the amazing health benefits of staying active","featured_image":"https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b","category_id":2,"author_id":1,"published_date":"2024-01-15","views":1250,"featured":true},
        {"title":"Latest JavaScript Frameworks in 2024","content":"The JavaScript ecosystem continues to evolve...","excerpt":"Explore the newest JavaScript frameworks and their features","featured_image":"https://images.unsplash.com/photo-1627398242454-45a1465c2479","category_id":1,"author_id":2,"published_date":"2024-01-10","views":890,"featured":false},
        {"title":"Best Travel Destinations for 2024","content":"Planning your next adventure? Here are the top destinations...","excerpt":"Discover the most exciting travel destinations this year","featured_image":"https://images.unsplash.com/photo-1488646953014-85cb44e25828","category_id":4,"author_id":3,"published_date":"2024-01-05","views":2100,"featured":true}
    ]'
)
```

---

## Step 6: Create Templates with Relationships

### Blog Posts with Category and Author
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
    
    var postsRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_posts",
        LoadNestedData = true,
        Queries = new List<MixQueryField>(),
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "published_date", Direction = "Desc" } 
        }
    };
    var posts = await mixDbDataService.GetListByAsync(postsRequest);
}

<div class="blog-posts">
    <div class="container">
        <h1>Latest Blog Posts</h1>
        
        <div class="row">
            @foreach (var post in posts)
            {
                <div class="col-lg-6 mb-4">
                    <article class="post-card">
                        @if(!string.IsNullOrEmpty(post.Value<string>("featured_image")))
                        {
                            <div class="post-image">
                                <img src="@(post.Value<string>("featured_image"))" 
                                     alt="@(post.Value<string>("title"))" 
                                     class="img-fluid">
                                @if(post.Value<bool>("featured"))
                                {
                                    <span class="featured-badge">Featured</span>
                                }
                            </div>
                        }
                        
                        <div class="post-content">
                            <!-- Category Display -->
                            @if(post.RelatedData != null && post.RelatedData.ContainsKey("Category"))
                            {
                                var category = post.RelatedData["Category"].FirstOrDefault();
                                if(category != null)
                                {
                                    <div class="post-category">
                                        <i class="@(category.Value<string>("icon"))"></i>
                                        <span style="color: @(category.Value<string>("color"))">
                                            @(category.Value<string>("name"))
                                        </span>
                                    </div>
                                }
                            }
                            
                            <h2 class="post-title">
                                <a href="/post/@(post.Value<string>("title").Replace(" ", "-").ToLower())">
                                    @(post.Value<string>("title"))
                                </a>
                            </h2>
                            
                            <p class="post-excerpt">@(post.Value<string>("excerpt"))</p>
                            
                            <div class="post-meta">
                                <!-- Author Display -->
                                <div class="author-info">
                                    @{
                                        // Get author by ID
                                        var authorId = post.Value<int>("author_id");
                                        var authorRequest = new SearchMixDbRequestModel
                                        {
                                            TableName = "mix_authors",
                                            Queries = new List<MixQueryField>
                                            {
                                                new MixQueryField { FieldName = "objId", Value = authorId.ToString(), Method = "Equal" }
                                            }
                                        };
                                        var authors = await mixDbDataService.GetListByAsync(authorRequest);
                                        var author = authors.FirstOrDefault();
                                    }
                                    
                                    @if(author != null)
                                    {
                                        <div class="author">
                                            <img src="@(author.Value<string>("avatar"))" 
                                                 alt="@(author.Value<string>("name"))" 
                                                 class="author-avatar">
                                            <span class="author-name">@(author.Value<string>("name"))</span>
                                        </div>
                                    }
                                </div>
                                
                                <div class="post-date">
                                    @(DateTime.Parse(post.Value<string>("published_date")).ToString("MMM dd, yyyy"))
                                </div>
                                
                                <div class="post-views">
                                    <i class="fas fa-eye"></i> @(post.Value<int>("views")) views
                                </div>
                            </div>
                        </div>
                    </article>
                </div>
            }
        </div>
    </div>
</div>

<style>
    .blog-posts {
        padding: 40px 0;
    }
    .post-card {
        border: 1px solid #e9ecef;
        border-radius: 10px;
        overflow: hidden;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        height: 100%;
        background: white;
    }
    .post-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 10px 25px rgba(0,0,0,0.1);
    }
    .post-image {
        position: relative;
        height: 250px;
        overflow: hidden;
    }
    .post-image img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        transition: transform 0.3s ease;
    }
    .post-card:hover .post-image img {
        transform: scale(1.05);
    }
    .featured-badge {
        position: absolute;
        top: 15px;
        right: 15px;
        background: #ff6b6b;
        color: white;
        padding: 5px 10px;
        border-radius: 15px;
        font-size: 0.75em;
        font-weight: bold;
    }
    .post-content {
        padding: 25px;
    }
    .post-category {
        margin-bottom: 10px;
    }
    .post-category i {
        margin-right: 5px;
    }
    .post-title {
        font-size: 1.3em;
        margin-bottom: 15px;
        line-height: 1.4;
    }
    .post-title a {
        color: #333;
        text-decoration: none;
        transition: color 0.3s ease;
    }
    .post-title a:hover {
        color: #007bff;
    }
    .post-excerpt {
        color: #666;
        margin-bottom: 20px;
        line-height: 1.6;
    }
    .post-meta {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 10px;
        font-size: 0.9em;
        color: #666;
    }
    .author-info {
        display: flex;
        align-items: center;
    }
    .author-avatar {
        width: 30px;
        height: 30px;
        border-radius: 50%;
        margin-right: 8px;
        object-fit: cover;
    }
    .post-views i {
        margin-right: 5px;
    }
</style>
```

### Category Page with Related Posts
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
    
    // Get category from URL parameter
    var categorySlug = Context.Request.Query["category"].ToString();
    
    // Get category details
    var categoryRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_categories",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "slug", Value = categorySlug, Method = "Equal" }
        }
    };
    var categories = await mixDbDataService.GetListByAsync(categoryRequest);
    var category = categories.FirstOrDefault();
    
    // Get posts in this category
    List<dynamic> posts = new List<dynamic>();
    if(category != null)
    {
        var postsRequest = new SearchMixDbRequestModel
        {
            TableName = "mix_posts",
            Queries = new List<MixQueryField>
            {
                new MixQueryField { FieldName = "category_id", Value = category.Value<int>("objId").ToString(), Method = "Equal" }
            },
            Sorts = new List<MixSort> 
            { 
                new MixSort { FieldName = "published_date", Direction = "Desc" } 
            }
        };
        posts = await mixDbDataService.GetListByAsync(postsRequest);
    }
}

@if(category != null)
{
    <div class="category-page">
        <div class="category-header" style="background: linear-gradient(135deg, @(category.Value<string>("color"))22, @(category.Value<string>("color"))44);">
            <div class="container">
                <div class="category-info">
                    <div class="category-icon">
                        <i class="@(category.Value<string>("icon"))" style="color: @(category.Value<string>("color"));"></i>
                    </div>
                    <h1 class="category-title">@(category.Value<string>("name"))</h1>
                    <p class="category-description">@(category.Value<string>("description"))</p>
                    <div class="category-stats">
                        <span class="posts-count">@posts.Count Posts</span>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="category-content">
            <div class="container">
                @if(posts.Any())
                {
                    <div class="row">
                        @foreach (var post in posts)
                        {
                            <div class="col-lg-4 col-md-6 mb-4">
                                <div class="category-post-card">
                                    <div class="post-image">
                                        <img src="@(post.Value<string>("featured_image"))" 
                                             alt="@(post.Value<string>("title"))" 
                                             class="img-fluid">
                                        @if(post.Value<bool>("featured"))
                                        {
                                            <span class="featured-label">Featured</span>
                                        }
                                    </div>
                                    <div class="post-body">
                                        <h3 class="post-title">@(post.Value<string>("title"))</h3>
                                        <p class="post-excerpt">@(post.Value<string>("excerpt"))</p>
                                        
                                        <div class="post-footer">
                                            <span class="post-date">
                                                @(DateTime.Parse(post.Value<string>("published_date")).ToString("MMM dd"))
                                            </span>
                                            <span class="post-views">
                                                @(post.Value<int>("views")) views
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        }
                    </div>
                }
                else
                {
                    <div class="no-posts">
                        <div class="text-center">
                            <i class="fas fa-file-alt fa-3x text-muted mb-3"></i>
                            <h3>No posts found</h3>
                            <p class="text-muted">There are no posts in this category yet.</p>
                        </div>
                    </div>
                }
            </div>
        </div>
    </div>
}
else
{
    <div class="category-not-found">
        <div class="container text-center">
            <h1>Category Not Found</h1>
            <p>The requested category could not be found.</p>
            <a href="/blog" class="btn btn-primary">Back to Blog</a>
        </div>
    </div>
}

<style>
    .category-header {
        padding: 80px 0;
        text-align: center;
        color: white;
    }
    .category-info {
        max-width: 600px;
        margin: 0 auto;
    }
    .category-icon {
        font-size: 3em;
        margin-bottom: 20px;
    }
    .category-title {
        font-size: 2.5em;
        margin-bottom: 15px;
        font-weight: bold;
    }
    .category-description {
        font-size: 1.2em;
        margin-bottom: 20px;
        opacity: 0.9;
    }
    .posts-count {
        background: rgba(255,255,255,0.2);
        padding: 8px 16px;
        border-radius: 20px;
        font-size: 0.9em;
    }
    .category-content {
        padding: 60px 0;
    }
    .category-post-card {
        border: 1px solid #e9ecef;
        border-radius: 10px;
        overflow: hidden;
        transition: transform 0.3s ease;
        height: 100%;
    }
    .category-post-card:hover {
        transform: translateY(-3px);
        box-shadow: 0 5px 15px rgba(0,0,0,0.1);
    }
    .post-image {
        position: relative;
        height: 200px;
        overflow: hidden;
    }
    .post-image img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
    .featured-label {
        position: absolute;
        top: 10px;
        right: 10px;
        background: #ff6b6b;
        color: white;
        padding: 4px 8px;
        border-radius: 10px;
        font-size: 0.7em;
        font-weight: bold;
    }
    .post-body {
        padding: 20px;
    }
    .post-title {
        font-size: 1.1em;
        margin-bottom: 10px;
        line-height: 1.4;
    }
    .post-excerpt {
        color: #666;
        font-size: 0.9em;
        margin-bottom: 15px;
    }
    .post-footer {
        display: flex;
        justify-content: space-between;
        font-size: 0.8em;
        color: #999;
    }
    .no-posts {
        padding: 60px 0;
    }
</style>
```

### Author Profile with Posts
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
    
    // Get author ID from URL
    int.TryParse(Context.Request.Query["id"], out int authorId);
    
    // Get author details
    var authorRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_authors",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "objId", Value = authorId.ToString(), Method = "Equal" }
        }
    };
    var authors = await mixDbDataService.GetListByAsync(authorRequest);
    var author = authors.FirstOrDefault();
    
    // Get author's posts
    List<dynamic> posts = new List<dynamic>();
    if(author != null)
    {
        var postsRequest = new SearchMixDbRequestModel
        {
            TableName = "mix_posts",
            LoadNestedData = true,
            Queries = new List<MixQueryField>
            {
                new MixQueryField { FieldName = "author_id", Value = authorId.ToString(), Method = "Equal" }
            },
            Sorts = new List<MixSort> 
            { 
                new MixSort { FieldName = "published_date", Direction = "Desc" } 
            }
        };
        posts = await mixDbDataService.GetListByAsync(postsRequest);
    }
}

@if(author != null)
{
    <div class="author-profile">
        <!-- Author Header -->
        <div class="author-header">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-md-3 text-center">
                        <div class="author-avatar-large">
                            <img src="@(author.Value<string>("avatar"))" 
                                 alt="@(author.Value<string>("name"))" 
                                 class="img-fluid rounded-circle">
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="author-info">
                            <h1 class="author-name">@(author.Value<string>("name"))</h1>
                            <p class="author-bio">@(author.Value<string>("bio"))</p>
                            
                            <div class="author-stats">
                                <div class="stat">
                                    <strong>@posts.Count</strong>
                                    <span>Articles</span>
                                </div>
                                <div class="stat">
                                    <strong>@posts.Sum(p => p.Value<int>("views"))</strong>
                                    <span>Total Views</span>
                                </div>
                            </div>
                            
                            <div class="author-links">
                                @if(!string.IsNullOrEmpty(author.Value<string>("website")))
                                {
                                    <a href="@(author.Value<string>("website"))" target="_blank" class="btn btn-outline-primary btn-sm">
                                        <i class="fas fa-globe"></i> Website
                                    </a>
                                }
                                @if(!string.IsNullOrEmpty(author.Value<string>("email")))
                                {
                                    <a href="mailto:@(author.Value<string>("email"))" class="btn btn-outline-secondary btn-sm">
                                        <i class="fas fa-envelope"></i> Email
                                    </a>
                                }
                                
                                @* Parse social links *@
                                @{
                                    var socialLinks = author.Value<string>("social_links");
                                    if(!string.IsNullOrEmpty(socialLinks))
                                    {
                                        var links = socialLinks.Split(',');
                                        foreach(var link in links)
                                        {
                                            var parts = link.Split(':');
                                            if(parts.Length == 2)
                                            {
                                                var platform = parts[0].Trim();
                                                var handle = parts[1].Trim();
                                                
                                                <a href="#" class="btn btn-outline-info btn-sm">
                                                    <i class="fab fa-@platform"></i> @platform
                                                </a>
                                            }
                                        }
                                    }
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Author's Posts -->
        <div class="author-posts">
            <div class="container">
                <h2>Articles by @(author.Value<string>("name"))</h2>
                
                @if(posts.Any())
                {
                    <div class="row">
                        @foreach (var post in posts)
                        {
                            <div class="col-lg-6 mb-4">
                                <div class="author-post-card">
                                    <div class="row no-gutters">
                                        <div class="col-md-4">
                                            <div class="post-thumbnail">
                                                <img src="@(post.Value<string>("featured_image"))" 
                                                     alt="@(post.Value<string>("title"))" 
                                                     class="img-fluid">
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="post-content">
                                                <!-- Category -->
                                                @if(post.RelatedData != null && post.RelatedData.ContainsKey("Category"))
                                                {
                                                    var category = post.RelatedData["Category"].FirstOrDefault();
                                                    if(category != null)
                                                    {
                                                        <div class="post-category">
                                                            <span class="category-badge" style="background: @(category.Value<string>("color"))">
                                                                @(category.Value<string>("name"))
                                                            </span>
                                                        </div>
                                                    }
                                                }
                                                
                                                <h3 class="post-title">
                                                    <a href="/post/@(post.Value<string>("title").Replace(" ", "-").ToLower())">
                                                        @(post.Value<string>("title"))
                                                    </a>
                                                </h3>
                                                
                                                <p class="post-excerpt">@(post.Value<string>("excerpt"))</p>
                                                
                                                <div class="post-meta">
                                                    <span class="post-date">
                                                        @(DateTime.Parse(post.Value<string>("published_date")).ToString("MMM dd, yyyy"))
                                                    </span>
                                                    <span class="post-views">
                                                        <i class="fas fa-eye"></i> @(post.Value<int>("views"))
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        }
                    </div>
                }
                else
                {
                    <div class="no-posts text-center">
                        <p>This author hasn't published any articles yet.</p>
                    </div>
                }
            </div>
        </div>
    </div>
}
else
{
    <div class="author-not-found">
        <div class="container text-center">
            <h1>Author Not Found</h1>
            <p>The requested author could not be found.</p>
            <a href="/authors" class="btn btn-primary">View All Authors</a>
        </div>
    </div>
}

<style>
    .author-header {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        padding: 80px 0;
        color: white;
    }
    .author-avatar-large img {
        width: 150px;
        height: 150px;
        object-fit: cover;
        border: 5px solid rgba(255,255,255,0.3);
    }
    .author-name {
        font-size: 2.5em;
        margin-bottom: 15px;
    }
    .author-bio {
        font-size: 1.1em;
        margin-bottom: 25px;
        opacity: 0.9;
    }
    .author-stats {
        display: flex;
        gap: 30px;
        margin-bottom: 25px;
    }
    .stat {
        text-align: center;
    }
    .stat strong {
        display: block;
        font-size: 1.5em;
        margin-bottom: 5px;
    }
    .stat span {
        font-size: 0.9em;
        opacity: 0.8;
    }
    .author-links .btn {
        margin-right: 10px;
        margin-bottom: 10px;
    }
    .author-posts {
        padding: 60px 0;
    }
    .author-post-card {
        border: 1px solid #e9ecef;
        border-radius: 10px;
        overflow: hidden;
        transition: transform 0.3s ease;
        height: 100%;
    }
    .author-post-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 5px 15px rgba(0,0,0,0.1);
    }
    .post-thumbnail {
        height: 200px;
        overflow: hidden;
    }
    .post-thumbnail img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
    .post-content {
        padding: 20px;
        height: 200px;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }
    .category-badge {
        color: white;
        padding: 4px 8px;
        border-radius: 12px;
        font-size: 0.7em;
        font-weight: bold;
    }
    .post-title {
        font-size: 1.1em;
        margin: 10px 0;
        line-height: 1.4;
    }
    .post-title a {
        color: #333;
        text-decoration: none;
    }
    .post-title a:hover {
        color: #007bff;
    }
    .post-excerpt {
        color: #666;
        font-size: 0.9em;
        margin-bottom: 15px;
        flex-grow: 1;
    }
    .post-meta {
        display: flex;
        justify-content: space-between;
        font-size: 0.8em;
        color: #999;
    }
</style>
```

---

## Advanced Relationship Patterns

### Many-to-Many: Posts and Tags

#### Create Tags Table
```markdown
CreateDatabaseFromPrompt(
    displayName: "Tags",
    schemaDescription: "A table for tags with name (text), slug (text), color (text), description (text)"
)
```

#### Create Junction Table
```markdown
CreateDatabaseFromPrompt(
    displayName: "Post Tags",
    schemaDescription: "A junction table for post-tag relationships with post_id (integer), tag_id (integer)"
)
```

#### Template with Tags
```razor
@{
    // Get post with its tags
    var postId = Context.Request.Query["id"];
    
    // Get post tags from junction table
    var postTagsRequest = new SearchMixDbRequestModel
    {
        TableName = "mix_post_tags",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "post_id", Value = postId, Method = "Equal" }
        }
    };
    var postTags = await mixDbDataService.GetListByAsync(postTagsRequest);
    
    // Get tag details
    var tagIds = postTags.Select(pt => pt.Value<int>("tag_id")).ToList();
    var tags = new List<dynamic>();
    
    foreach(var tagId in tagIds)
    {
        var tagRequest = new SearchMixDbRequestModel
        {
            TableName = "mix_tags",
            Queries = new List<MixQueryField>
            {
                new MixQueryField { FieldName = "objId", Value = tagId.ToString(), Method = "Equal" }
            }
        };
        var tagResult = await mixDbDataService.GetListByAsync(tagRequest);
        if(tagResult.Any())
            tags.Add(tagResult.First());
    }
}

<div class="post-tags">
    @if(tags.Any())
    {
        <h4>Tags:</h4>
        <div class="tags-list">
            @foreach(var tag in tags)
            {
                <a href="/tag/@(tag.Value<string>("slug"))" 
                   class="tag-link" 
                   style="background: @(tag.Value<string>("color"))">
                    #@(tag.Value<string>("name"))
                </a>
            }
        </div>
    }
</div>
```

---

## Troubleshooting Relationships

### Common Issues:
1. **Foreign key mismatches**: Ensure column names match exactly
2. **Missing relationship data**: Verify `LoadNestedData = true`
3. **Performance issues**: Use specific queries instead of loading all data
4. **Null reference errors**: Always check if related data exists

### Verification Checklist:
- ✅ Parent tables created before child tables
- ✅ Foreign key columns have correct data types
- ✅ Relationships defined in both directions
- ✅ Sample data includes valid foreign key values
- ✅ Templates check for null/empty related data

---

## Best Practices for Relationships

### Database Design:
- Use consistent naming conventions (e.g., `category_id`, `author_id`)
- Include proper indexing on foreign key columns
- Document all relationships in schema documentation
- Plan for cascading deletes/updates

### Template Performance:
- Load only necessary related data
- Use pagination for large datasets
- Cache frequently accessed relationship data
- Optimize queries with proper sorting and filtering

### Data Integrity:
- Validate foreign key references before insert/update
- Handle orphaned records appropriately
- Implement proper error handling for missing relationships
- Use transactions for complex relationship operations

---

## Related Workflows

- **[Database Data](./workflow-4-database-data.md)** - Basic database operations
- **[Creating Modules](./workflow-2-creating-modules.md)** - Data-driven modules
- **[Template Patterns](../patterns/)** - Advanced template examples
