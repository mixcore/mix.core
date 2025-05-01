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

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tool for creating Mix Databases from prompt descriptions
    /// </summary>
    [McpServerToolType]
    public class MixDatabasePromptTool
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IMixdbStructure _mixDbStructureService;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCacheService _cacheService;
        private readonly DatabaseService _databaseService;
        private readonly ILogger<MixDatabasePromptTool> _logger;
        private readonly ILlmServiceFactory _llmServiceFactory;

        /// <summary>
        /// Initializes a new instance of the MixDatabasePromptTool class
        /// </summary>
        public MixDatabasePromptTool(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixdbStructure mixDbService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            DatabaseService databaseService,
            ILlmServiceFactory llmServiceFactory,
            ILogger<MixDatabasePromptTool> logger)
        {
            _cmsUow = cmsUow;
            _mixDbStructureService = mixDbService;
            _memoryCache = memoryCache;
            _cacheService = cacheService;
            _databaseService = databaseService;
            _logger = logger;
            _llmServiceFactory = llmServiceFactory;
        }

        /// <summary>
        /// Create a Mix Database with columns based on a prompt description
        /// </summary>
        [McpServerTool, Description("Create a Mix Database with columns based on a prompt description")]
        public async Task<string> CreateDatabaseFromPrompt(
            [Description("Display name for the database")] string displayName,
            [Description("Description of the database schema in natural language (e.g., 'Create a Product table with name, price, and description')")] string schemaDescription,
            [Description("Naming convention to use (SnakeCase, CamelCase, KebabCase, PascalCase)")] MixDatabaseNamingConvention namingConvention = MixDatabaseNamingConvention.SnakeCase,
            [Description("Type of database (Service, GuidService, AdditionalData, GuidAdditionalData)")] MixDatabaseType type = MixDatabaseType.Service,
            [Description("Mix Database Context ID (default: 1)")] int mixDatabaseContextId = 1,
            [Description("LLM service type to use for schema parsing (OpenAI, DeepSeek, LmStudio)")] LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            [Description("LLM model to use (e.g., gpt-4, deepseek-chat)")] string llmModel = "mathstral-7b-v0.1")
        {
            try
            {
                _logger.LogInformation("Creating database {DisplayName} with schema description: {SchemaDescription}", 
                    displayName, schemaDescription);
                
                // Parse the schema description using LLM to extract columns
                var columns = await ParseSchemaDescriptionWithLLM(schemaDescription, llmServiceType, llmModel);
                
                if (columns.Count == 0)
                {
                    return "Could not determine columns from the schema description. Please provide more details.";
                }

                // Generate system name based on display name and naming convention
                string systemName = GenerateSystemName(displayName, namingConvention);
                
                // Check if database already exists
                var existingDb = await _cmsUow.DbContext.MixDatabase
                    .FirstOrDefaultAsync(db => db.SystemName == systemName && !db.IsDeleted);

                if (existingDb != null)
                {
                    return $"A database with the system name '{systemName}' already exists";
                }

                // Create database
                var dbViewModel = new MixDbDatabaseViewModel(_cmsUow)
                {
                    TenantId = 1,
                    DisplayName = displayName,
                    SystemName = systemName,
                    Type = type,
                    Description = schemaDescription,
                    NamingConvention = namingConvention,
                    MixDatabaseContextId = mixDatabaseContextId
                };

                
                // Add custom columns from the schema description
                foreach (var column in columns)
                {
                    AddColumnToViewModel(dbViewModel, column.Name, column.DataType, column.IsRequired, column.Description);
                }

                // Save database
                var result = await dbViewModel.SaveAsync();
                if (result <= 0)
                {
                    return $"Failed to create database: {systemName}";
                }
                
                // Migrate the database schema
                await _mixDbStructureService.MigrateDatabase(systemName);

                return JsonSerializer.Serialize(new
                {
                    Success = true,
                    Message = $"Database '{displayName}' created successfully with {columns.Count} custom columns",
                    SystemName = systemName,
                    Columns = columns.Select(c => new { Name = c.Name, Type = c.DataType.ToString(), IsRequired = c.IsRequired }).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database from prompt: {Message}", ex.Message);
                return $"Error creating database: {ex.Message}";
            }
        }

        /// <summary>
        /// Parse the schema description using LLM to extract column information
        /// </summary>
        private async Task<List<ColumnInfo>> ParseSchemaDescriptionWithLLM(
            string schemaDescription, 
            LLMServiceType serviceType, 
            string model)
        {
            var columns = new List<ColumnInfo>();

            try
            {
                // Create LLM service
                var llmService = _llmServiceFactory.CreateService(serviceType);

                // Create prompt for the LLM
                string prompt = $@"
Please analyze the following database schema description and extract column information in JSON format.
For each column, identify:
1. name (field name in snake_case)
2. dataType (one of: String, Text, Integer, Double, Boolean, DateTime, Reference, Upload, Json)
3. isRequired (true/false)
4. description (brief purpose of the field)

Schema description: ""{schemaDescription}""

Respond ONLY with a valid JSON array of column objects, for example:
[
  {{
    ""name"": ""product_name"",
    ""dataType"": ""String"",
    ""isRequired"": true,
    ""description"": ""Name of the product""
  }},
  {{
    ""name"": ""price"",
    ""dataType"": ""Double"",
    ""isRequired"": true,
    ""description"": ""Product price in dollars""
  }}
]

Include only custom columns (no need for standard fields like id, created_by, etc.).
Infer missing information from context where possible.";

                // Get response from LLM
                var response = await llmService.ChatAsync(prompt, model, 0.7);
                
                if (string.IsNullOrEmpty(response?.choices[0]?.Text))
                {
                    _logger.LogWarning("LLM service returned empty response");
                    return await FallbackToRegexParsing(schemaDescription);
                }

                // Extract JSON array from response (handling possible text before/after JSON)
                string jsonContent = ExtractJsonArrayFromText(response.choices[0].Text);
                
                if (string.IsNullOrEmpty(jsonContent))
                {
                    _logger.LogWarning("Could not extract valid JSON from LLM response: {Response}", response.choices[0]?.Text);
                    return await FallbackToRegexParsing(schemaDescription);
                }

                // Parse JSON content
                try
                {
                    var columnArray = JsonSerializer.Deserialize<JsonElement>(jsonContent);
                    
                    if (columnArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var columnElement in columnArray.EnumerateArray())
                        {
                            var column = new ColumnInfo
                            {
                                Name = GetStringPropertyOrDefault(columnElement, "name", ""),
                                IsRequired = GetBoolPropertyOrDefault(columnElement, "isRequired", false),
                                Description = GetStringPropertyOrDefault(columnElement, "description", null)
                            };

                            // Parse data type
                            string dataTypeStr = GetStringPropertyOrDefault(columnElement, "dataType", "String");
                            column.DataType = MapStringToDataType(dataTypeStr);

                            // Add only if name is valid
                            if (!string.IsNullOrWhiteSpace(column.Name))
                            {
                                columns.Add(column);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing LLM response as JSON: {Response}", jsonContent);
                    return await FallbackToRegexParsing(schemaDescription);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using LLM to parse schema description: {Message}", ex.Message);
                return await FallbackToRegexParsing(schemaDescription);
            }

            // If no columns were extracted, fall back to regex parsing
            if (columns.Count == 0)
            {
                _logger.LogWarning("No columns extracted from LLM response, falling back to regex parsing");
                return await FallbackToRegexParsing(schemaDescription);
            }

            return columns;
        }

        /// <summary>
        /// Extract JSON array from text that may contain explanatory text before/after the JSON
        /// </summary>
        private string ExtractJsonArrayFromText(string text)
        {
            // Try to find the first '[' and last ']' for a JSON array
            int startIndex = text.IndexOf('[');
            int endIndex = text.LastIndexOf(']');
            
            if (startIndex >= 0 && endIndex > startIndex)
            {
                return text.Substring(startIndex, endIndex - startIndex + 1);
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Get string property from JsonElement with default value if not found
        /// </summary>
        private string GetStringPropertyOrDefault(JsonElement element, string propertyName, string defaultValue)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property) && 
                property.ValueKind == JsonValueKind.String)
            {
                return property.GetString() ?? defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// Get boolean property from JsonElement with default value if not found
        /// </summary>
        private bool GetBoolPropertyOrDefault(JsonElement element, string propertyName, bool defaultValue)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property) && 
                property.ValueKind == JsonValueKind.True || property.ValueKind == JsonValueKind.False)
            {
                return property.GetBoolean();
            }
            return defaultValue;
        }

        /// <summary>
        /// Fallback to regex parsing when LLM parsing fails
        /// </summary>
        private Task<List<ColumnInfo>> FallbackToRegexParsing(string schemaDescription)
        {
            _logger.LogInformation("Falling back to regex parsing for schema description");
            return Task.FromResult(ParseSchemaDescriptionWithRegex(schemaDescription));
        }

        /// <summary>
        /// Parse the schema description to extract column information using regex
        /// </summary>
        private List<ColumnInfo> ParseSchemaDescriptionWithRegex(string schemaDescription)
        {
            var columns = new List<ColumnInfo>();
            
            // Common field patterns to look for
            var fieldPatterns = new List<(string Pattern, MixDataType DataType)>
            {
                (@"(?:id|identifier|key)\b", MixDataType.Integer),
                (@"(?:name|title|label)\b", MixDataType.String),
                (@"(?:price|cost|amount|fee)\b", MixDataType.Double),
                (@"(?:date|time|created|modified|updated)\b", MixDataType.DateTime),
                (@"(?:description|content|text|details)\b", MixDataType.Text),
                (@"(?:is|has|enable|active|status)\b", MixDataType.Boolean),
                (@"(?:image|photo|picture|avatar)\b", MixDataType.String),
                (@"(?:email|mail)\b", MixDataType.String),
                (@"(?:phone|mobile|telephone)\b", MixDataType.String),
                (@"(?:address|location)\b", MixDataType.String),
                (@"(?:quantity|count|number)\b", MixDataType.Integer),
                (@"(?:url|link|website)\b", MixDataType.String)
            };

            // Extract words that might be field names
            var words = Regex.Matches(schemaDescription, @"\b[a-zA-Z_]+\b")
                .Cast<Match>()
                .Select(m => m.Value.ToLower())
                .Distinct()
                .ToList();

            // Look for field name patterns
            foreach (var word in words)
            {
                foreach (var (pattern, dataType) in fieldPatterns)
                {
                    if (Regex.IsMatch(word, pattern, RegexOptions.IgnoreCase))
                    {
                        bool isRequired = schemaDescription.Contains($"required {word}") || 
                                        schemaDescription.Contains($"{word} required") ||
                                        schemaDescription.Contains($"mandatory {word}") ||
                                        schemaDescription.Contains($"{word} mandatory");

                        // Don't add duplicate columns
                        if (!columns.Any(c => c.Name.Equals(word, StringComparison.OrdinalIgnoreCase)))
                        {
                            columns.Add(new ColumnInfo
                            {
                                Name = word,
                                DataType = dataType,
                                IsRequired = isRequired,
                                Description = null // No detailed description available from prompt
                            });
                        }
                        break;
                    }
                }
            }

            // Look for specific datatype mentions
            var datatypePatterns = new List<(string Pattern, string FieldGroup, string TypeGroup, MixDataType DefaultType)>
            {
                (@"(\w+)\s+(?:as|is|of type)\s+(string|text|varchar|nvarchar)", "field", "type", MixDataType.String),
                (@"(\w+)\s+(?:as|is|of type)\s+(int|integer|number)", "field", "type", MixDataType.Integer),
                (@"(\w+)\s+(?:as|is|of type)\s+(float|double|decimal|money)", "field", "type", MixDataType.Double),
                (@"(\w+)\s+(?:as|is|of type)\s+(bool|boolean|bit)", "field", "type", MixDataType.Boolean),
                (@"(\w+)\s+(?:as|is|of type)\s+(date|datetime|timestamp)", "field", "type", MixDataType.DateTime),
                (@"(string|text|varchar|nvarchar)\s+(?:field|column|type)\s+(\w+)", "type", "field", MixDataType.String),
                (@"(int|integer|number)\s+(?:field|column|type)\s+(\w+)", "type", "field", MixDataType.Integer),
                (@"(float|double|decimal|money)\s+(?:field|column|type)\s+(\w+)", "type", "field", MixDataType.Double),
                (@"(bool|boolean|bit)\s+(?:field|column|type)\s+(\w+)", "type", "field", MixDataType.Boolean),
                (@"(date|datetime|timestamp)\s+(?:field|column|type)\s+(\w+)", "type", "field", MixDataType.DateTime)
            };

            foreach (var (pattern, fieldGroup, typeGroup, defaultType) in datatypePatterns)
            {
                var matches = Regex.Matches(schemaDescription, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    string fieldName;
                    MixDataType dataType = defaultType;

                    if (fieldGroup == "field")
                    {
                        fieldName = match.Groups[1].Value.ToLower();
                        string typeName = match.Groups[2].Value.ToLower();
                        dataType = MapStringToDataType(typeName);
                    }
                    else
                    {
                        fieldName = match.Groups[2].Value.ToLower();
                        string typeName = match.Groups[1].Value.ToLower();
                        dataType = MapStringToDataType(typeName);
                    }

                    bool isRequired = schemaDescription.Contains($"required {fieldName}") || 
                                    schemaDescription.Contains($"{fieldName} required") ||
                                    schemaDescription.Contains($"mandatory {fieldName}") ||
                                    schemaDescription.Contains($"{fieldName} mandatory");

                    // Don't add duplicate columns
                    if (!columns.Any(c => c.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        columns.Add(new ColumnInfo
                        {
                            Name = fieldName,
                            DataType = dataType,
                            IsRequired = isRequired,
                            Description = null
                        });
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// Convert string type name to MixDataType
        /// </summary>
        private MixDataType MapStringToDataType(string typeName)
        {
            return typeName?.ToLower() switch
            {
                "string" or "varchar" or "nvarchar" => MixDataType.String,
                "text" or "longtext" or "content" => MixDataType.Text,
                "int" or "integer" or "number" => MixDataType.Integer,
                "float" or "double" or "decimal" or "money" => MixDataType.Double,
                "bool" or "boolean" or "bit" => MixDataType.Boolean,
                "date" or "datetime" or "timestamp" => MixDataType.DateTime,
                "ref" or "reference" or "reference_id" => MixDataType.Reference,
                "upload" or "file" or "image" => MixDataType.Upload,
                "json" => MixDataType.Json,
                _ => MixDataType.String
            };
        }

        /// <summary>
        /// Generate system name based on display name and naming convention
        /// </summary>
        private string GenerateSystemName(string displayName, MixDatabaseNamingConvention namingConvention)
        {
            string prefix = "mix_";
            string name = displayName
                .ToLower()
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace(".", "_")
                .Replace(",", "_");

            name = Regex.Replace(name, @"[^a-z0-9_]", "").ToSEOString('_');
            return prefix + name;
        }

        /// <summary>
        /// Add a column to the database view model
        /// </summary>
        private void AddColumnToViewModel(
            MixDbDatabaseViewModel db, 
            string name, 
            MixDataType dataType, 
            bool isRequired, 
            string description,
            string defaultValue = null)
        {
            string displayName = FormatDisplayName(name);
            string systemName = name.ToSEOString('_');

            db.Columns.Add(new MixdbDatabaseColumnViewModel(_cmsUow)
            {
                MixDatabaseId = db.Id,
                MixDatabaseName = db.SystemName,
                DataType = dataType,
                DisplayName = displayName,
                SystemName = systemName,
                DefaultValue = defaultValue,
                ColumnConfigurations = new Mix.Shared.Models.ColumnConfigurations
                {
                    IsRequire = isRequired,
                    IsEncrypt = false,
                    IsUnique = name.Equals("id", StringComparison.OrdinalIgnoreCase)
                }
            });
        }

        /// <summary>
        /// Format name as display name (Title Case)
        /// </summary>
        private string FormatDisplayName(string name)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                name.Replace("_", " ").Replace("-", " ")
            );
        }

        /// <summary>
        /// Column information structure
        /// </summary>
        private class ColumnInfo
        {
            public string Name { get; set; }
            public MixDataType DataType { get; set; }
            public bool IsRequired { get; set; }
            public string Description { get; set; }
        }
    }
}