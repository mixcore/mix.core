# Mix Content Module Creation Workflow

## 1. Template Creation First
Always create the template first before creating the module content. This ensures you have the template ID to reference.

Example:
```bash
CreateTemplate with parameters:
- fileName: "ModuleName"
- folderType: 2 (Modules)
- mixThemeId: 1
- content: "@model dynamic\n[Your module HTML content]"
```

## 2. Module Content Creation
After template creation, use the returned template ID when creating the module:

```bash
CreateModuleContent with parameters:
- title: "Module Title"
- systemName: "module_system_name"
- templateId: [Template ID from step 1]
```

## 3. Page Template Rendering
In page templates, render modules using partial views following this pattern:

```html
@{
    Layout = "../Masters/YourLayout.cshtml";
}

@await Html.PartialAsync("Modules/ModuleName")
@await Html.PartialAsync("Modules/AnotherModule")

@RenderSection("Scripts", false)
```

## Best Practices
1. Keep module templates focused on single responsibilities
2. Use consistent naming: "CategoryName" (e.g. "HealthcareHero")
3. Store modules in the Modules folder (folderType: 2)
4. Reference templates by their file name without extension in PartialAsync calls
5. Always include `@model dynamic` in module templates
6. Use the healthcare modular template as reference: 
   `src/applications/mixcore/wwwroot/mixcontent/templates/demo_mix_agent/default/Pages/HealthcareHomeModular.cshtml`
