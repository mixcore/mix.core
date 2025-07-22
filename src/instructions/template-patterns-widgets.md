# Widget Templates (folderType: 6)

Widget templates create small, reusable UI components that provide specific functionality or display targeted content. They are perfect for sidebars, footers, and modular page elements.

---

## Overview

Widget templates (`folderType: 6`) are small, focused components designed for specific functionality or content display. They are highly reusable and can be embedded anywhere in your website.

### Key Characteristics
- **Purpose:** Small, focused UI components
- **Model:** `@model dynamic` or specific widget models
- **Usage:** Embedded in pages, modules, or layouts
- **Features:** Lightweight, reusable, targeted functionality

---

## Creating Widget Templates

### MCP Command
```csharp
CreateTemplate(
    folderType: 6,
    fileName: "SearchWidget",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<div class=\"search-widget\"><!-- widget content --></div>"
)
```

### Basic Widget Template Structure
```razor
@model dynamic

<div class="widget-container">
    <div class="widget-header">
        <h4>Widget Title</h4>
    </div>
    <div class="widget-content">
        <!-- Widget content -->
    </div>
</div>
```

---

## Widget Template Examples

### Search Widget
```razor
@model dynamic

<div class="search-widget">
    <div class="widget-header">
        <h4>Search</h4>
    </div>
    <div class="widget-content">
        <form class="search-form" action="/search" method="get">
            <div class="input-group">
                <input type="text" name="q" class="form-control" 
                       placeholder="Search..." value="@ViewBag.SearchQuery">
                <button type="submit" class="btn btn-primary">
                    <i class="fas fa-search"></i>
                </button>
            </div>
        </form>
        
        @if (!string.IsNullOrEmpty(ViewBag.SearchQuery))
        {
            <div class="search-results-info">
                <small class="text-muted">
                    Showing results for "@ViewBag.SearchQuery"
                </small>
            </div>
        }
    </div>
</div>

<style>
    .search-widget {
        background: #f8f9fa;
        border-radius: 8px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header h4 {
        margin-bottom: 1rem;
        color: #495057;
        font-size: 1.1rem;
    }
    .search-form .input-group {
        margin-bottom: 0.5rem;
    }
    .search-form .form-control {
        border-radius: 20px 0 0 20px;
    }
    .search-form .btn {
        border-radius: 0 20px 20px 0;
        padding: 0.5rem 1rem;
    }
    .search-results-info {
        margin-top: 0.5rem;
    }
</style>
```

### Recent Posts Widget
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
        Queries = new List<MixQueryField>(),
        Sorts = new List<MixSort> 
        { 
            new MixSort { FieldName = "CreatedDateTime", Direction = "Desc" } 
        }
    };
    var recentPosts = await mixDbDataService.GetListByAsync(request);
}

<div class="recent-posts-widget">
    <div class="widget-header">
        <h4>Recent Posts</h4>
        <a href="/blog" class="widget-link">View All</a>
    </div>
    <div class="widget-content">
        @if (recentPosts.Any())
        {
            <ul class="post-list">
                @foreach (var post in recentPosts.Take(5))
                {
                    <li class="post-item">
                        <a href="/post/@(post.Value<string>("SeoName"))" class="post-link">
                            @if (!string.IsNullOrEmpty(post.Value<string>("FeaturedImage")))
                            {
                                <div class="post-thumbnail">
                                    <img src="@(post.Value<string>("FeaturedImage"))" 
                                         alt="@(post.Value<string>("Title"))">
                                </div>
                            }
                            <div class="post-info">
                                <h6 class="post-title">@(post.Value<string>("Title"))</h6>
                                <small class="post-date">
                                    @(post.Value<DateTime>("CreatedDateTime").ToString("MMM dd, yyyy"))
                                </small>
                            </div>
                        </a>
                    </li>
                }
            </ul>
        }
        else
        {
            <p class="no-posts">No recent posts available.</p>
        }
    </div>
</div>

<style>
    .recent-posts-widget {
        background: #fff;
        border: 1px solid #e9ecef;
        border-radius: 8px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
        padding-bottom: 0.5rem;
        border-bottom: 1px solid #e9ecef;
    }
    .widget-header h4 {
        margin: 0;
        color: #495057;
        font-size: 1.1rem;
    }
    .widget-link {
        color: #007bff;
        text-decoration: none;
        font-size: 0.9rem;
    }
    .post-list {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    .post-item {
        margin-bottom: 1rem;
    }
    .post-item:last-child {
        margin-bottom: 0;
    }
    .post-link {
        display: flex;
        align-items: center;
        text-decoration: none;
        color: inherit;
    }
    .post-link:hover {
        color: #007bff;
    }
    .post-thumbnail {
        width: 50px;
        height: 50px;
        margin-right: 1rem;
        flex-shrink: 0;
    }
    .post-thumbnail img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        border-radius: 4px;
    }
    .post-info {
        flex: 1;
    }
    .post-title {
        margin: 0 0 0.25rem 0;
        font-size: 0.9rem;
        line-height: 1.3;
    }
    .post-date {
        color: #6c757d;
    }
    .no-posts {
        color: #6c757d;
        font-style: italic;
        margin: 0;
    }
</style>
```

### Social Media Widget
```razor
@model dynamic

<div class="social-media-widget">
    <div class="widget-header">
        <h4>Follow Us</h4>
    </div>
    <div class="widget-content">
        <div class="social-links">
            <a href="https://facebook.com/yourpage" class="social-link facebook" target="_blank">
                <i class="fab fa-facebook-f"></i>
                <span class="social-label">Facebook</span>
                <span class="social-count">2.5K</span>
            </a>
            
            <a href="https://twitter.com/yourhandle" class="social-link twitter" target="_blank">
                <i class="fab fa-twitter"></i>
                <span class="social-label">Twitter</span>
                <span class="social-count">1.8K</span>
            </a>
            
            <a href="https://instagram.com/youraccount" class="social-link instagram" target="_blank">
                <i class="fab fa-instagram"></i>
                <span class="social-label">Instagram</span>
                <span class="social-count">3.2K</span>
            </a>
            
            <a href="https://linkedin.com/company/yourcompany" class="social-link linkedin" target="_blank">
                <i class="fab fa-linkedin-in"></i>
                <span class="social-label">LinkedIn</span>
                <span class="social-count">892</span>
            </a>
            
            <a href="https://youtube.com/yourchannel" class="social-link youtube" target="_blank">
                <i class="fab fa-youtube"></i>
                <span class="social-label">YouTube</span>
                <span class="social-count">1.1K</span>
            </a>
        </div>
    </div>
</div>

<style>
    .social-media-widget {
        background: #fff;
        border: 1px solid #e9ecef;
        border-radius: 8px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header h4 {
        margin-bottom: 1rem;
        color: #495057;
        font-size: 1.1rem;
    }
    .social-links {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
    }
    .social-link {
        display: flex;
        align-items: center;
        padding: 0.75rem;
        border-radius: 6px;
        text-decoration: none;
        color: white;
        transition: transform 0.2s ease;
    }
    .social-link:hover {
        transform: translateY(-2px);
        color: white;
    }
    .social-link i {
        width: 20px;
        text-align: center;
        margin-right: 1rem;
    }
    .social-label {
        flex: 1;
        font-weight: 500;
    }
    .social-count {
        font-size: 0.9rem;
        opacity: 0.9;
    }
    
    /* Social platform colors */
    .facebook { background: #3b5998; }
    .twitter { background: #1da1f2; }
    .instagram { background: linear-gradient(45deg, #f09433 0%, #e6683c 25%, #dc2743 50%, #cc2366 75%, #bc1888 100%); }
    .linkedin { background: #0077b5; }
    .youtube { background: #ff0000; }
</style>
```

### Newsletter Signup Widget
```razor
@model dynamic

<div class="newsletter-widget">
    <div class="widget-header">
        <h4>Stay Updated</h4>
    </div>
    <div class="widget-content">
        <p class="newsletter-description">
            Get the latest news and updates delivered straight to your inbox.
        </p>
        
        <form class="newsletter-form" id="newsletterForm">
            <div class="form-group">
                <input type="email" class="form-control" name="email" 
                       placeholder="Enter your email address" required>
            </div>
            <div class="form-group">
                <select class="form-control" name="frequency">
                    <option value="weekly">Weekly Digest</option>
                    <option value="daily">Daily Updates</option>
                    <option value="monthly">Monthly Newsletter</option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary btn-block">
                Subscribe Now
            </button>
        </form>
        
        <div class="newsletter-success d-none">
            <div class="success-message">
                <i class="fas fa-check-circle text-success"></i>
                <p>Thank you for subscribing! Check your email to confirm.</p>
            </div>
        </div>
        
        <div class="newsletter-privacy">
            <small class="text-muted">
                We respect your privacy. Unsubscribe anytime.
            </small>
        </div>
    </div>
</div>

<style>
    .newsletter-widget {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border-radius: 8px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header h4 {
        margin-bottom: 1rem;
        color: white;
        font-size: 1.1rem;
    }
    .newsletter-description {
        margin-bottom: 1.5rem;
        font-size: 0.9rem;
        opacity: 0.9;
    }
    .newsletter-form .form-control {
        border: none;
        border-radius: 6px;
        padding: 0.75rem 1rem;
        margin-bottom: 1rem;
    }
    .newsletter-form .btn {
        background: rgba(255, 255, 255, 0.2);
        border: 1px solid rgba(255, 255, 255, 0.3);
        color: white;
        font-weight: 600;
    }
    .newsletter-form .btn:hover {
        background: rgba(255, 255, 255, 0.3);
        border-color: rgba(255, 255, 255, 0.5);
    }
    .success-message {
        text-align: center;
        padding: 1rem;
    }
    .success-message i {
        font-size: 2rem;
        margin-bottom: 0.5rem;
        display: block;
    }
    .success-message p {
        margin: 0;
    }
    .newsletter-privacy {
        text-align: center;
        margin-top: 1rem;
    }
</style>

<script>
document.getElementById('newsletterForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    // Simulate subscription process
    setTimeout(() => {
        this.style.display = 'none';
        document.querySelector('.newsletter-success').classList.remove('d-none');
    }, 1000);
});
</script>
```

### Weather Widget
```razor
@model dynamic

<div class="weather-widget">
    <div class="widget-header">
        <h4>Weather</h4>
        <button class="refresh-btn" onclick="refreshWeather()">
            <i class="fas fa-sync-alt"></i>
        </button>
    </div>
    <div class="widget-content">
        <div class="current-weather">
            <div class="weather-main">
                <div class="weather-icon">
                    <i class="fas fa-sun" id="weatherIcon"></i>
                </div>
                <div class="weather-info">
                    <div class="temperature">
                        <span id="currentTemp">72</span>°F
                    </div>
                    <div class="weather-desc" id="weatherDesc">Sunny</div>
                </div>
            </div>
            <div class="location" id="location">New York, NY</div>
        </div>
        
        <div class="weather-details">
            <div class="detail-item">
                <span class="detail-label">Feels like</span>
                <span class="detail-value" id="feelsLike">75°F</span>
            </div>
            <div class="detail-item">
                <span class="detail-label">Humidity</span>
                <span class="detail-value" id="humidity">65%</span>
            </div>
            <div class="detail-item">
                <span class="detail-label">Wind</span>
                <span class="detail-value" id="windSpeed">8 mph</span>
            </div>
        </div>
        
        <div class="forecast">
            <h6>5-Day Forecast</h6>
            <div class="forecast-list">
                <div class="forecast-item">
                    <span class="day">Mon</span>
                    <i class="fas fa-cloud-sun"></i>
                    <span class="temp">74°</span>
                </div>
                <div class="forecast-item">
                    <span class="day">Tue</span>
                    <i class="fas fa-cloud-rain"></i>
                    <span class="temp">68°</span>
                </div>
                <div class="forecast-item">
                    <span class="day">Wed</span>
                    <i class="fas fa-sun"></i>
                    <span class="temp">76°</span>
                </div>
                <div class="forecast-item">
                    <span class="day">Thu</span>
                    <i class="fas fa-cloud"></i>
                    <span class="temp">71°</span>
                </div>
                <div class="forecast-item">
                    <span class="day">Fri</span>
                    <i class="fas fa-sun"></i>
                    <span class="temp">78°</span>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .weather-widget {
        background: linear-gradient(135deg, #74b9ff 0%, #0984e3 100%);
        color: white;
        border-radius: 12px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1.5rem;
    }
    .widget-header h4 {
        margin: 0;
        color: white;
        font-size: 1.1rem;
    }
    .refresh-btn {
        background: none;
        border: none;
        color: white;
        cursor: pointer;
        padding: 0.25rem;
        border-radius: 4px;
        transition: background 0.2s ease;
    }
    .refresh-btn:hover {
        background: rgba(255, 255, 255, 0.2);
    }
    .weather-main {
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
    }
    .weather-icon {
        font-size: 3rem;
        margin-right: 1rem;
    }
    .temperature {
        font-size: 2rem;
        font-weight: bold;
        margin-bottom: 0.25rem;
    }
    .weather-desc {
        font-size: 0.9rem;
        opacity: 0.9;
    }
    .location {
        text-align: center;
        font-size: 0.9rem;
        opacity: 0.8;
        margin-bottom: 1rem;
    }
    .weather-details {
        display: flex;
        justify-content: space-between;
        margin-bottom: 1.5rem;
        padding: 1rem;
        background: rgba(255, 255, 255, 0.1);
        border-radius: 8px;
    }
    .detail-item {
        text-align: center;
    }
    .detail-label {
        display: block;
        font-size: 0.8rem;
        opacity: 0.8;
        margin-bottom: 0.25rem;
    }
    .detail-value {
        font-weight: bold;
    }
    .forecast h6 {
        margin-bottom: 1rem;
        text-align: center;
    }
    .forecast-list {
        display: flex;
        justify-content: space-between;
    }
    .forecast-item {
        text-align: center;
        font-size: 0.8rem;
    }
    .forecast-item .day {
        display: block;
        margin-bottom: 0.5rem;
        opacity: 0.8;
    }
    .forecast-item i {
        display: block;
        margin-bottom: 0.5rem;
        font-size: 1.2rem;
    }
    .forecast-item .temp {
        font-weight: bold;
    }
</style>

<script>
function refreshWeather() {
    const refreshBtn = document.querySelector('.refresh-btn i');
    refreshBtn.style.animation = 'spin 1s linear';
    
    setTimeout(() => {
        refreshBtn.style.animation = '';
        // Here you would typically fetch new weather data
    }, 1000);
}

// Add spin animation
const style = document.createElement('style');
style.textContent = `
    @keyframes spin {
        from { transform: rotate(0deg); }
        to { transform: rotate(360deg); }
    }
`;
document.head.appendChild(style);
</script>
```

### Quick Contact Widget
```razor
@model dynamic

<div class="quick-contact-widget">
    <div class="widget-header">
        <h4>Quick Contact</h4>
    </div>
    <div class="widget-content">
        <div class="contact-methods">
            <div class="contact-method">
                <div class="method-icon phone">
                    <i class="fas fa-phone"></i>
                </div>
                <div class="method-info">
                    <h6>Call Us</h6>
                    <a href="tel:+15551234567">+1 (555) 123-4567</a>
                </div>
            </div>
            
            <div class="contact-method">
                <div class="method-icon email">
                    <i class="fas fa-envelope"></i>
                </div>
                <div class="method-info">
                    <h6>Email Us</h6>
                    <a href="mailto:contact@example.com">contact@example.com</a>
                </div>
            </div>
            
            <div class="contact-method">
                <div class="method-icon chat">
                    <i class="fas fa-comments"></i>
                </div>
                <div class="method-info">
                    <h6>Live Chat</h6>
                    <button class="chat-btn" onclick="openChat()">Start Chat</button>
                </div>
            </div>
        </div>
        
        <div class="quick-form">
            <h6>Send Quick Message</h6>
            <form class="mini-contact-form" id="quickContactForm">
                <input type="text" name="name" placeholder="Your Name" class="form-control" required>
                <input type="email" name="email" placeholder="Your Email" class="form-control" required>
                <textarea name="message" placeholder="Your Message" class="form-control" rows="3" required></textarea>
                <button type="submit" class="btn btn-primary btn-sm btn-block">Send</button>
            </form>
        </div>
        
        <div class="business-hours">
            <h6>Business Hours</h6>
            <div class="hours-list">
                <div class="hours-item">
                    <span>Mon - Fri</span>
                    <span>9:00 AM - 6:00 PM</span>
                </div>
                <div class="hours-item">
                    <span>Saturday</span>
                    <span>10:00 AM - 4:00 PM</span>
                </div>
                <div class="hours-item">
                    <span>Sunday</span>
                    <span>Closed</span>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .quick-contact-widget {
        background: #fff;
        border: 1px solid #e9ecef;
        border-radius: 8px;
        padding: 1.5rem;
        margin-bottom: 1rem;
    }
    .widget-header h4 {
        margin-bottom: 1.5rem;
        color: #495057;
        font-size: 1.1rem;
    }
    .contact-methods {
        margin-bottom: 1.5rem;
    }
    .contact-method {
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
        padding: 0.75rem;
        background: #f8f9fa;
        border-radius: 6px;
    }
    .method-icon {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-right: 1rem;
        color: white;
    }
    .method-icon.phone { background: #28a745; }
    .method-icon.email { background: #007bff; }
    .method-icon.chat { background: #ffc107; color: #212529; }
    .method-info h6 {
        margin: 0 0 0.25rem 0;
        font-size: 0.9rem;
    }
    .method-info a {
        color: #495057;
        text-decoration: none;
        font-size: 0.85rem;
    }
    .method-info a:hover {
        color: #007bff;
    }
    .chat-btn {
        background: none;
        border: none;
        color: #007bff;
        padding: 0;
        font-size: 0.85rem;
        cursor: pointer;
    }
    .quick-form {
        margin-bottom: 1.5rem;
        padding-top: 1rem;
        border-top: 1px solid #e9ecef;
    }
    .quick-form h6 {
        margin-bottom: 1rem;
        font-size: 0.9rem;
    }
    .mini-contact-form .form-control {
        font-size: 0.85rem;
        padding: 0.5rem;
        margin-bottom: 0.75rem;
        border-radius: 4px;
    }
    .business-hours {
        padding-top: 1rem;
        border-top: 1px solid #e9ecef;
    }
    .business-hours h6 {
        margin-bottom: 1rem;
        font-size: 0.9rem;
    }
    .hours-item {
        display: flex;
        justify-content: space-between;
        margin-bottom: 0.5rem;
        font-size: 0.85rem;
    }
    .hours-item:last-child {
        margin-bottom: 0;
    }
</style>

<script>
function openChat() {
    alert('Chat feature would open here');
    // Implement your chat system integration
}

document.getElementById('quickContactForm').addEventListener('submit', function(e) {
    e.preventDefault();
    alert('Message sent! We\'ll get back to you soon.');
    this.reset();
});
</script>
```

---

## Best Practices

### Widget Design
- **Single Purpose:** Each widget should have one clear function
- **Consistent Styling:** Match your site's design system
- **Responsive Design:** Ensure widgets work on all devices
- **Loading Performance:** Keep widgets lightweight and fast

### Code Organization
- **Modular CSS:** Use scoped styles within widgets
- **Error Handling:** Include fallbacks for failed data loading
- **Accessibility:** Ensure widgets are keyboard and screen reader accessible
- **Configuration:** Make widgets easily configurable

### Data Management
- **Efficient Queries:** Optimize database calls
- **Caching:** Cache widget data when appropriate
- **Real-time Updates:** Consider WebSocket connections for dynamic data
- **Fallback Content:** Show meaningful content when data is unavailable

---

## Rendering Widgets

### In Page Templates
```razor
<!-- Direct widget inclusion -->
@await Html.PartialAsync("../Widgets/SearchWidget.cshtml")

<!-- Conditional widget rendering -->
@if (ViewBag.ShowWeatherWidget == true)
{
    @await Html.PartialAsync("../Widgets/WeatherWidget.cshtml")
}
```

### In Master Layouts
```razor
<!-- Sidebar widgets in layout -->
<aside class="sidebar">
    @await Html.PartialAsync("../Widgets/SearchWidget.cshtml")
    @await Html.PartialAsync("../Widgets/RecentPostsWidget.cshtml")
    @await Html.PartialAsync("../Widgets/NewsletterWidget.cshtml")
</aside>
```

### Dynamic Widget Loading
```razor
@{
    var widgets = ViewBag.SidebarWidgets as List<string> ?? new List<string>();
}

@foreach (var widget in widgets)
{
    @await Html.PartialAsync($"../Widgets/{widget}.cshtml")
}
```

---

## Widget Configuration

### Configurable Widget Example
```razor
@model dynamic

@{
    var config = ViewBag.WidgetConfig ?? new {
        Title = "Default Title",
        ShowHeader = true,
        MaxItems = 5,
        Theme = "default"
    };
}

<div class="configurable-widget @($"theme-{config.Theme}")">
    @if (config.ShowHeader)
    {
        <div class="widget-header">
            <h4>@config.Title</h4>
        </div>
    }
    <div class="widget-content">
        <!-- Widget content with configuration -->
    </div>
</div>
```

---

## Troubleshooting

### Common Issues

**Widget not displaying:**
- Check partial path: `../Widgets/WidgetName.cshtml`
- Verify widget template exists with `folderType: 6`
- Ensure template syntax is valid

**Styling conflicts:**
- Use unique CSS class names
- Test widget in different contexts
- Check for CSS inheritance issues

**Performance problems:**
- Optimize database queries
- Implement caching where appropriate
- Minimize external API calls

**JavaScript errors:**
- Check for conflicts with other scripts
- Use proper event handling
- Include error handling in widget scripts

---

## Next Steps

After creating widget templates:

1. **Test Thoroughly** - Verify widgets work in different contexts
2. **Optimize Performance** - Cache data and minimize resource usage
3. **Add Configuration** - Make widgets easily customizable
4. **Monitor Usage** - Track widget performance and user interaction

---

## Related Guides

- **[Page Templates](./template-patterns-pages.md)** - Embedding widgets in pages
- **[Master Layouts](./template-patterns-masters.md)** - Adding widgets to layouts
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
