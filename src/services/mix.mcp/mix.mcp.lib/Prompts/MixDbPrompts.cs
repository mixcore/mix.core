using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    /// <summary>
    /// Provides prompts for Mix Database operations
    /// </summary>
    [McpServerPromptType]
    public class MixDbPrompts
    {
        private readonly ILogger<MixDbPrompts> _logger;

        /// <summary>
        /// Initializes a new instance of the MixDbPrompts class
        /// </summary>
        /// <param name="logger">The logger</param>
        public MixDbPrompts(ILogger<MixDbPrompts> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// CreateMixDbData a prompt for generating a database schema description
        /// </summary>
        /// <param name="purpose">Purpose or domain of the database</param>
        /// <param name="requirements">Additional requirements or context</param>
        /// <returns>A chat LLMMessage with the schema generation prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for generating a database schema description")]
        public ChatMessage CreateMixDatabaseDescriptionPrompt(
            [Description("Purpose or domain of the database (e.g., e-commerce, blog, inventory)")] string purpose,
            [Description("Additional requirements or context (optional)")] string requirements = null)
        {
            _logger.LogInformation("Creating schema description prompt for {Purpose}", purpose);

            string prompt = $"Generate a detailed database schema description for a {purpose} application. " +
                "Include field names, data types, and which fields should be required. " +
                "The schema should include all necessary fields to properly model the domain.";

            if (!string.IsNullOrEmpty(requirements))
            {
                prompt += $"\n\nAdditional requirements: {requirements}";
            }

            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// CreateMixDbData a prompt for analyzing existing database schema
        /// </summary>
        /// <param name="schema">JSON representation of the database schema</param>
        /// <returns>A chat LLMMessage with the schema analysis prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for analyzing existing database schema")]
        public ChatMessage CreateMixDatabaseAnalysisPrompt(
            [Description("JSON representation of the database schema")] string schema)
        {
            _logger.LogInformation("Creating schema analysis prompt");

            string prompt = "Analyze the following database schema and provide feedback on its design: " +
                "Are there any missing fields? Are the data types appropriate? " +
                "Are there any potential performance issues or design flaws? " +
                "Suggest improvements if necessary.\n\n" +
                $"MixDbTableName:\n{schema}";

            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// CreateMixDbData a prompt for database migration guidance
        /// </summary>
        /// <param name="currentMixDatabase">Current database schema</param>
        /// <param name="targetMixDatabase">Target database schema</param>
        /// <returns>A chat LLMMessage with the migration guidance prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for database migration guidance")]
        public ChatMessage CreateMigrationGuidancePrompt(
            [Description("Current database schema")] string currentMixDatabase,
            [Description("Target database schema")] string targetMixDatabase)
        {
            _logger.LogInformation("Creating migration guidance prompt");

            string prompt = "Compare the current database schema and the target schema below. " +
                "Generate a migration plan that outlines the steps needed to migrate from the current schema to the target schema. " +
                "Consider data preservation, potential conflicts, and required transformations.\n\n" +
                $"Current MixDbTableName:\n{currentMixDatabase}\n\n" +
                $"Target MixDbTableName:\n{targetMixDatabase}";

            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// CreateMixDbData a prompt for generating sample data for a database
        /// </summary>
        /// <param name="schema">Database schema</param>
        /// <param name="recordCount">Number of sample records to generate</param>
        /// <returns>A chat LLMMessage with the sample data generation prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for generating sample data for a database")]
        public ChatMessage CreateSampleDataPrompt(
            [Description("Database schema")] string schema,
            [Description("Number of sample records to generate")] int recordCount = 5)
        {
            _logger.LogInformation("Creating sample data prompt for {RecordCount} records", recordCount);

            string prompt = $"Generate {recordCount} realistic sample records for a database with the following schema. " +
                "The data should be varied and realistic for the domain. " +
                "Format the results as a valid JSON array of objects, with each object representing one record.\n\n" +
                $"MixDbTableName:\n{schema}";

            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// CreateMixDbData a prompt for optimizing database queries
        /// </summary>
        /// <param name="currentQuery">Current database query</param>
        /// <param name="tableStructure">Structure of relevant tables</param>
        /// <returns>A chat LLMMessage with the query optimization prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for optimizing database queries")]
        public ChatMessage CreateQueryOptimizationPrompt(
            [Description("Current database query")] string currentQuery,
            [Description("Structure of relevant tables")] string tableStructure)
        {
            _logger.LogInformation("Creating query optimization prompt");

            string prompt = "Analyze and optimize the following database query for better performance. " +
                "Suggest improvements to the query structure, indexing, or other optimizations. " +
                "Explain the reasoning behind your suggestions.\n\n" +
                $"Table Structure:\n{tableStructure}\n\n" +
                $"Current Query:\n{currentQuery}";

            return new ChatMessage(ChatRole.User, prompt);
        }
    }
}