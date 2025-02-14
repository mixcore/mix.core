using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Services;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Helpers
{
    public class MixDbHelper
    {

        #region Build Search Request
        public static Task<List<MixQueryField>> BuildSearchQueryAsync(SearchMixDbRequestDto request, MixDbDatabaseViewModel mixDb, FieldNameService fieldNameService)
        {
            var queries = new List<MixQueryField>();
            if (request.ObjParentId != null)
            {
                queries.Add(new MixQueryField(fieldNameService.GetParentId(request.ParentName), request.ObjParentId));
            }
            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    var col = mixDb.Columns.FirstOrDefault(m => m.SystemName == query.FieldName);
                    if (col != null)
                    {
                        queries.Add(new(query.FieldName,

                            ParseSearchKeyword(
                                query.CompareOperator,
                                query.Value),
                            query.CompareOperator));
                    }
                }
            }

            AddCursorQuery(request, queries, mixDb);
            return Task.FromResult(queries);
        }
        private static void AddCursorQuery(SearchMixDbRequestDto request, List<MixQueryField> queries, MixDbDatabaseViewModel mixDb)
        {
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var orderByCol = mixDb.Columns.FirstOrDefault(m => m.SystemName == request.SortBy);
                object? val = request.After ?? request.Before;

                if (orderByCol != null)
                {
                    val = MixDbHelper.ParseObjectValue(orderByCol.DataType, val);
                }
                if (val != null)
                {
                    queries.Add(new(request.SortBy, val, request.After != null ? MixCompareOperator.GreaterThan : MixCompareOperator.LessThan));
                }
            }
        }
        
        #endregion
        public static object? ParseSearchKeyword(MixCompareOperator? searchMethod, object? keyword)
        {
            if (keyword is null)
            {
                return keyword;
            }
            return searchMethod switch
            {
                MixCompareOperator.Like => $"%{keyword}%",
                _ => keyword
            };
        }
        public static object? GetJPropertyValue(JProperty prop)
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
        public static bool IsLongTextColumn(MixdbDatabaseColumnViewModel col)
        {
            return col.DataType == MixDataType.Text
                   | col.DataType == MixDataType.Array
                   | col.DataType == MixDataType.ArrayMedia
                   | col.DataType == MixDataType.ArrayRadio
                   | col.DataType == MixDataType.Html
                   | col.DataType == MixDataType.Json
                   | col.DataType == MixDataType.TuiEditor;
        }

        public static void ParseRawDataToEntityAsync(JObject? result, List<MixdbDatabaseColumnViewModel> columns)
        {
            if (result is null)
            {
                return;
            }

            foreach (var pr in result.Properties())
            {
                var col = columns.FirstOrDefault(c => c.SystemName.Equals(pr.Name, StringComparison.InvariantCultureIgnoreCase));
                if (col != null && pr.Value != null)
                {
                    string? strVal = pr.Value?.ToString();
                    switch (col.DataType)
                    {
                        case MixDataType.Json:
                        case MixDataType.ArrayRadio:
                            result[pr.Name] = !string.IsNullOrEmpty(strVal) ? JObject.Parse(strVal) : null;
                            break;
                        case MixDataType.Tag:
                        case MixDataType.Array:
                        case MixDataType.ArrayMedia:
                            result[pr.Name] = !string.IsNullOrEmpty(strVal) ? JArray.Parse(strVal) : null;
                            break;
                        case MixDataType.Date:
                        case MixDataType.DateTime:
                            result[pr.Name] = pr.Value?.ToObject<DateTime?>();
                            break;
                        case MixDataType.Boolean:
                            result[pr.Name] = pr.Value?.ToObject<bool?>();
                            break;
                        case MixDataType.Integer:
                            result[pr.Name] = pr.Value?.ToObject<int?>();
                            break;
                        case MixDataType.Double:
                            result[pr.Name] = pr.Value?.ToObject<double?>();
                            break;
                        default:
                            result[pr.Name] = strVal;
                            break;
                    }
                }
            }
        }

        public static Task<JObject> ParseImportDtoToEntityAsync(JObject dto, List<MixdbDatabaseColumnViewModel> columns,
            FieldNameService fieldNameService,
            string aesKey,
            int? tenantId = null, string? username = null)
        {
            try
            {
                JObject result = new();
                var encryptedColumnNames = columns
                    .Where(m => m.ColumnConfigurations.IsEncrypt)
                    .Select(c => c.SystemName)
                    .ToList();
                foreach (var pr in dto.Properties())
                {
                    var col = columns.FirstOrDefault(c => c.SystemName.Equals(pr.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (encryptedColumnNames.Contains(pr.Name))
                    {
                        result.Add(
                            new JProperty(
                                    pr.Name, AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    aesKey)));
                    }
                    else
                    {
                        if (col != null)
                        {
                            if (col.DataType == MixDataType.Json || col.DataType == MixDataType.ArrayRadio)
                            {
                                result.Add(new JProperty(pr.Name, JObject.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Array || col.DataType == MixDataType.ArrayMedia)
                            {

                                result.Add(new JProperty(pr.Name, JArray.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Date || col.DataType == MixDataType.DateTime)
                            {
                                result.Add(new JProperty(pr.Name, DateTime.Parse(pr.Value.ToString())));
                            }
                            else if (col.DataType == MixDataType.Boolean)
                            {
                                result.Add(new JProperty(pr.Name, bool.Parse(pr.Value.ToString())));
                            }
                            else
                            {
                                result.Add(new JProperty(pr.Name, pr.Value));
                            }
                        }
                        else
                        {
                            result.Add(new JProperty(pr.Name, pr.Value));
                        }
                    }
                }

                if (!result.ContainsKey(fieldNameService.Id))
                {
                    result.Add(new JProperty(fieldNameService.Id, string.Empty));
                    if (!result.ContainsKey(fieldNameService.CreatedBy))
                    {
                        result.Add(new JProperty(fieldNameService.CreatedBy, username));
                    }
                    if (!result.ContainsKey(fieldNameService.CreatedDateTime))
                    {
                        result.Add(new JProperty(fieldNameService.CreatedDateTime, DateTime.UtcNow));
                    }
                    if (!result.ContainsKey(fieldNameService.CreatedBy))
                    {
                        result.Add(new JProperty(fieldNameService.CreatedBy, username));
                    }
                }
                else
                {
                    result[fieldNameService.CreatedDateTime] = DateTime.Parse(result[fieldNameService.CreatedDateTime]!.ToString());
                    result[fieldNameService.ModifiedBy] = username;
                    result[fieldNameService.LastModified] = DateTime.UtcNow;
                    result[fieldNameService.IsDeleted] = bool.Parse(result[fieldNameService.IsDeleted]!.ToString());
                }

                if (!result.ContainsKey(fieldNameService.Priority))
                {
                    result.Add(new JProperty(fieldNameService.Priority, 0));
                }
                if (!result.ContainsKey(fieldNameService.TenantId))
                {
                    result.Add(new JProperty(fieldNameService.TenantId, tenantId));
                }

                if (!result.ContainsKey(fieldNameService.Status))
                {
                    result.Add(new JProperty(fieldNameService.Status, MixContentStatus.Published.ToString()));
                }

                if (!result.ContainsKey(fieldNameService.IsDeleted))
                {
                    result.Add(new JProperty(fieldNameService.IsDeleted, false));
                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public static object? ParseObjectValueToDbType(MixDataType? dataType, JToken value)
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
                    switch (dataType)
                    {
                        case MixDataType.Date:
                        case MixDataType.DateTime:
                            DateTime.TryParse(strValue, out var dateValue);
                            if (dateValue.Kind != DateTimeKind.Utc)
                            {
                                return dateValue.ToUniversalTime();
                            }
                            return dateValue;

                        case MixDataType.Boolean:
                            return bool.Parse(strValue);
                        case MixDataType.Array:
                        case MixDataType.ArrayMedia:
                            return value.Type != JTokenType.String
                                ? JArray.FromObject(value).ToString(Formatting.None)
                                : value.Value<string>();
                        case MixDataType.Json:
                        case MixDataType.ArrayRadio:
                            return value.Type != JTokenType.String
                                    ? JObject.FromObject(value).ToString(Formatting.None)
                                    : value.Value<string>(); ;
                        case MixDataType.Integer:
                        case MixDataType.Reference:
                            return int.Parse(strValue);
                        case MixDataType.Double:
                            return double.Parse(strValue);
                        case MixDataType.Guid:
                            Guid.TryParse(value.ToString(), out var guildResult);
                            return guildResult;
                        default:
                            return value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
            return null;
        }
        public static object? ParseObjectValue(MixDataType? dataType, object objValue)
        {
            try
            {
                if (objValue == null)
                {
                    return objValue;
                }

                if (objValue != null)
                {
                    if (dataType == null)
                    {
                        return objValue;
                    }

                    var strValue = objValue.ToString();
                    if (string.IsNullOrEmpty(strValue))
                    {
                        return objValue;
                    }

                    switch (dataType)
                    {
                        case MixDataType.Date:
                        case MixDataType.DateTime:
                            var obj = DateTime.Parse(strValue);
                            if (obj.Kind == DateTimeKind.Unspecified)
                            {
                                return DateTime.SpecifyKind(obj, DateTimeKind.Utc);
                            }
                            if (obj.Kind == DateTimeKind.Local)
                            {
                                return obj.ToUniversalTime();
                            }
                            return obj;
                        case MixDataType.Boolean:
                            return bool.Parse(strValue);
                        case MixDataType.Integer:
                        case MixDataType.Reference:
                            return int.Parse(strValue);
                        case MixDataType.Double:
                            return double.Parse(strValue);
                        case MixDataType.Guid:
                            Guid.TryParse(strValue, out var guildResult);
                            return guildResult;
                        case MixDataType.Array:
                        case MixDataType.ArrayMedia:
                        case MixDataType.Json:
                        case MixDataType.ArrayRadio:
                        default:
                            return strValue;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
            return null;
        }
        public static IDatabaseConstants GetDatabaseConstant(MixDatabaseProvider databaseProvider)
        {
            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                MixDatabaseProvider.SCYLLADB => new CassandraDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
