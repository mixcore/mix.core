using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.Helpers
{
    public static class MixDataHelper
    {
        public static JObject ParseData(
           Guid dataContentId,
           UnitOfWorkInfo uowInfo)
        {
            var context = (MixCmsContext)uowInfo.ActiveDbContext;
            var values = context.MixDataContentValue.Where(
                m => m.MixDataContentId == dataContentId
                    && !string.IsNullOrEmpty(m.MixDatabaseColumnName));
            var properties = values.Select(m => m.ToJProperty());
            var obj = new JObject(
                new JProperty("id", dataContentId),
                properties
            );

            return obj;
        }

        public static JProperty ToJProperty(
            this MixDataContentValue item)
        {
            switch (item.DataType)
            {
                case MixDataType.DateTime:
                    return new JProperty(item.MixDatabaseColumnName, item.DateTimeValue);

                case MixDataType.Date:
                    if (!item.DateTimeValue.HasValue)
                    {
                        if (DateTime.TryParseExact(
                            item.StringValue,
                            "yyyy-MM-dd HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.RoundtripKind,
                            out DateTime date))
                        {
                            item.DateTimeValue = date;
                        }
                    }
                    return (new JProperty(item.MixDatabaseColumnName, item.DateTimeValue.Value.ToString("yyyy-MM-dd HH:mm:ss")));

                case MixDataType.Time:
                    return (new JProperty(item.MixDatabaseColumnName, item.DateTimeValue));

                case MixDataType.Double:
                    return (new JProperty(item.MixDatabaseColumnName, item.DoubleValue ?? 0));

                case MixDataType.Boolean:
                    return (new JProperty(item.MixDatabaseColumnName, item.BooleanValue));

                case MixDataType.Integer:
                    return (new JProperty(item.MixDatabaseColumnName, item.IntegerValue ?? 0));

                case MixDataType.Reference:
                    return (new JProperty(item.MixDatabaseColumnName, new JArray()));

                case MixDataType.Upload:
                    return (new JProperty(item.MixDatabaseColumnName, item.StringValue));

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
                    return (new JProperty(item.MixDatabaseColumnName, item.StringValue));
            }
        }

        public static async Task LoadAllReferenceDataAsync(
            this JObject obj,
            Guid dataContentId,
            string mixDatabaseName,
            UnitOfWorkInfo uowInfo,
            List<MixDatabaseColumn> refColumns = null)
        {
            var context = (MixCmsContext)uowInfo.ActiveDbContext;
            refColumns ??= context.MixDatabaseColumn.Where(
                   m => m.MixDatabaseName == mixDatabaseName
                    && m.DataType == MixDataType.Reference).ToList();

            foreach (var item in refColumns.Where(p => p.DataType == MixDataType.Reference))
            {
                JArray arr = await GetRelatedDataAsync(item.ReferenceId.Value, dataContentId, uowInfo);

                if (obj.ContainsKey(item.SystemName))
                {
                    obj[item.SystemName] = arr;
                }
                else
                {
                    obj.Add(new JProperty(item.SystemName, arr));
                }
            }
        }

        public static void ToModelValue(this MixDataContentValueViewModel item,
            JToken property)
        {
            if (property == null)
            {
                return;
            }

            if (item.Column.ColumnConfigurations.IsEncrypt)
            {
                var obj = property.Value<JObject>();
                item.StringValue = obj.ToString(Formatting.None);
                item.EncryptValue = obj["data"]?.ToString();
                item.EncryptKey = obj["key"]?.ToString();
            }
            else
            {
                switch (item.Column.DataType)
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
                        item.StringValue = mediaData;
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

        private static async Task<JArray> GetRelatedDataAsync(int referenceId, Guid dataContentId, UnitOfWorkInfo uowInfo)
        {
            using var assoRepo = new QueryRepository<MixCmsContext, MixDataContentAssociation, Guid>(uowInfo);
            Expression<Func<MixDataContentAssociation, bool>> predicate = model =>
                    (model.MixDatabaseId == referenceId)
                    && (model.GuidParentId == dataContentId && model.ParentType == MixDatabaseParentType.Set);
            var relatedContents = await assoRepo.GetListViewAsync<MixDataContentAssociationViewModel>(predicate);

            JArray arr = new JArray();
            foreach (var nav in relatedContents.OrderBy(v => v.Priority))
            {
                arr.Add(nav.DataContent);
            }
            return arr;
        }

    }
}
