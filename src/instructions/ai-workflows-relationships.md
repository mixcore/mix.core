# AI Workflows: Creating Database Relationships with MCP Tools

This guide provides comprehensive instructions for AI agents on when and how to create database relationships using Mix CMS MCP Tools.

---

## When to Create Relationships

### Scenarios Requiring Relationships

1. **Page-Module Connections**
   - When pages need to display multiple modules (header, content, sidebar, footer)
   - When modules need to be reused across different pages
   - When dynamic content requires nested module loading

2. **Post-Category Organization**
   - When blog posts need to be categorized
   - When articles need tags or classification
   - When content needs to be grouped by topics

3. **Content Hierarchies**
   - When building parent-child content structures
   - When creating navigation hierarchies
   - When implementing content inheritance

4. **Cross-Reference Content**
   - When content needs to reference other content
   - When building recommendation systems
   - When creating content dependencies

---

## Available MCP Tools for Relationships

### Primary Tool: CreateMixDbRelationshipFromPrompt

This is the main MCP Tool for creating relationships between different content types.

**MCP Tool:** `CreateMixDbRelationshipFromPrompt`

**Required Parameters:**
- `sourceTableName`: The name of the source table/content type
- `destinateTableName`: The name of the destination table/content type  
- `displayName`: Human-readable name for the relationship
- `propertyName`: Property name used when loading related data (optional)
- `relationshipType`: Type of relationship (default: 0 for one-to-many)

**Available Relationship Types:**
- `0`: One-to-Many (most common)
- `1`: Many-to-Many
- `2`: One-to-One

---

## Common Relationship Patterns

### 1. Page-Module Relationships

**Use Case:** Connect pages with their nested modules for complex layouts.

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "Page",
    destinateTableName: "Module", 
    displayName: "Page Modules",
    propertyName: "modules",
    relationshipType: 0
)
```

**When to Use:**
- Homepage with header, hero, features, footer modules
- Landing pages with multiple content sections
- Complex layouts requiring modular content

**Workflow:**
1. Create the page using `CreatePageContent`
2. Create individual modules using `CreateModuleContent`
3. Establish relationship using `CreateMixDbRelationshipFromPrompt`
4. Load related data with `loadNestedData: true`

### 2. Post-Category Relationships

**Use Case:** Organize blog posts and articles by categories or tags.

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "Post",
    destinateTableName: "Category",
    displayName: "Post Categories", 
    propertyName: "categories",
    relationshipType: 0
)
```

**When to Use:**
- Blog systems with categorized content
- News articles with topic classification
- Content management with filtering needs

**Workflow:**
1. Create posts using `CreatePostContent`
2. Create categories using database operations
3. Establish relationship using `CreateMixDbRelationshipFromPrompt`
4. Query with `loadNestedData: true` for categorized content

### 3. Custom Content Relationships

**Use Case:** Link custom database tables created with MCP Tools.

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_appointments",
    destinateTableName: "mix_healthcare_services",
    displayName: "Appointment Services",
    propertyName: "service",
    relationshipType: 0
)
```

**When to Use:**
- Custom business logic requiring data connections
- E-commerce product-category relationships
- User-generated content with associations

---

## Step-by-Step Relationship Creation Workflow

### Step 1: Verify Table/Content Types Exist

Before creating relationships, ensure both source and destination entities exist:

```markdown
// Check existing tables
GetTables()

// Check specific table structure  
GetTableSchema(tableName: "mix_pages")
GetTableSchema(tableName: "mix_modules")

// For content types, verify they exist
ListPageContents()
ListModuleContents()
```

### Step 2: Plan the Relationship

Determine:
- **Source**: What content initiates the relationship?
- **Destination**: What content is being referenced?
- **Type**: One-to-many, many-to-many, or one-to-one?
- **Purpose**: How will this relationship be used in templates?

### Step 3: Create the Relationship

Use the `CreateMixDbRelationshipFromPrompt` MCP Tool:

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "{source_table}",
    destinateTableName: "{destination_table}",
    displayName: "{meaningful_description}",
    propertyName: "{property_for_templates}",
    relationshipType: {0_1_or_2}
)
```

### Step 4: Test the Relationship

Verify the relationship works by loading data:

```markdown
// For pages with modules
GetPageContent(id: {page_id}, loadNestedData: true)

// For posts with categories  
GetPostContent(id: {post_id}, loadNestedData: true)

// For custom relationships
GetListMidxDbData(
    databaseSystemName: "{table_name}",
    queryJson: "[{\"Field\":\"id\",\"Value\":\"{id}\",\"Method\":\"Equal\"}]",
    loadNestedData: true
)
```

### Step 5: Document the Relationship

**CRITICAL:** Always document relationships in your project files:

```markdown
// In project-progress.md
### 2025-01-XX - Relationship Creation
- **Source:** Page content
- **Destination:** Module content  
- **Relationship:** One-to-many (Page → Modules)
- **Purpose:** Enable complex page layouts with nested modules
- **Property Name:** "modules" 
- **Status:** ✅ Complete - Relationship working correctly

// In database-schema.md  
## Relationships
1. mix_page_content.id → mix_module_content (FK relationship)
   - Type: One-to-Many
   - Purpose: Page layout modularity
   - Property: modules
```

---

## Loading Related Data

### Using loadNestedData Parameter

When querying content with relationships, always use `loadNestedData: true`:

```markdown
// Pages with modules
GetPageContent(id: 1, loadNestedData: true)

// Posts with categories
ListPostContents(
    pageIndex: 0,
    pageSize: 10, 
    loadNestedData: true
)

// Custom queries
GetListMidxDbData(
    databaseSystemName: "mix_pages",
    queryJson: "[{\"Field\":\"status\",\"Value\":\"1\",\"Method\":\"Equal\"}]",
    loadNestedData: true
)
```

### Template Usage

In Razor templates, access related data through the property name:

```razor
@model PageContentViewModel

<div class="page-content">
    <h1>@Model.Title</h1>
    
    @if (Model.Modules != null && Model.Modules.Any())
    {
        @foreach (var module in Model.Modules)
        {
            @await Html.PartialAsync($"../Modules/{module.Template.FileName}.cshtml", module)
        }
    }
</div>
```

---

## Common Issues and Troubleshooting

### Relationship Creation Failures

**Issue:** `CreateMixDbRelationshipFromPrompt` fails
**Solutions:**
1. Verify table names exist using `GetTables()`
2. Check table schemas with `GetTableSchema()`
3. Ensure source and destination tables have correct structure
4. Use exact table names (case-sensitive)

### Data Not Loading

**Issue:** Related data doesn't appear when querying
**Solutions:**
1. Ensure `loadNestedData: true` is set
2. Verify the relationship was created successfully
3. Check that related records actually exist
4. Confirm property name matches in templates

### Template Rendering Issues

**Issue:** Related content doesn't render in templates
**Solutions:**
1. Check property name matches relationship definition
2. Verify template paths are correct for related content
3. Ensure related content has proper templates
4. Add null checks in templates

### Performance Considerations

**Issue:** Slow loading with nested data
**Solutions:**
1. Use specific queries instead of loading all data
2. Implement pagination for large relationship sets
3. Consider caching strategies for frequently accessed relationships
4. Use `selectColumns` to limit returned data

---

## Best Practices

### 1. Naming Conventions
- Use descriptive `displayName` values
- Choose meaningful `propertyName` for template usage
- Follow consistent naming patterns across relationships

### 2. Relationship Design
- Plan relationships before implementation
- Consider data access patterns
- Design for template rendering requirements
- Document relationship purposes clearly

### 3. Testing Strategy
- Test relationship creation immediately
- Verify data loading with `loadNestedData: true`
- Test template rendering with sample data
- Validate performance with realistic data volumes

### 4. Documentation Requirements
- Document all relationships in `database-schema.md`
- Update `project-progress.md` with implementation status
- Include relationship purpose and usage examples
- Maintain relationship diagrams for complex systems

---

## Relationship Types Guide

### One-to-Many (relationshipType: 0)
**Most Common Pattern**
- One parent record can have multiple child records
- Examples: Page → Modules, Post → Comments, Category → Posts
- Use when: Parent controls child lifecycle

### Many-to-Many (relationshipType: 1)
**Complex Associations**
- Multiple records can be associated with multiple other records
- Examples: Posts ↔ Tags, Users ↔ Roles, Products ↔ Categories
- Use when: Independent entities need cross-references

### One-to-One (relationshipType: 2)
**Rare but Specific**
- Each record relates to exactly one other record
- Examples: User → Profile, Order → Payment, Page → SEO Data
- Use when: Extending entity data or enforcing unique relationships

---

## Advanced Scenarios

### Hierarchical Content
For parent-child content structures:

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_categories",
    destinateTableName: "mix_categories", 
    displayName: "Category Hierarchy",
    propertyName: "subcategories",
    relationshipType: 0
)
```

### Cross-Content References
For content referencing other content:

```markdown
CreateMixDbRelationshipFromPrompt(
    sourceTableName: "mix_posts",
    destinateTableName: "mix_posts",
    displayName: "Related Posts", 
    propertyName: "relatedPosts",
    relationshipType: 1
)
```

### Multi-Table Workflows
For complex business logic requiring multiple relationships:

1. Create primary relationships first
2. Add secondary relationships
3. Test each relationship independently
4. Validate combined data loading
5. Document the complete relationship network

---

## Success Criteria

A relationship is successfully implemented when:

1. ✅ `CreateMixDbRelationshipFromPrompt` executes without errors
2. ✅ Data loads correctly with `loadNestedData: true`
3. ✅ Templates render related content properly
4. ✅ Relationship is documented in project files
5. ✅ Performance is acceptable for expected data volumes
6. ✅ Error handling works for edge cases

---

This guide ensures AI agents can effectively create and manage database relationships in Mix CMS using MCP Tools, following best practices for maintainable and performant implementations.
