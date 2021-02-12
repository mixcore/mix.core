using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Extensions;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.Extensions
{
    public static class MixAttributeValueModelExtensions
    {
        public static JProperty ToJProperty(
            this MixAttributeSetValue item,
            MixCmsContext _context,
            IDbContextTransaction _transaction)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                   _context, _transaction,
                   out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            switch (item.DataType)
            {
                case MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);

                case MixDataType.Date:
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

                case MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue ?? 0));

                case MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));

                case MixDataType.Integer:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue ?? 0));

                case MixDataType.Reference:
                    return (new JProperty(item.AttributeFieldName, new JArray()));
                case MixDataType.Upload:
                    string domain = MixService.GetConfig<string>(MixAppSettingKeywords.Domain);
                    string url = !string.IsNullOrEmpty(item.StringValue)
                   ? !item.StringValue.Contains(domain) 
                        ? $"{MixService.GetConfig<string>(MixAppSettingKeywords.Domain)}{item.StringValue}"
                        :  item.StringValue
                   : null;
                    return (new JProperty(item.AttributeFieldName, url));
                case MixDataType.Custom:
                case MixDataType.Duration:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.Html:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
        }

        public static void ToModelValue(this ViewModels.MixAttributeSetValues.UpdateViewModel item, JToken property)
        {
            if (property == null)
            {
                return;
            }

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
                    case MixDataType.DateTime:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Date:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Time:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Double:
                        item.DoubleValue = property.Value<double?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Boolean:
                        item.BooleanValue = property.Value<bool?>();
                        item.StringValue = property.Value<string>()?.ToLower();
                        break;

                    case MixDataType.Integer:
                        item.IntegerValue = property.Value<int?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Reference:
                        item.StringValue = property.Value<string>();
                        break;

                    case MixDataType.Upload:
                        string mediaData = property.Value<string>();
                        if (mediaData.IsBase64())
                        {
                            ViewModels.MixMedias.UpdateViewModel media = new ViewModels.MixMedias.UpdateViewModel()
                            {
                                Specificulture = item.Specificulture,
                                Status = MixContentStatus.Published,
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

                    case MixDataType.Custom:
                    case MixDataType.Duration:
                    case MixDataType.PhoneNumber:
                    case MixDataType.Text:
                    case MixDataType.Html:
                    case MixDataType.MultilineText:
                    case MixDataType.EmailAddress:
                    case MixDataType.Password:
                    case MixDataType.Url:
                    case MixDataType.ImageUrl:
                    case MixDataType.CreditCard:
                    case MixDataType.PostalCode:
                    case MixDataType.Color:
                    case MixDataType.Icon:
                    case MixDataType.VideoYoutube:
                    case MixDataType.TuiEditor:
                    default:
                        item.StringValue = property.Value<string>();
                        break;
                }
            }

        }

        public static void LoadAllReferenceData(this JObject obj
           , string dataId, int attributeSetId, string culture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var refFields = context.MixAttributeField.Where(
                   m => m.AttributeSetId == attributeSetId
                    && m.DataType == MixDataType.Reference).ToList();

            foreach (var item in refFields)
            {
                JArray arr = GetRelatedData(item.ReferenceId.Value, dataId, culture, _context, _transaction);

                if (obj.ContainsKey(item.Name))
                {
                    obj[item.Name] = arr;
                }
                else
                {
                    obj.Add(new JProperty(item.Name, arr));
                }
            }
            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
        }

        private static JArray GetRelatedData(int referenceId, string dataId, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                    (model.AttributeSetId == referenceId)
                    && (model.ParentId == dataId && model.ParentType == MixDatabaseParentType.Set)
                    && model.Specificulture == culture
                    ;
            var getData = ViewModels.MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(predicate, context, transaction);

            JArray arr = new JArray();
            foreach (var nav in getData.Data.OrderBy(v => v.Priority))
            {
                arr.Add(nav.Data.Obj);
            }
            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
            return arr;
        }
    }
}
