using Mix.Constant.Enums;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Identity.Constants;
using Mix.RepoDb.ViewModels;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.RepoDb.Helpers
{
    public class MixDbHelper
    {
        public const string CreatedByFieldName = "CreatedBy";
        public const string ModifiedByFieldName = "ModifiedBy";
        public const string LastModifiedFieldName = "LastModified";
        public const string CreatedDateFieldName = "CreatedDateTime";
        public const string PriorityFieldName = "Priority";
        public const string IdFieldName = "Id";
        public const string ParentIdFieldName = "ParentId";
        public const string ChildIdFieldName = "ChildId";
        public const string TenantIdFieldName = "MixTenantId";
        public const string StatusFieldName = "Status";
        public const string IsDeletedFieldName = "IsDeleted";

        public static Task<JObject> ParseDtoToEntityAsync(JObject dto, List<MixDatabaseColumnViewModel> columns, int? tenantId = null, string? username = null)
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
                                    pr.Name.ToTitleCase(), AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    GlobalConfigService.Instance.AppSettings.ApiEncryptKey)));
                    }
                    else
                    {
                        if (col != null)
                        {
                            if (col.DataType == MixDataType.Json || col.DataType == MixDataType.ArrayRadio)
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), JObject.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Array || col.DataType == MixDataType.ArrayMedia)
                            {

                                result.Add(new JProperty(pr.Name.ToTitleCase(), JArray.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Boolean)
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), bool.Parse(pr.Value.ToString())));
                            }
                            else
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
                            }
                        }
                        else
                        {
                            result.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
                        }
                    }
                }

                if (!result.ContainsKey(IdFieldName))
                {
                    result.Add(new JProperty(IdFieldName, null));
                    if (!result.ContainsKey(CreatedByFieldName))
                    {
                        result.Add(new JProperty(CreatedByFieldName, username));
                    }
                    if (!result.ContainsKey(CreatedDateFieldName))
                    {
                        result.Add(new JProperty(CreatedDateFieldName, DateTime.UtcNow));
                    }
                    if (!result.ContainsKey(CreatedByFieldName))
                    {
                        result.Add(new JProperty(CreatedByFieldName, username));
                    }
                }
                else
                {
                    result[ModifiedByFieldName] = username;
                    result[LastModifiedFieldName] = DateTime.UtcNow;
                }

                if (!result.ContainsKey(PriorityFieldName))
                {
                    result.Add(new JProperty(PriorityFieldName, 0));
                }
                if (!result.ContainsKey(TenantIdFieldName))
                {
                    result.Add(new JProperty(TenantIdFieldName, tenantId));
                }

                if (!result.ContainsKey(StatusFieldName))
                {
                    result.Add(new JProperty(StatusFieldName, MixContentStatus.Published.ToString()));
                }

                if (!result.ContainsKey(IsDeletedFieldName))
                {
                    result.Add(new JProperty(IsDeletedFieldName, false));
                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        
        public static Task<JObject> ParseImportDtoToEntityAsync(JObject dto, List<MixDatabaseColumnViewModel> columns, int? tenantId = null, string? username = null)
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
                                    pr.Name.ToTitleCase(), AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    GlobalConfigService.Instance.AppSettings.ApiEncryptKey)));
                    }
                    else
                    {
                        if (col != null)
                        {
                            if (col.DataType == MixDataType.Json || col.DataType == MixDataType.ArrayRadio)
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), JObject.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Array || col.DataType == MixDataType.ArrayMedia)
                            {

                                result.Add(new JProperty(pr.Name.ToTitleCase(), JArray.FromObject(pr.Value).ToString(Formatting.None)));
                            }
                            else if (col.DataType == MixDataType.Date || col.DataType == MixDataType.DateTime)
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), DateTime.Parse(pr.Value.ToString())));
                            }
                            else if (col.DataType == MixDataType.Boolean)
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), bool.Parse(pr.Value.ToString())));
                            }
                            else
                            {
                                result.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
                            }
                        }
                        else
                        {
                            result.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
                        }
                    }
                }

                if (!result.ContainsKey(IdFieldName))
                {
                    result.Add(new JProperty(IdFieldName, null));
                    if (!result.ContainsKey(CreatedByFieldName))
                    {
                        result.Add(new JProperty(CreatedByFieldName, username));
                    }
                    if (!result.ContainsKey(CreatedDateFieldName))
                    {
                        result.Add(new JProperty(CreatedDateFieldName, DateTime.UtcNow));
                    }
                    if (!result.ContainsKey(CreatedByFieldName))
                    {
                        result.Add(new JProperty(CreatedByFieldName, username));
                    }
                }
                else
                {
                    result[CreatedDateFieldName] = DateTime.Parse(result[CreatedDateFieldName]!.ToString());
                    result[ModifiedByFieldName] = username;
                    result[LastModifiedFieldName] = DateTime.UtcNow;
                    result[IsDeletedFieldName] = bool.Parse(result[IsDeletedFieldName]!.ToString());
                }

                if (!result.ContainsKey(PriorityFieldName))
                {
                    result.Add(new JProperty(PriorityFieldName, 0));
                }
                if (!result.ContainsKey(TenantIdFieldName))
                {
                    result.Add(new JProperty(TenantIdFieldName, tenantId));
                }

                if (!result.ContainsKey(StatusFieldName))
                {
                    result.Add(new JProperty(StatusFieldName, MixContentStatus.Published.ToString()));
                }

                if (!result.ContainsKey(IsDeletedFieldName))
                {
                    result.Add(new JProperty(IsDeletedFieldName, false));
                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
    }
}
