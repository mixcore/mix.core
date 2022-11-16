using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Expressions;

namespace Mix.Lib.Helpers
{
    public static class MixDataHelper
    {
        public static JObject ParseData(
           Guid dataContentId,
           UnitOfWorkInfo uowInfo)
        {
            var context = (MixCmsContext)uowInfo.ActiveDbContext;
            var values = context.MixDataContentValue.AsNoTracking().Where(
                m => m.ParentId == dataContentId
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

                case MixDataType.Json:
                    return (new JProperty(item.MixDatabaseColumnName, string.IsNullOrEmpty(item.StringValue) ? new JObject() : JObject.Parse(item.StringValue)));

                case MixDataType.Array:
                    return (new JProperty(item.MixDatabaseColumnName, string.IsNullOrEmpty(item.StringValue) ? new JArray() : JArray.Parse(item.StringValue)));

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
            List<MixDatabaseColumn> refColumns = null,
            CancellationToken cancellationToken = default)
        {
            var context = (MixCmsContext)uowInfo.ActiveDbContext;
            refColumns ??= context.MixDatabaseColumn.Where(
                   m => m.MixDatabaseName == mixDatabaseName
                    && m.DataType == MixDataType.Reference).ToList();

            foreach (var item in refColumns.Where(p => p.DataType == MixDataType.Reference))
            {
                JArray arr = await GetRelatedDataAsync(item.ReferenceId.Value, dataContentId, uowInfo, cancellationToken);

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

                    case MixDataType.Json:
                        item.StringValue = property.Value<JObject>().ToString(Formatting.None);
                        break;

                    case MixDataType.Array:
                        item.StringValue = property.Value<JArray>().ToString(Formatting.None);
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

        private static async Task<JArray> GetRelatedDataAsync(int referenceId, Guid dataContentId, UnitOfWorkInfo uowInfo, CancellationToken cancellationToken = default)
        {
            using var assoRepo = MixDataContentAssociationViewModel.GetRepository(uowInfo);
            Expression<Func<MixDataContentAssociation, bool>> predicate = model =>
                    model.MixDatabaseId == referenceId
                    && model.GuidParentId == dataContentId
                    && model.ParentType == MixDatabaseParentType.Set;

            var relatedContents = await assoRepo.GetListAsync(predicate, cancellationToken);

            JArray arr = new JArray();
            foreach (var nav in relatedContents.OrderBy(v => v.Priority))
            {
                arr.Add(nav.ChildDataContent.Data);
            }
            return arr;
        }

        public static async Task<T> GetAdditionalDataAsync<T>(
            UnitOfWorkInfo uow,
            MixDatabaseParentType parentType,
            string databaseName,
            Guid? guidParentId = null,
            int? intParentId = null,
            string specificulture = null,
            CancellationToken cancellationToken = default)
            where T : HaveParentSEOContentViewModelBase<MixCmsContext, MixDataContent, Guid, T>
        {
            T result = null;
            var contentRepo = new Repository<MixCmsContext, MixDataContent, Guid, T>(uow);
            var mixDbRepo = MixDatabaseViewModel.GetRepository(uow);
            var context = (MixCmsContext)uow.ActiveDbContext;

            Expression<Func<MixDataContentAssociation, bool>> predicate = m => m.MixDatabaseName == databaseName && m.ParentType == parentType;
            var mixDb = await mixDbRepo.GetSingleAsync(m => m.SystemName == databaseName, cancellationToken);
            if (mixDb != null)
            {

                predicate = predicate.AndAlsoIf(specificulture is not null, m => m.Specificulture == specificulture);
                predicate = predicate.AndAlsoIf(guidParentId.HasValue, m => m.GuidParentId == guidParentId);
                predicate = predicate.AndAlsoIf(intParentId.HasValue, m => m.IntParentId == intParentId);

                var dataId = (await context.MixDataContentAssociation.FirstOrDefaultAsync(predicate))?.DataContentId;
                if (dataId != null)
                {
                    result = await contentRepo.GetSingleAsync(m => m.Id == dataId, cancellationToken);

                }
                //result ??= new()
                //{
                //    Data = new(),
                //    Specificulture = specificulture,
                //    MixDatabaseId = mixDb.Id,
                //    MixDatabaseName = mixDb.SystemName,
                //    Status = MixContentStatus.Published,
                //    Columns = mixDb.Columns,
                //    CreatedDateTime = DateTime.UtcNow
                //};
                //result.GuidParentId = guidParentId;
                //result.IntParentId = intParentId;
                //result.ParentType = parentType;
                return result;
            }
            return default;
        }
    }
}
