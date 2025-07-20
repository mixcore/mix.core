# Mix CMS AI Agent: Your Content Creation Assistant

Welcome! You're working with an AI assistant designed to help you build and manage websites using Mix CMS. This guide will walk you through the essential concepts and tools you'll need.

---

## 1. Core Concepts: Building Blocks of Your Website

Your website is made of two main things: **Templates** (the design and layout) and **Content** (the text and images).

### Templates: The Blueprint for Your Pages

Templates define how your content looks. Think of them as reusable blueprints. We have different types for different jobs:

-   **Master Layouts (`folderType: 7`):** The main skeleton of your site. This is where your site-wide header, footer, and navigation live. **Every page needs one.**
-   **Page Templates (`folderType: 1`):** The layout for a specific type of page, like a blog post or a contact page. It defines the content area within the Master Layout.
-   **Modules (`folderType: 2`):** Reusable blocks of content, like a "Contact Us" form or an image gallery that can be placed on any page.

### Content: The Information on Your Pages

-   **Pages (`CreatePageContent`):** The actual webpages that your visitors see, like "Home" or "About Us". Each page uses a Page Template and a Master Layout to display its content.
-   **Posts (`CreatePostContent`):** Used for blog entries or news articles.
-   **Custom Data (`CreateDatabaseFromPrompt`):** For lists of things, like menu items, products, or staff profiles. Instead of hard-coding them into a page, you can store them in a database table and display them dynamically.

---

## 2. Your Toolbox: The MCP Commands

You have a set of powerful tools (MCP commands) to create and manage your site.

### For Managing Templates

-   `CreateTemplate`: Make a new template.
-   `ListTemplates`: See all existing templates.
-   `UpdateTemplate`: Change an existing template.
-   `DeleteTemplate`: Remove a template.

### For Managing Content

-   `CreatePageContent`: Create a new webpage.
-   `ListPageContents`: See all your pages.
-   `UpdatePageContent`: Change a page's content or settings.
-   `DeletePageContent`: Remove a page.

### For Managing Data

-   `CreateDatabaseFromPrompt`: Create a new database table from a simple description.
-   `CreateManyMixDbData`: Add multiple records (e.g., menu items) to a table at once.
-   `GetListMidxDbData`: Fetch data from a table to display on a page.

---

## 3. How-To Guides: Common Workflows

Here are step-by-step guides for the most common tasks.

### How to Create a New Webpage

Follow these three steps to create a fully functional webpage.

**Step 1: Create the Master Layout (Do this once per site)** First, create the main layout that all your pages will share.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 7` (This is important!)
    -   `fileName`: "MasterLayout.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the full HTML structure, including placeholders for navigation and the main body. **Must include the required Razor sections.**

**Step 2: Create the Page Template** Next, create the specific layout for the content of your new page.

-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 1`
    -   `fileName`: "HomePage.cshtml" or "ContactPage.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Provide the HTML for the page's content area. Use a public URL for any sample images.

**Step 3: Create the Page Content** Finally, create the page itself and link it to the templates you just made.

-   **Tool:** `CreatePageContent`
-   **Parameters:**
    -   `title`: "Welcome to our Restaurant"
    -   `seoName`: "home" (this becomes the URL, e.g., `yoursite.com/home`)
    -   `templateId`: The ID of the **Page Template** from Step 2.
    -   `layoutId`: The ID of the **Master Layout** from Step 1.
    -   `tenantId: 1`

### How to Create Reusable Modules

For reusable components like contact forms, image galleries, or content blocks:

**Step 1: Create the Module Template**
-   **Tool:** `CreateTemplate`
-   **Parameters:**
    -   `folderType: 2` (Modules)
    -   `fileName`: "ContactForm.cshtml"
    -   `mixThemeId: 1`
    -   `content`: Start with `@model dynamic` and add your module HTML

**Step 2: Create the Module Content**
-   **Tool:** `CreateModuleContent`
-   **Parameters:**
    -   `title`: "Contact Form"
    -   `systemName`: "contact_form"
    -   `tenantId: 1`

**Step 3: Use in Page Templates**
Reference modules in your page templates:
```razor
@await Html.PartialAsync("Modules/ContactForm")
```

### How to Handle Lists of Items (e.g., a Food Menu)

If your page has a list of similar items, don't hard-code them. Create a database table for them.

**Step 1: Create a Database Table** Use a simple prompt to define the structure of your data.

-   **Tool:** `CreateDatabaseFromPrompt`
-   **Example:** `CreateDatabaseFromPrompt(displayName: "Menu Items", schemaDescription: "A table for menu items with fields for name (text), description (text), and price (decimal)")`

**Step 2: Add Your Data** Populate the table with your items.

-   **Tool:** `CreateManyMixDbData`
-   **Parameters:**
    -   `databaseSystemName`: "mix_menu_items" (the name from Step 1)
    -   `dataJson`: A JSON array of your items.

**Step 3: Display the Data in a Template** Modify your Page or Module template to fetch and display the data.

1.  **Add the required using statements** at the top of your `.cshtml` file:
    ```razor
    @using Mix.Mixdb.Interfaces
    @using Mix.Shared.Models
    @using Mix.Shared.Dtos
    @using Mix.Constant.Enums
    @using Mix.Constant.Constants
    ```
2.  **Inject the service** after the using statements:
    ```razor
    @inject Mix.Database.Services.MixGlobalSettings.DatabaseService dbSrv;
    @inject IMixDbDataServiceFactory mixDbDataServiceFactory
    ```
3.  **Fetch the data** inside a code block:
    ```csharp
    @{
        var mixDbDataService = mixDbDataServiceFactory.Create(dbSrv.DatabaseProvider, dbSrv.GetConnectionString(MixConstants.CONST_CMS_CONNECTION));
        var request = new SearchMixDbRequestModel
        {
            TableName = "mix_menu_items",
            Queries = new List<MixQueryField>
            {
                new MixQueryField { FieldName = "is_featured", Value = true, CompareOperator = MixCompareOperator.Equal }
            }
        };
        var menuItems = await mixDbDataServiceFactory.GetListByAsync(request);
    }
    ```
    
    **Common Query Examples:**
    ```csharp
    // Get all items (no filter)
    var request = new SearchMixDbRequestModel { TableName = "mix_menu_items" };
    
    // Filter by category
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_menu_items",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "category", Value = "appetizer", CompareOperator = MixCompareOperator.Equal }
        }
    };
    
    // Search by name (contains)
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_menu_items",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "name", Value = "chicken", CompareOperator = MixCompareOperator.Like }
        }
    };
    
    // Multiple conditions (AND)
    var request = new SearchMixDbRequestModel
    {
        TableName = "mix_menu_items",
        Queries = new List<MixQueryField>
        {
            new MixQueryField { FieldName = "is_featured", Value = true, CompareOperator = MixCompareOperator.Equal },
            new MixQueryField { FieldName = "price", Value = 20, CompareOperator = MixCompareOperator.LessThan }
        }
    };
    ```
4.  **Loop through and display** the data:
    ```razor
    @foreach(var item in menuItems)
    {
        <div class="menu-item">
            <h3>@item["name"]</h3>
            <p>@item["description"]</p>
            <span>$@item["price"]</span>
        </div>
    }
    ```

---

## 4. Best Practices & Key Reminders

-   **Check MCP Tool Support First:** Before executing any task, check if there's an existing MCP tool that can help accomplish it. Use `ListSections` to explore available tools and resources.
-   **Database Schema Documentation:** When creating new tables, document the schema in your project's `DATABASE_SCHEMA.md` file. Include table name, columns, data types, and relationships. Use `GetTableSchema` to retrieve schema details.
-   **Schema Verification for Content Rendering:** Before creating content that loads data from MixDb, always check the database schema using `GetTableSchema` to ensure you understand the structure. This ensures you use the correct field names when rendering data in templates.
-   **Master Layouts First:** Always create your `folderType: 7` Master Layout before creating pages.
-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"HomePage"`, not `"HomePage.cshtml"`)
    - The system will automatically combine them to create the full filename
-   **Module Rendering Pattern:**
    - Always create module content first using `CreateModuleContent`
    - Then create template with `.cshtml` extension
    - Associate posts/content using `CreateModulePostAssociation`
    - Render in templates using:
      ```razor
      @{
          var module = Model.GetModule("moduleSystemName");
      }
      @if(module != null){
          @await Html.PartialAsync(module.Template.FilePath, module, null);
      }
      ```
-   **Required Using Statements:** When using MixDb data in templates, always include these using statements at the top:
    ```razor
    @using Mix.Mixdb.Interfaces
    @using Mix.Shared.Models
    @using Mix.Shared.Dtos
    @using Mix.Constant.Enums
    @inject IMixDbDataService MixDbDataService
    ```
-   **Public Image URLs:** When using images in templates, always use full, public URLs (e.g., from Unsplash). Do not use local file paths.
-   **Required Razor Sections:** Your Master Layout template **must** include these lines for styles and scripts to work correctly:
    ```razor
    @RenderSection("Schema", false)     
    @RenderSection("Seo", false)     
    <!--[STYLES]-->
    @RenderSection("Styles", false)   
    @RenderSection("Scripts", false)   
    ```
-   **Check for Existing Templates:** Use `ListTemplates` to avoid creating duplicates.

---

## 5. Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` to check first.
-   **Page is missing header/footer:** You likely forgot to assign the `layoutId` when you created the page with `CreatePageContent`. You can fix this with `UpdatePageContent`.
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.
-   **Data not displaying:** Check that you have the correct using statements, service injection, and that your `SearchMixDbRequestModel` uses the right property names (`FieldName`, `CompareOperator`).
-   **Incorrect field names in templates:** If you're seeing null values or errors when rendering data, verify the database schema using `GetTableSchema` to ensure you're using the correct field names. Field names are case-sensitive.
-   **Wrong comparison operator:** Use `MixCompareOperator.Equal`, `MixCompareOperator.Like`, `MixCompareOperator.LessThan`, etc. (not `ExpressionMethod`).

### MCP Response Format
MCP commands return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and pages together.

---

## 6. Important Distinction: MCP Commands vs Template Code

**MCP Commands** are the tools you use to create and manage your site structure (like `CreateTemplate`, `CreatePageContent`, `GetListMidxDbData`). These are called through the AI assistant.

**Template Code** is the Razor/C# code you write inside your `.cshtml` templates to display data dynamically. This code uses the Mix CMS services directly.

### When to Use Which:

- **Use MCP Commands to:**
  - Create templates and pages
  - Set up database tables
  - Add initial data to tables
  - Manage your site structure

- **Use Template Code to:**
  - Display data from database tables on your web pages
  - Create dynamic content that changes based on data
  - Implement search, filtering, and pagination

### Example Workflow:
1. **MCP Command:** `CreateDatabaseFromPrompt` to create a "products" table
2. **MCP Command:** `CreateManyMixDbData` to add product records
3. **Template Code:** Use `SearchMixDbRequestModel` and `IMixDbDataService` in your `.cshtml` file to display the products

---

## 7. Quick Reference: Essential Commands

Here are the most common commands for quick access.

### Template & Page Creation Workflow
1.  **Create Master Layout:**
    `CreateTemplate(content, fileName: "MasterLayout.cshtml", folderType: 7, mixThemeId: 1)`
    *Returns the `master_layout_id`.*

2.  **Create Page Template:**
    `CreateTemplate(content, fileName: "MyPageTemplate.cshtml", folderType: 1, mixThemeId: 1)`
    *Returns the `page_template_id`.*

3.  **Create Page Content:**
    `CreatePageContent(title, content, seoName, excerpt, templateId: {page_template_id}, layoutId: {master_layout_id}, tenantId: 1)`

### Finding Your Content
-   **List Templates:** `ListTemplates(folderType: 7)`
-   **Get Page by URL:** `GetPageContentBySeoName(seoName: "home")`

---

## Additional Resources

- **[MCP Tools Reference](./mcp-tools-reference.md)** - Complete MCP command documentation with parameters and examples
- **[Developer Guide](./developer-guide.md)** - Technical guide for C# developers building MCP tools
- **[Instructions README](./README.md)** - Overview of all documentation files
