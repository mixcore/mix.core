# AI Workflows: Creating Basic Modules and Templates

This guide provides step-by-step workflows for creating basic website modules and templates using Mix CMS MCP Tools for CRUD operations.

---

## How to Create a New Module

Follow these steps to create a fully functional module using MCP Tools.

### Step 1: Check Existing Templates
Use `ListTemplates` to see all available templates and avoid creating duplicates. You'll need to identify:
- **Master Layout Template**: A template with `folderType` = "Masters" (for layoutId)
- **Module Template**: A template with `folderType` = "Modules" (for templateId)

### Step 2: Create Templates (if needed)
If you don't have the required templates, create them first:
- **Master Layout**: Use `CreateTemplate` with `folderType: 7` (Masters)
- **Module Template**: Use `CreateTemplate` with `folderType: 2` (Modules)

### Step 3: Create the Module Content

Finally, create the module itself using MCP Tools and link it to the templates you identified/created.

-   **MCP Tool:** `CreateModuleContent`
-   **Parameters:**
    -   `title`: "Featured Products Module"
    -   `excerpt`: "A module to display featured products on the homepage"
    -   `systemName`: "featured-products" (this becomes the module identifier)
    -   `pageSize`: 10 (number of items to display)
    -   `tenantId`: 1
    -   `type`: 0 (module type)

---

## CRUD Operations for Module Content

Use these MCP Tools for managing module content:

### Create Module Content
-   **MCP Tool:** `CreateModuleContent`
-   **Purpose:** Create new modules

### Read Module Content
-   **MCP Tool:** `GetModuleContent` (by ID)
-   **MCP Tool:** `ListModuleContents` (list multiple modules with filtering)
-   **Purpose:** Retrieve existing module data

### Update Module Content
-   **MCP Tool:** `UpdateModuleContent`
-   **Purpose:** Modify existing modules
-   **Required:** `id` parameter (from create/list operations)

### Delete Module Content
-   **MCP Tool:** `DeleteModuleContent`
-   **Purpose:** Remove modules
-   **Required:** `id` parameter and `confirmDelete: "YES"`

---

## Template Naming Best Practices

-   **Template Naming:**
    - The `extension` parameter must be `.cshtml` (e.g., `".cshtml"`) - always include the dot
    - The `fileName` parameter should NOT include `.cshtml` (e.g., `"FeaturedProducts"`, not `"FeaturedProducts.cshtml"`)
    - The system will automatically combine them to create the full filename

-   **Template Identification:**
    - **layoutId**: Must be the ID of a template with `folderType` = "Masters" (folderType: 7)
    - **templateId**: Must be the ID of a template with `folderType` = "Modules" (folderType: 2)
    - Use `ListTemplates` MCP Tool to find existing templates and their folderType values

-   **Check for Existing Templates:** Use `ListTemplates` MCP Tool to avoid creating duplicates.

---

## Partial Rendering Patterns

When rendering partial templates, always follow the naming pattern `$"../{template.FolderType.ToString()}/{templateName}.cshtml"`:

For modules, this would be: `"../Modules/FeaturedProducts.cshtml"`

## Common Issues and Solutions

### Troubleshooting

-   **"Template already exists" error:** You tried to create a template with a `fileName` that's already in use. Use `ListTemplates` MCP Tool to check first.
-   **Module not displaying:** You likely forgot to assign the correct template or the module template has rendering issues.
-   **Template linking issues:** Ensure `templateId` points to a template with `folderType` = "Modules" and `layoutId` points to a template with `folderType` = "Masters".
-   **Styles look broken:** Make sure your Master Layout includes the required Razor sections mentioned above.

### MCP Response Format
MCP Tools return JSON responses. Successful operations typically include:
- `Success`: true/false
- `Data`: The created/updated object with an `id` field
- `Message`: Description of what happened

Always check the `id` in the response - you'll need these IDs to link templates and modules together.

### Workflow Example
1. Run `ListTemplates` to see available templates
2. Identify Master Layout (folderType = "Masters") for `layoutId`
3. Identify Module Template (folderType = "Modules") for `templateId`
4. Use `CreateModuleContent` with the correct configuration
5. Verify with `GetModuleContent` or `ListModuleContents`

### Task Documentation (CRITICAL)
**After successfully completing any module creation task, document it in your project's `project-progress.md` file:**

```markdown
## Completed Tasks
### 2025-01-XX - Featured Products Module Creation
- **Master Layout:** Using existing MasterLayout.cshtml (folderType: 7)
- **Module Template:** Created FeaturedProducts.cshtml (folderType: 2) 
- **Module Content:** Created "Featured Products" module with systemName: featured-products
- **Status:** ✅ Complete - Module accessible and rendering correctly
- **Notes:** Displays 10 featured products, uses card layout with images

### 2025-01-XX - Newsletter Signup Module
- **Module Template:** Created NewsletterSignup.cshtml (folderType: 2)
- **Module Content:** Created "Newsletter Signup" module 
- **Status:** ✅ Complete - Form ready for email integration
```

This documentation ensures other team members can build upon your work and understand the current project state.

---

## Next Steps

Once you've mastered basic module creation, explore:
- **[Working with Dynamic Data](./ai-workflows-mixdb-data.md)** - Create database-driven module content
- **[Creating Blog Posts](./ai-workflows-posts.md)** - Set up blog functionality
- **[Advanced Module Patterns](./ai-template-patterns.md)** - Learn advanced template techniques
