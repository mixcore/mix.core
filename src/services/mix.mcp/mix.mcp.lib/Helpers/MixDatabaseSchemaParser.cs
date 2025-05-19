using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.LLM;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace Mix.MCP.Lib.Helpers
{
    /// <summary>
    /// Helper class for parsing database schema descriptions
    /// </summary>
    public class MixDatabaseSchemaParser
    {
        private readonly ILogger _logger;
        private readonly ILlmServiceFactory _llmServiceFactory;

        /// <summary>
        /// Initialize a new instance of MixDatabaseSchemaParser
        /// </summary>
        public MixDatabaseSchemaParser(ILlmServiceFactory llmServiceFactory, ILogger logger)
        {
            _llmServiceFactory = llmServiceFactory;
            _logger = logger;
        }

        /// <summary>
        /// Parse the schema description using LLM to extract column information
        /// </summary>
        public async Task<List<ColumnInfo>> ParseSchemaDescriptionWithLLM(
            string schemaDescription, 
            LLMServiceType serviceType, 
            string model,
            CancellationToken cancellationToken = default)
        {
            var columns = new List<ColumnInfo>();

            try
            {
                // CreateMixDbData LLM service
                var llmService = _llmServiceFactory.CreateService(serviceType);

                // CreateMixDbData prompt for the LLM
                string prompt = $@"
You are a database schema designer for a CMS system. Analyze this database schema description and extract structured column information.

Schema description: ""{schemaDescription}""

For each column/field mentioned or implied in the description:
1. Determine an appropriate snake_case name (e.g., product_name)
2. Select the most appropriate data type from these options:
   - String: For short text (names, titles, codes, etc.)
   - Text: For long text content (descriptions, articles, etc.)
   - Integer: For whole numbers
   - Double: For decimal numbers (prices, measurements, etc.)
   - Boolean: For true/false values
   - DateTime: For dates and times
   - Reference: For references to other entities
   - Upload: For files, images, or other uploads
   - Json: For structured data
3. Determine if the field should be required (true/false)
4. Add a default value if appropriate (optional)

Response format (JSON array only):
[
  {{
    ""name"": ""product_name"",
    ""dataType"": ""String"",
    ""isRequired"": true,
    ""defaultValue"": null
  }},
  {{
    ""name"": ""price"",
    ""dataType"": ""Double"",
    ""isRequired"": true,
    ""defaultValue"": ""0.00""
  }}
]

Do NOT include standard system fields (id, created_by, created_date_time, etc.) as they will be added automatically.
Focus on extracting domain-specific fields from the description.
If the description is ambiguous, make reasonable inferences based on common database patterns and best practices.
If the description mentions fields that don't clearly map to a data type, choose the most appropriate one.
Respond ONLY with a valid JSON array.";

                // Get response from LLM
                var response = await llmService.ChatAsync(prompt, model, 0.7, -1, cancellationToken);
                
                if (response?.choices == null || response.choices.Length == 0 || 
                    string.IsNullOrEmpty(response.choices[0]?.Message.Content))
                {
                    _logger.LogWarning("No columns extracted from LLM response");
                    return columns;
                }

                string responseText = response.choices[0].Message.Content;
                _logger.LogDebug("LLM Response: {Response}", responseText);

                // Extract JSON array from response (handling possible text before/after JSON)
                string jsonContent = ExtractJsonArrayFromText(responseText);
                
                if (string.IsNullOrEmpty(jsonContent))
                {
                    _logger.LogWarning("Could not extract valid JSON from LLM response: {Response}", responseText);
                    
                    // Try a more aggressive JSON extraction
                    jsonContent = ExtractJsonWithRegex(responseText);
                    
                    if (string.IsNullOrEmpty(jsonContent))
                    {
                        _logger.LogWarning("No columns extracted from LLM response");
                        return columns;
                    }
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
                                DefaultValue = GetStringPropertyOrDefault(columnElement, "defaultValue", null)
                            };

                            // Parse data type
                            string dataTypeStr = GetStringPropertyOrDefault(columnElement, "dataType", "String");
                            column.DataType = MapStringToDataType(dataTypeStr);

                            // Validate and clean column name
                            if (!string.IsNullOrWhiteSpace(column.Name))
                            {
                                column.Name = CleanColumnName(column.Name);
                                columns.Add(column);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing LLM response as JSON: {Response}", jsonContent);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using LLM to parse schema description: {Message}", ex.Message);
                throw;
            }

            // If no columns were extracted, fall back to regex parsing
            if (columns.Count == 0)
            {
                _logger.LogWarning("No columns extracted from LLM response");
            }

            return columns;
        }

        /// <summary>
        /// Clean column name to ensure it uses valid characters and naming conventions
        /// </summary>
        public static string CleanColumnName(string name)
        {
            // Convert to lowercase
            string cleanName = name.ToLower();
            
            // Replace spaces and special characters with underscores
            cleanName = Regex.Replace(cleanName, @"[\s\-\.,]", "_");
            
            // Remove any non-alphanumeric characters (except underscores)
            cleanName = Regex.Replace(cleanName, @"[^a-z0-9_]", "");
            
            // Remove consecutive underscores
            cleanName = Regex.Replace(cleanName, @"_+", "_");
            
            // Remove leading and trailing underscores
            cleanName = cleanName.Trim('_');
            
            // Ensure name isn't empty
            if (string.IsNullOrEmpty(cleanName))
            {
                cleanName = "field";
            }
            
            return cleanName;
        }

        /// <summary>
        /// Extract JSON array from text using regex as a fallback method
        /// </summary>
        private static string ExtractJsonWithRegex(string text)
        {
            // Try to match a JSON array with any content
            var match = Regex.Match(text, @"\[\s*{.*}\s*\]", RegexOptions.Singleline);
            
            if (match.Success)
            {
                return match.Value;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Extract JSON array from text that may contain explanatory text before/after the JSON
        /// </summary>
        private static string ExtractJsonArrayFromText(string text)
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
        private static string GetStringPropertyOrDefault(JsonElement element, string propertyName, string defaultValue)
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
        private static bool GetBoolPropertyOrDefault(JsonElement element, string propertyName, bool defaultValue)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                if (property.ValueKind == JsonValueKind.True || property.ValueKind == JsonValueKind.False)
                {
                    return property.GetBoolean();
                }
                else if (property.ValueKind == JsonValueKind.String)
                {
                    // Handle string representations of boolean values
                    string boolStr = property.GetString()?.ToLower();
                    if (boolStr == "true" || boolStr == "yes" || boolStr == "1")
                    {
                        return true;
                    }
                    else if (boolStr == "false" || boolStr == "no" || boolStr == "0")
                    {
                        return false;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Convert string type name to MixDataType
        /// </summary>
        public static MixDataType MapStringToDataType(string typeName)
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
    }
} 