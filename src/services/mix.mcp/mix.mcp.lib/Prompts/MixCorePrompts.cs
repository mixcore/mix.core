using Microsoft.Extensions.AI;
using Mix.MCP.Lib.Services.LLM;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    [McpServerPromptType]
    public class MixCorePrompts
    {
        [McpServerPrompt, Description("CreateMixDbData a new database schema with columns")]
        public static Microsoft.Extensions.AI.ChatMessage CreateDatabaseSchema(
            [Description("Name of the database to create")] string databaseName,
            [Description("Purpose and description of this database")] string purpose)
        {
            return new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, $@"Please help me design a database schema for a {databaseName} database. 

Purpose: {purpose}

I need you to define:
1. Appropriate columns with their data types
2. Required fields
3. Unique constraints
4. DefaultBaseUrl values where appropriate

For each column, define:
- Column name
- Data type (String, Integer, Double, Boolean, DateTime, etc.)
- Whether it's required
- DefaultBaseUrl value (if any)
- Whether it should be unique
- Maximum length (for string columns)

Please format your response so I can easily use it with MixCore CMS's database creation tools. For each column, provide a clear explanation of what it stores and why it's needed.");
        }

        [McpServerPrompt, Description("Design database relationships for existing schemas")]
        public static Microsoft.Extensions.AI.ChatMessage DesignDatabaseRelationships(
            [Description("Names of the databases to create relationships between")] string databaseNames,
            [Description("Purpose of these relationships")] string purpose)
        {
            return new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, $@"I need to design relationships between these databases: {databaseNames}

Purpose: {purpose}

Please analyze these databases and recommend:
1. Which tables should be related
2. What type of relationships should be created (one-to-one, one-to-many, many-to-many)
3. Which columns should be used for the relationships
4. How to implement these relationships in MixCore CMS

For each relationship, explain:
- The parent database and key field
- The child database and key field
- The type of relationship
- Why this relationship is needed
- Any considerations or constraints

Format your response so I can easily implement these relationships using MixCore's relationship creation tools.");
        }

        [McpServerPrompt, Description("CreateMixDbData a database query for specific requirements")]
        public static Microsoft.Extensions.AI.ChatMessage CreateDatabaseQuery(
            [Description("Name of the database to query")] string databaseName,
            [Description("Description of what you want to query")] string queryDescription)
        {
            return new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, $@"I need to create a database query for the '{databaseName}' database.

Query requirements: {queryDescription}

Please help me design:
1. The appropriate filter criteria
2. Sorting requirements
3. Any pagination needs
4. The specific columns I should include in the results

Format your response as both:
1. A natural explanation of the query approach
2. A structured JSON format that I can use with MixCore's SearchData tool

Include examples of how to handle common scenarios like:
- Text searching with wildcards
- Date range filtering
- Numeric comparisons
- Complex logical conditions (AND/OR)");
        }

        [McpServerPrompt, Description("Design a multi-tenant data structure")]
        public static Microsoft.Extensions.AI.ChatMessage DesignMultiTenantStructure(
            [Description("Types of tenants needed")] string tenantTypes,
            [Description("Description of data isolation requirements")] string isolationRequirements)
        {
            return new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, $@"I need to design a multi-tenant data structure in MixCore CMS.

Tenant types: {tenantTypes}
Data isolation requirements: {isolationRequirements}

Please help me design:
1. The tenant structure (how tenants should be organized)
2. Which databases should be shared across tenants vs. tenant-specific
3. How to handle tenant-specific customizations
4. The security and isolation model
5. Considerations for scaling and performance

For each recommendation, explain:
- The approach and rationale
- How to implement it in MixCore CMS
- Tradeoffs and alternatives
- Best practices for security and data integrity

Include a section on how to manage relationships between tenant-specific and shared data.");
        }

        [McpServerPrompt, Description("Generate sample data for testing a database")]
        public static Microsoft.Extensions.AI.ChatMessage GenerateSampleData(
            [Description("Name of the database to generate data for")] string databaseName,
            [Description("Number of sample records to generate")] int recordCount)
        {
            return new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, $@"I need to generate {recordCount} sample records for testing the '{databaseName}' database in MixCore CMS.

Please create realistic test data that:
1. Follows the schema constraints (required fields, data types, etc.)
2. Includes a variety of scenarios and edge cases
3. Has realistic values that would be encountered in production

Format the data as:
1. A clear explanation of the generated data patterns
2. JSON objects that can be directly used with MixCore's SaveData tool

Ensure the sample data includes:
- A variety of text values with different lengths and patterns
- Realistic dates and times
- Numeric values with appropriate ranges
- Boolean fields with a mix of true/false values
- Any special data patterns specific to the database purpose");
        }
    }
}