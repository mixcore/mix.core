using Mix.Constant.Enums;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Services;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.RepoDb.Helpers
{
    public class MixDbHelper
    {
        public static Task<JObject> ParseDtoToEntityAsync(JObject dto, List<RepoDbMixDatabaseColumnViewModel> columns, FieldNameService fieldNameService, int? tenantId = null, string? username = null)
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
                    var colName = fieldNameService.NamingConvention == MixDatabaseNamingConvention.TitleCase? pr.Name.ToTitleCase() : pr.Name;
                    var col = columns.FirstOrDefault(c => c.SystemName.Equals(colName, StringComparison.InvariantCultureIgnoreCase));

                    if (encryptedColumnNames.Contains(colName))
                    {
                        result.Add(
                            new JProperty(
                                    colName, AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    GlobalConfigService.Instance.AppSettings.ApiEncryptKey)));
                    }
                    else
                    {
                        if (col != null)
                        {
                            result.Add(new JProperty(colName, ParseObjectValue(col.DataType, pr.Value)));
                        }
                        else
                        {
                            result.Add(new JProperty(colName, pr.Value));
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
                    result[fieldNameService.ModifiedBy] = username;
                    result[fieldNameService.LastModified] = DateTime.UtcNow;
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

        public static Task<JObject> ParseImportDtoToEntityAsync(JObject dto, List<RepoDbMixDatabaseColumnViewModel> columns,
            FieldNameService fieldNameService,
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
                                    GlobalConfigService.Instance.AppSettings.ApiEncryptKey)));
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

        public static object? ParseObjectValue(MixDataType? dataType, JToken value)
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
                        return DateTime.Parse(strValue).ToUniversalTime();
                    case MixDataType.Boolean:
                        return bool.Parse(strValue);
                    case MixDataType.Array:
                    case MixDataType.ArrayMedia:
                        return JArray.FromObject(value).ToString(Formatting.None);
                    case MixDataType.Json:
                    case MixDataType.ArrayRadio:
                        return JObject.FromObject(value).ToString(Formatting.None);
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
            return null;
        }
    }
}
