using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.ViewModels;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.Service.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
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
        private readonly IMixdbStructure _mixDbService;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCacheService _cacheService;
        private readonly DatabaseService _databaseService;
        private readonly ILogger<MixDatabasePromptTool> _logger;

        /// <summary>
        /// Initializes a new instance of the MixDatabasePromptTool class
        /// </summary>
        public MixDatabasePromptTool(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixdbStructure mixDbService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            DatabaseService databaseService,
            ILogger<MixDatabasePromptTool> logger)
        {
            _cmsUow = cmsUow;
            _mixDbService = mixDbService;
            _memoryCache = memoryCache;
            _cacheService = cacheService;
            _databaseService = databaseService;
            _logger = logger;
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
            [Description("Mix Database Context ID (default: 1)")] int mixDatabaseContextId = 1)
        {
            try
            {
                _logger.LogInformation("Creating database {DisplayName} with schema description: {SchemaDescription}", 
                    displayName, schemaDescription);
                
                // Parse the schema description to extract columns
                var columns = ParseSchemaDescription(schemaDescription);
                
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

                // Add standard columns
                AddStandardColumns(dbViewModel);
                
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
                await _mixDbService.MigrateDatabase(systemName);

                return JsonSerializer.Serialize(new
                {
                    Success = true,
                    Message = $"Database '{displayName}' created successfully with {columns.Count} custom columns",
                    SystemName = systemName,
                    Columns = columns.Select(c => new { Name = c.Name, Type = c.DataType.ToString() }).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database from prompt: {Message}", ex.Message);
                return $"Error creating database: {ex.Message}";
            }
        }

        /// <summary>
        /// Parse the schema description to extract column information
        /// </summary>
        private List<ColumnInfo> ParseSchemaDescription(string schemaDescription)
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
            return typeName.ToLower() switch
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
        /// Add standard columns to the database
        /// </summary>
        private void AddStandardColumns(MixDbDatabaseViewModel db)
        {
            // Id column
            AddColumnToViewModel(db, "id", MixDataType.Integer, true, "Primary key");
            
            // Created by
            AddColumnToViewModel(db, "created_by", MixDataType.String, false, "User who created the record");
            
            // Created datetime
            AddColumnToViewModel(db, "created_date_time", MixDataType.DateTime, true, "Creation date and time", "now()");
            
            // Last modified
            AddColumnToViewModel(db, "last_modified", MixDataType.DateTime, true, "Last modification date and time", "now()");
            
            // Priority
            AddColumnToViewModel(db, "priority", MixDataType.Integer, false, "Display priority", "0");
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