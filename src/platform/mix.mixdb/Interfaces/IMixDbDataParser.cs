using Mix.Constant.Enums;
using Mix.Mixdb.ViewModels;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Interfaces
{
    /// <summary>
    /// Interface defining methods for processing and converting MixDb data
    /// </summary>
    public interface IMixDbDataParser
    {
        /// <summary>
        /// Converts DTO to Entity
        /// </summary>
        Task<Dictionary<string, object>> ParseDtoToEntityAsync(JObject dto, string? username = null);

        /// <summary>
        /// Converts object value to database data type
        /// </summary>
        object? ParseObjectValueToDbType(MixDataType? dataType, JToken value);

        /// <summary>
        /// Extracts ID from object
        /// </summary>
        Task<object> ExtractIdAsync(string tableName, JObject obj);

        /// <summary>
        /// Gets property value
        /// </summary>
        object? GetPropertyValue(JProperty prop);
        Task LoadMixDb(string tableName);
    }
}