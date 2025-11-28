using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Extensions;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.Service.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service xử lý và chuyển đổi dữ liệu MixDb
    /// </summary>
    public class MixDbDataParser : IMixDbDataParser
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MixDbDataParser> _logger;
        private readonly FieldNameService _fieldNameService;
        private readonly IMixDbInfoService _mixDbInfoService;
        private MixDbTableViewModel _mixDb;

        public MixDbDataParser(
            IConfiguration configuration,
            ILogger<MixDbDataParser> logger,
            IMixDbInfoService mixDbInfoService)
        {
            _configuration = configuration;
            _logger = logger;
            _fieldNameService = new FieldNameService(MixDatabaseNamingConvention.SnakeCase);
            _mixDbInfoService = mixDbInfoService;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> ParseDtoToEntityAsync(JObject dto, string? username = null)
        {
            try
            {
                Dictionary<string, object> result = new();
                var encryptedColumnNames = _mixDb.Columns
                    .Where(m => m.ColumnConfigurations.IsEncrypt)
                    .Select(c => c.SystemName)
                    .ToList();

                foreach (var pr in dto.Properties())
                {
                    var colName = _fieldNameService.NamingConvention == MixDatabaseNamingConvention.TitleCase
                        ? pr.Name.ToTitleCase()
                        : pr.Name.ToHyphenCase('_', true);
                    var col = _mixDb.Columns.FirstOrDefault(c => c.SystemName.Equals(colName, StringComparison.InvariantCultureIgnoreCase));
                    if (colName.ToLower() == "id" && pr.Value is null)
                    {
                        continue;
                    }
                    if (encryptedColumnNames.Contains(colName))
                    {
                        result.Add(colName, AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    _configuration.AesKey()));
                    }
                    else
                    {
                        if (col != null)
                        {
                            result.Add(colName, ParseObjectValueToDbType(col.DataType, pr.Value));
                        }
                        else
                        {
                            result.Add(colName, GetPropertyValue(pr));
                        }
                    }
                }

                AddDefaultFields(result, username);
                return result;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when converting DTO to Entity");
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        /// <inheritdoc/>
        public object? ParseObjectValueToDbType(MixDataType? dataType, JToken value)
        {
            try
            {
                if (value != null)
                {
                    string strValue = value.ToString();
                    if (string.IsNullOrEmpty(strValue))
                    {
                        return default;
                    }
                    return dataType switch
                    {
                        MixDataType.Date or MixDataType.DateTime => ParseDateTime(strValue),
                        MixDataType.Boolean => bool.Parse(strValue),
                        MixDataType.Array or MixDataType.ArrayMedia => ParseArray(value),
                        MixDataType.Json or MixDataType.ArrayRadio => ParseJson(value),
                        MixDataType.Integer or MixDataType.Reference => int.Parse(strValue),
                        MixDataType.Double => double.Parse(strValue),
                        MixDataType.Guid => ParseGuid(value),
                        _ => value.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when converting value to data type {dataType}");
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
            return null;
        }

        /// <inheritdoc/>
        public async Task<object> ExtractIdAsync(string tableName, JObject obj)
        {
            try
            {
                await LoadMixDb(tableName);
                return _mixDb.Type == MixDbTableType.GuidService
                    ? obj.GetJObjectProperty<Guid>(_fieldNameService.Id)
                    : obj.GetJObjectProperty<int>(_fieldNameService.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when extracting ID from object in table {tableName}");
                throw;
            }
        }

        /// <inheritdoc/>
        public object? GetPropertyValue(JProperty prop)
        {
            try
            {
                string strVal = prop.Value.ToString();
                return string.IsNullOrEmpty(strVal) || prop.Value?.Type == null
                    ? default
                    : prop.Value.Type switch
                    {
                        JTokenType.Date => DateTime.Parse(strVal),
                        JTokenType.Array => JArray.Parse(strVal),
                        JTokenType.Object => JObject.Parse(strVal),
                        JTokenType.Integer => int.Parse(strVal),
                        JTokenType.Float => double.Parse(strVal),
                        JTokenType.Boolean => bool.Parse(strVal),
                        _ => prop.Value?.ToString()
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting property value {prop.Name}");
                throw;
            }
        }

        private void AddDefaultFields(Dictionary<string, object> result, string? username)
        {
            if (!result.ContainsKey(_fieldNameService.Id))
            {
                if (_mixDb.Type == MixDbTableType.GuidService)
                {
                    result.Add(_fieldNameService.Id, Guid.NewGuid());
                }
                else
                {
                    result.Add(_fieldNameService.Id, string.Empty);
                }
                if (!result.ContainsKey(_fieldNameService.LastModified)
                    && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.LastModified))
                {
                    result.Add(_fieldNameService.LastModified, DateTime.UtcNow);
                }
            }
            else
            {
                if (_mixDb.Columns.Any(m => m.SystemName == _fieldNameService.ModifiedBy))
                {
                    result[_fieldNameService.ModifiedBy] = username ?? string.Empty;
                }
                if (_mixDb.Columns.Any(m => m.SystemName == _fieldNameService.LastModified))
                {
                    result[_fieldNameService.LastModified] = DateTime.UtcNow;
                }
            }

            AddCommonFields(result, username);
        }

        private void AddCommonFields(Dictionary<string, object> result, string? username)
        {
            if (!result.ContainsKey(_fieldNameService.CreatedBy))
            {
                result.Add(_fieldNameService.CreatedBy, username ?? string.Empty);
            }
            if (!result.ContainsKey(_fieldNameService.CreatedDateTime)
                && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.CreatedDateTime))
            {
                result.Add(_fieldNameService.CreatedDateTime, DateTime.UtcNow);
            }
            if (!result.ContainsKey(_fieldNameService.Priority)
                && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.Priority))
            {
                result.Add(_fieldNameService.Priority, 0);
            }
            if (!result.ContainsKey(_fieldNameService.Status))
            {
                result.Add(_fieldNameService.Status, MixContentStatus.Published.ToString());
            }
            if (!result.ContainsKey(_fieldNameService.IsDeleted))
            {
                result.Add(_fieldNameService.IsDeleted, false);
            }
        }

        private DateTime ParseDateTime(string strValue)
        {
            DateTime.TryParse(strValue, out var dateValue);
            if (dateValue.Kind != DateTimeKind.Utc)
            {
                return dateValue.ToUniversalTime();
            }
            return dateValue;
        }

        private string ParseArray(JToken value)
        {
            return value.Type != JTokenType.String
                ? JArray.FromObject(value).ToString(Formatting.None)
                : value.Value<string>();
        }

        private string ParseJson(JToken value)
        {
            return value.Type != JTokenType.String
                ? JObject.FromObject(value).ToString(Formatting.None)
                : value.Value<string>();
        }

        private Guid ParseGuid(JToken value)
        {
            Guid.TryParse(value.ToString(), out var guildResult);
            return guildResult;
        }

        public async Task LoadMixDb(string tableName)
        {
            if (_mixDb != null && _mixDb.SystemName == tableName)
            {
                return;
            }
            await _mixDbInfoService.LoadMixDb(tableName);
            _mixDb = await _mixDbInfoService.GetMixDb(tableName);
            if (_mixDb == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid table name");
            }
        }


    }
}