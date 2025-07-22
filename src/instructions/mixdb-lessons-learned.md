# MixDb Database Creation Best Practices

## Key Lessons Learned

1. **Schema Design**
   - Always define clear relationships between tables upfront
   - Use proper data types for each field (string, integer, datetime, etc.)
   - Document all fields with descriptions in database-schema.md

2. **Relationship Management**
   - Verify table and column names before creating relationships
   - Use GetTableSchema to confirm schema details
   - Document relationships in both database-schema.md and project-progress.md

3. **Data Population**
   - Use CreateManyMixDbData for bulk inserts
   - Include sample data in database-schema.md for reference
   - Use full public URLs for any image references

4. **Error Handling**
   - Check for duplicate template creation
   - Verify tool responses before proceeding
   - Document all operations in project-progress.md

## Recommended Prompt Structure

When requesting database creation, include:
1. Table name (prefix with mix_)
2. Field names and types
3. Required vs optional fields
4. Relationship requirements
5. Sample data needs

Example:
"Create a mix_products table with:
- name (string, required)
- price (double, required)
- description (text, optional)
- category_id (integer, required - will relate to mix_categories)
Sample data: 3 sample products"
