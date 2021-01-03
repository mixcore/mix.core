using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDataValues
{
    public static class Helper
    {
        public static JProperty ToJProperty(this MixAttributeSetValue item)
        {
            switch (item.DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);

                case MixEnums.MixDataType.Date:
                    if (!item.DateTimeValue.HasValue)
                    {
                        if (DateTime.TryParseExact(
                            item.StringValue,
                            "MM/dd/yyyy HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.RoundtripKind,
                            out DateTime date))
                        {
                            item.DateTimeValue = date;
                        }
                    }
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixEnums.MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixEnums.MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue ?? 0));

                case MixEnums.MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));

                case MixEnums.MixDataType.Integer:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue ?? 0));

                case MixEnums.MixDataType.Reference:
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    return (new JProperty(item.AttributeFieldName, null));

                case MixEnums.MixDataType.Custom:
                case MixEnums.MixDataType.Duration:
                case MixEnums.MixDataType.PhoneNumber:
                case MixEnums.MixDataType.Text:
                case MixEnums.MixDataType.Html:
                case MixEnums.MixDataType.MultilineText:
                case MixEnums.MixDataType.EmailAddress:
                case MixEnums.MixDataType.Password:
                case MixEnums.MixDataType.Url:
                case MixEnums.MixDataType.ImageUrl:
                case MixEnums.MixDataType.CreditCard:
                case MixEnums.MixDataType.PostalCode:
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
        }

        public static void ToModelValue(this MixAttributeSetValues.UpdateViewModel item, JToken property)
        {
            if (item.Field.IsEncrypt)
            {
                var obj = property.Value<JObject>();
                item.StringValue = obj.ToString(Formatting.None);
                item.EncryptValue = obj["data"]?.ToString();
                item.EncryptKey = obj["key"]?.ToString();
            }
            else
            {
                switch (item.Field.DataType)
                {
                    case MixEnums.MixDataType.DateTime:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Date:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Time:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Double:
                        item.DoubleValue = property.Value<double?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Boolean:
                        item.BooleanValue = property.Value<bool?>();
                        item.StringValue = property.Value<string>()?.ToLower();
                        break;

                    case MixEnums.MixDataType.Integer:
                        item.IntegerValue = property.Value<int?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Reference:
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Upload:
                        string mediaData = property.Value<string>();
                        if (mediaData.IsBase64())
                        {
                            MixMedias.UpdateViewModel media = new MixMedias.UpdateViewModel()
                            {
                                Specificulture = item.Specificulture,
                                Status = MixEnums.MixContentStatus.Published,
                                MediaFile = new FileViewModel()
                                {
                                    FileStream = mediaData,
                                    Extension = ".png",
                                    Filename = Guid.NewGuid().ToString(),
                                    FileFolder = "Attributes"
                                }
                            };
                            var saveMedia = media.SaveModel(true);
                            if (saveMedia.IsSucceed)
                            {
                                item.StringValue = saveMedia.Data.FullPath;
                            }
                        }
                        else
                        {
                            item.StringValue = mediaData;
                        }
                        break;

                    case MixEnums.MixDataType.Custom:
                    case MixEnums.MixDataType.Duration:
                    case MixEnums.MixDataType.PhoneNumber:
                    case MixEnums.MixDataType.Text:
                    case MixEnums.MixDataType.Html:
                    case MixEnums.MixDataType.MultilineText:
                    case MixEnums.MixDataType.EmailAddress:
                    case MixEnums.MixDataType.Password:
                    case MixEnums.MixDataType.Url:
                    case MixEnums.MixDataType.ImageUrl:
                    case MixEnums.MixDataType.CreditCard:
                    case MixEnums.MixDataType.PostalCode:
                    case MixEnums.MixDataType.Color:
                    case MixEnums.MixDataType.Icon:
                    case MixEnums.MixDataType.VideoYoutube:
                    case MixEnums.MixDataType.TuiEditor:
                    default:
                        item.StringValue = property.Value<string>();
                        break;
                }
            }

        }


        public static async Task<RepositoryResponse<List<TView>>> FilterByOtherValueAsync<TView>(
            string culture, string attributeSetName
            , string filterType, Dictionary<string, string> queries
            , string responseName
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetValue, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.AttributeSetName == attributeSetName;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                foreach (var fieldQuery in queries)
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = GetValueFilter(filterType, fieldQuery.Key, fieldQuery.Value);
                    valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                }
                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixAttributeSetValue, bool>> predicate =
                        m => dataIds.Any(id => m.DataId == id) &&
                            m.AttributeFieldName == responseName;
                    result = await DefaultRepository<MixCmsContext, MixAttributeSetValue, TView>.Instance.GetModelListByAsync(
                        predicate, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<List<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private static Expression<Func<MixAttributeSetValue, bool>> GetValueFilter(string filterType, string key, string value)
        {
            switch (filterType)
            {
                case "equal":
                    return m => m.AttributeFieldName == key
                        && (EF.Functions.Like(m.StringValue, $"{value}"));
                case "contain":
                    return m => m.AttributeFieldName == key &&
                                            (EF.Functions.Like(m.StringValue, $"%{value}%"));

            }
            return null;
        }
    }
}