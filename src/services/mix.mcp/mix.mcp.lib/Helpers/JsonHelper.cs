using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Helpers
{
    public static class JsonHelper
    {
        private static readonly Regex JsonArrayRegex = new(@"\[\s*({[^{}]*(?:{[^{}]*}[^{}]*)*})\s*\]", RegexOptions.Singleline | RegexOptions.Compiled);
        /// <summary>
        /// Extract JSON array from text that may contain explanatory text before/after the JSON
        /// </summary>
        public static string ExtractJsonArrayFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            try
            {
                // First try to find a valid JSON array using bracket matching
                int startIndex = text.IndexOf('[');
                if (startIndex < 0) return string.Empty;

                int bracketCount = 1;
                int endIndex = startIndex + 1;

                while (endIndex < text.Length && bracketCount > 0)
                {
                    if (text[endIndex] == '[') bracketCount++;
                    if (text[endIndex] == ']') bracketCount--;
                    endIndex++;
                }

                if (bracketCount == 0)
                {
                    string jsonContent = text.Substring(startIndex, endIndex - startIndex);
                    // Validate that it's actually valid JSON
                    if (IsValidJson(jsonContent))
                    {
                        return jsonContent;
                    }
                }
            }
            catch (Exception)
            {
                // If bracket matching fails, continue to regex fallback
            }

            return string.Empty;
        }

        /// <summary>
        /// Extract JSON array from text that may contain explanatory text before/after the JSON
        /// </summary>
        public static string ExtractJsonObjectFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            try
            {
                // First try to find a valid JSON array using bracket matching
                int startIndex = text.IndexOf('{');
                if (startIndex < 0) return string.Empty;

                int bracketCount = 1;
                int endIndex = startIndex + 1;

                while (endIndex < text.Length && bracketCount > 0)
                {
                    if (text[endIndex] == '{') bracketCount++;
                    if (text[endIndex] == '}') bracketCount--;
                    endIndex++;
                }

                if (bracketCount == 0)
                {
                    string jsonContent = text.Substring(startIndex, endIndex - startIndex);
                    // Validate that it's actually valid JSON
                    if (IsValidJson(jsonContent))
                    {
                        return jsonContent;
                    }
                }
            }
            catch (Exception)
            {
                // If bracket matching fails, continue to regex fallback
                return ExtractJsonWithRegex(text);
            }
            return string.Empty;

        }

        /// <summary>
        /// Extract JSON array from text using regex as a fallback method
        /// </summary>
        public static string ExtractJsonWithRegex(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            try
            {
                var match = JsonArrayRegex.Match(text);
                if (match.Success)
                {
                    string jsonContent = match.Value;
                    // Validate that it's actually valid JSON
                    if (IsValidJson(jsonContent))
                    {
                        return jsonContent;
                    }
                }
            }
            catch (Exception)
            {
                // If regex matching fails, return empty string
            }

            return string.Empty;
        }

        /// <summary>
        /// Get string property from JsonElement with default value if not found
        /// </summary>
        public static string GetStringPropertyOrDefault(JsonElement element, string propertyName, string defaultValue)
        {
            try
            {
                if (element.TryGetProperty(propertyName, out JsonElement property))
                {
                    return property.ValueKind switch
                    {
                        JsonValueKind.String => property.GetString() ?? defaultValue,
                        JsonValueKind.Number => property.GetDecimal().ToString(),
                        JsonValueKind.True => "true",
                        JsonValueKind.False => "false",
                        JsonValueKind.Null => defaultValue,
                        _ => defaultValue
                    };
                }
            }
            catch (Exception)
            {
                // If any error occurs during property access, return default value
            }

            return defaultValue;
        }

        /// <summary>
        /// Get boolean property from JsonElement with default value if not found
        /// </summary>
        public static bool GetBoolPropertyOrDefault(JsonElement element, string propertyName, bool defaultValue)
        {
            try
            {
                if (element.TryGetProperty(propertyName, out JsonElement property))
                {
                    return property.ValueKind switch
                    {
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.String => ParseBoolString(property.GetString()),
                        JsonValueKind.Number => property.GetDecimal() != 0,
                        _ => defaultValue
                    };
                }
            }
            catch (Exception)
            {
                // If any error occurs during property access, return default value
            }

            return defaultValue;
        }

        /// <summary>
        /// Validate if a string is valid JSON
        /// </summary>
        private static bool IsValidJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(text);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Parse string representation of boolean values
        /// </summary>
        private static bool ParseBoolString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.ToLower() switch
            {
                "true" or "yes" or "1" or "y" or "on" => true,
                "false" or "no" or "0" or "n" or "off" => false,
                _ => false
            };
        }

    }
}
