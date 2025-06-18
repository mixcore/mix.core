using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.ViewModels;
using Mix.MCP.Lib.Services.LLM;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.Service.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Mix.Heart.Extensions;
using Mix.Mixdb.Services;
using Mix.MCP.Lib.Helpers;
using Mix.MCP.Lib.Models;
using System.Threading;
using Microsoft.AspNetCore.Http.Timeouts;
using ModelContextProtocol;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tool for creating Mix Databases from prompt descriptions
    /// </summary>
    [McpServerToolType]
    public class MixDbPromptTool : BaseMcpTool
    {
        private readonly IMixdbStructure _mixDbStructureService;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCacheService _cacheService;
        private readonly DatabaseService _databaseService;
        private readonly ILlmServiceFactory _llmServiceFactory;
        private readonly MixDbHelper _databaseHelper;
        private readonly MixDbSchemaParser _schemaParser;

        /// <summary>
        /// Initializes a new instance of the MixDbPromptTool class
        /// </summary>
        public MixDbPromptTool(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixdbStructure mixDbService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            DatabaseService databaseService,
            ILlmServiceFactory llmServiceFactory,
            ILogger<MixDbPromptTool> logger)
            : base(cmsUow, logger)
        {
            _mixDbStructureService = mixDbService;
            _memoryCache = memoryCache;
            _cacheService = cacheService;
            _databaseService = databaseService;
            _llmServiceFactory = llmServiceFactory;
            _databaseHelper = new MixDbHelper(cmsUow, mixDbService, logger);
            _schemaParser = new MixDbSchemaParser(llmServiceFactory, logger);
        }

        /// <summary>
        /// CreateMixDbData a Mix Database with columns based on a prompt description
        /// </summary>
        [McpServerTool, Description("Create a Mix Database with columns based on a prompt description"), DisableRequestTimeout]
        public async Task<string> CreateDatabaseFromPrompt(
            [Description("Display name for the database")] string displayName,
            [Description("Description of the database schema in natural language (e.g., 'CreateMixDbData a Product table with name, price, and description')")] string schemaDescription,
            [Description("Mix Database Context ID (default: 1)")] int mixDatabaseContextId = 1,
            [Description("LLM service type to use for schema parsing (OpenAI, DeepSeek, LmStudio)")] LLMServiceType llmServiceType = LLMServiceType.DeepSeek,
            [Description("LLM model to use (e.g., gpt-4, deepseek-chat, mathstral-7b-v0.1)")] string llmModel = "deepseek-chat",
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new McpException("Display name cannot be empty.");
            if (string.IsNullOrWhiteSpace(schemaDescription))
                throw new McpException("Schema description cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating database {DisplayName} with schema description: {SchemaDescription}",
                    displayName, schemaDescription);
                var columns = await _schemaParser.ParseSchemaDescriptionWithLLM(schemaDescription, llmServiceType, llmModel);
                if (columns.Count == 0)
                    throw new McpException("Could not determine columns from the schema description. Please provide more details about the fields needed for your database.");
                string systemName = MixDbHelper.GenerateSystemName(displayName, MixDatabaseNamingConvention.SnakeCase);
                bool databaseExists = await _databaseHelper.DatabaseExists(systemName);
                if (databaseExists)
                    throw new McpException($"A database with the system name '{systemName}' already exists. Please use a different name or delete the existing database first.");
                var database = await _databaseHelper.CreateDatabase(
                    displayName,
                    systemName,
                    columns,
                    schemaDescription,
                    MixDatabaseNamingConvention.SnakeCase,
                    MixDbTableType.Service,
                    mixDatabaseContextId);
                if (database == null)
                    throw new McpException($"Failed to create database: {systemName}. Please check the logs for more details.");
                return JsonSerializer.Serialize(new
                {
                    Success = true,
                    Message = $"Database '{displayName}' created successfully with {columns.Count} custom columns",
                    SystemName = systemName,
                    Columns = columns.Select(c => new
                    {
                        Name = c.Name,
                        Type = c.DataType.ToString(),
                        IsRequired = c.IsRequired,
                    }).ToList()
                });
            }, "CreateDatabaseFromPrompt");
        }
        
        /// <summary>
        /// CreateMixDbData a Mix Database with columns based on a prompt description
        /// </summary>
        [McpServerTool, Description("Create a Mix Database with columns based on a prompt description"), DisableRequestTimeout]
        public async Task<string> CreateMixDbRelationshipFromPrompt(
            [Description("Display name for the source table")] string sourceTableName,
            [Description("Display name for the destinate table")] string destinateTableName,
            [Description("Display name when load related data")] string displayName,
            [Description("Property Name when load related data")] string? propertyName = null,
            [Description("Display name for the source column")] string? sourceColumnName = null,
            [Description("Display name for the destinate column")] string? destinateColumnName = null,
            [Description("Relationship type")] MixDbTableRelationshipType relationshipType = MixDbTableRelationshipType.OneToMany,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sourceTableName))
                throw new McpException("source table name cannot be empty.");
            if (string.IsNullOrWhiteSpace(destinateTableName))
                throw new McpException("destinate table name cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating relationship {sourceTableName} with schema description: {destinateTableName}",
                    sourceTableName, destinateTableName);
                await _databaseHelper.CreateRelationship(
                    sourceTableName,
                    destinateTableName,
                    displayName,
                    propertyName,
                    sourceColumnName,
                    destinateColumnName,
                    relationshipType);
                return JsonSerializer.Serialize(new
                {
                    Success = true,
                    Message = $"Relationship '{sourceTableName}' and {destinateTableName} created successfully"
                });
            }, "CreateDatabaseFromPrompt");
        }

        /// <summary>
        /// Add multiple columns to an existing Mix Database using schema text
        /// </summary>
        [McpServerTool, Description("Add multiple columns to an existing Mix Database using schema text")]
        public async Task<string> AddColumnToDatabase(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Schema description for the columns (e.g., 'Add a price column that stores decimal values and is required, and a description column for long text')")] string schemaText,
            [Description("LLM service type to use for schema parsing (OpenAI, DeepSeek, LmStudio)")] LLMServiceType llmServiceType = LLMServiceType.DeepSeek,
            [Description("LLM model to use (e.g., gpt-4, deepseek-chat, mathstral-7b-v0.1)")] string llmModel = "deepseek-chat",
            [Description("Timeout in seconds for LLM operations (default: 120)")] int timeoutSeconds = 120,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                throw new McpException("Database system name cannot be empty.");
            if (string.IsNullOrWhiteSpace(schemaText))
                throw new McpException("Schema text cannot be empty.");
            if (timeoutSeconds <= 0)
                throw new McpException("Timeout must be greater than 0 seconds.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Adding columns to database {DatabaseName} with schema: {SchemaText}, timeout: {Timeout}s",
                    databaseSystemName, schemaText, timeoutSeconds);
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    throw new McpException($"Database with system name '{databaseSystemName}' not found.");
                var llmService = _llmServiceFactory.CreateService(llmServiceType);
                llmService.SetTimeout(TimeSpan.FromSeconds(timeoutSeconds));
                var columns = await _schemaParser.ParseSchemaDescriptionWithLLM(schemaText, llmServiceType, llmModel, ct);
                if (columns.Count == 0)
                    throw new McpException("Could not determine column information from the schema text. Please provide more details.");
                var results = new List<object>();
                var successCount = 0;
                foreach (var column in columns)
                {
                    bool success = await _databaseHelper.AddColumn(database, column);
                    results.Add(new
                    {
                        Name = column.Name,
                        DisplayName = MixDbHelper.FormatDisplayName(column.Name),
                        DataType = column.DataType.ToString(),
                        IsRequired = column.IsRequired,
                        Success = success
                    });
                    if (success) successCount++;
                }
                return JsonSerializer.Serialize(new
                {
                    Success = successCount > 0,
                    Message = $"Added {successCount} of {columns.Count} columns to database '{databaseSystemName}'",
                    Columns = results
                });
            }, "AddColumnToDatabase", timeoutSeconds);
        }

        /// <summary>
        /// UpdateMixDbData multiple columns in a Mix Database using schema text
        /// </summary>
        [McpServerTool, Description("UpdateMixDbData multiple columns in a Mix Database using schema text")]
        public async Task<string> UpdateDatabaseColumn(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Schema description for updating columns (e.g., 'Change the product_price column to be non-required and set a default value of 0.00, and rename the description column to product_details')")] string schemaText,
            [Description("LLM service type to use for schema parsing (OpenAI, DeepSeek, LmStudio)")] LLMServiceType llmServiceType = LLMServiceType.DeepSeek,
            [Description("LLM model to use (e.g., gpt-4, deepseek-chat, mathstral-7b-v0.1)")] string llmModel = "deepseek-chat",
            [Description("Timeout in seconds for LLM operations (default: 120)")] int timeoutSeconds = 120,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                throw new McpException("Error: Database system name cannot be empty.");
            if (string.IsNullOrWhiteSpace(schemaText))
                throw new McpException("Error: Schema text cannot be empty.");
            if (timeoutSeconds <= 0)
                throw new McpException("Error: Timeout must be greater than 0 seconds.");
            try
            {
                return await ExecuteWithExceptionHandlingAsync(async (ct) =>
                {
                    _logger.LogInformation("Updating columns in database {DatabaseName} with schema: {SchemaText}, timeout: {Timeout}s",
                        databaseSystemName, schemaText, timeoutSeconds);
                    var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                    if (database == null)
                        throw new McpException($"Error: Database with system name '{databaseSystemName}' not found.");
                    var llmService = _llmServiceFactory.CreateService(llmServiceType);
                    llmService.SetTimeout(TimeSpan.FromSeconds(timeoutSeconds));
                    var columns = await _schemaParser.ParseSchemaDescriptionWithLLM(schemaText, llmServiceType, llmModel, ct);
                    if (columns.Count == 0)
                        throw new McpException("Error: Could not determine column information from the schema text. Please provide more details.");
                    var results = new List<object>();
                    var successCount = 0;
                    foreach (var column in columns)
                    {
                        bool success = await _databaseHelper.UpdateColumn(database, column.Name);
                        results.Add(new
                        {
                            Name = column.Name,
                            DisplayName = MixDbHelper.FormatDisplayName(column.Name),
                            DataType = column.DataType.ToString(),
                            IsRequired = column.IsRequired,
                            Success = success
                        });
                        if (success) successCount++;
                    }
                    return JsonSerializer.Serialize(new
                    {
                        Success = successCount > 0,
                        Message = $"Updated {successCount} of {columns.Count} columns in database '{databaseSystemName}'",
                        Columns = results
                    });
                }, "UpdateDatabaseColumn", timeoutSeconds);
            }
            catch (McpException ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// DeleteMixDbData multiple columns from a Mix Database using schema text
        /// </summary>
        [McpServerTool, Description("DeleteMixDbData multiple columns from a Mix Database using schema text")]
        public async Task<string> DeleteDatabaseColumn(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Schema description indicating columns to delete (e.g., 'Remove the product_code column and the manufacturer column')")] string schemaText,
            [Description("LLM service type to use for schema parsing (OpenAI, DeepSeek, LmStudio)")] LLMServiceType llmServiceType = LLMServiceType.DeepSeek,
            [Description("LLM model to use (e.g., gpt-4, deepseek-chat, mathstral-7b-v0.1)")] string llmModel = "deepseek-chat",
            [Description("Timeout in seconds for LLM operations (default: 120)")] int timeoutSeconds = 120,
            [Description("Confirm drop with 'YES' (case sensitive)")] string confirmDropColumn = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                throw new McpException("Database system name cannot be empty.");
            if (string.IsNullOrWhiteSpace(schemaText))
                throw new McpException("Schema text cannot be empty.");
            if (timeoutSeconds <= 0)
                throw new McpException("Timeout must be greater than 0 seconds.");
            if (confirmDropColumn != "YES")
                throw new McpException("To delete columns, you must confirm by setting confirmDropColumn to 'YES' (case sensitive).");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting columns from database {DatabaseName} with schema: {SchemaText}, timeout: {Timeout}s",
                    databaseSystemName, schemaText, timeoutSeconds);
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    throw new McpException($"Database with system name '{databaseSystemName}' not found.");
                var llmService = _llmServiceFactory.CreateService(llmServiceType);
                llmService.SetTimeout(TimeSpan.FromSeconds(timeoutSeconds));
                var columns = await _schemaParser.ParseSchemaDescriptionWithLLM(schemaText, llmServiceType, llmModel, ct);
                if (columns.Count == 0)
                    throw new McpException("Could not determine column information from the schema text. Please provide more details.");
                var results = new List<object>();
                var successCount = 0;
                foreach (var column in columns)
                {
                    bool success = await _databaseHelper.DeleteColumn(database, column.Name);
                    results.Add(new
                    {
                        Name = column.Name,
                        DisplayName = MixDbHelper.FormatDisplayName(column.Name),
                        DataType = column.DataType.ToString(),
                        IsRequired = column.IsRequired,
                        Success = success
                    });
                    if (success) successCount++;
                }
                return JsonSerializer.Serialize(new
                {
                    Success = successCount > 0,
                    Message = $"Deleted {successCount} of {columns.Count} columns from database '{databaseSystemName}'",
                    Columns = results
                });
            }, "DeleteDatabaseColumn", timeoutSeconds);
        }
    }
}