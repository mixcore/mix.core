using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Constants;
using Mix.Lib.Enums;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using Mix.Heart.Models;
using Mix.Lib.ViewModels.Cms;
using Mix.Heart.Extensions;

namespace Mix.Lib.Extensions
{
    public static class MixDatabaseDataValueModelExtensions
    {
        public static JProperty ToJProperty(this MixDatabaseDataValue item)
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
                    string domain = MixService.GetConfig<string>(MixAppSettingKeywords.Domain);
                    string url = !string.IsNullOrEmpty(item.StringValue)
                   ? !item.StringValue.Contains(domain)
                        ? $"{MixService.GetConfig<string>(MixAppSettingKeywords.Domain)}{item.StringValue}"
                        : item.StringValue
                   : null;
                    return (new JProperty(item.MixDatabaseColumnName, url));

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

        public static void ToModelValue<T, TColumn>(this MixDatabaseDataValuesBase<T, TColumn> item,
            JToken property,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
            where T : ViewModelBase<MixCmsContext, MixDatabaseDataValue, T>
            where TColumn : ViewModelBase<MixCmsContext, MixDatabaseColumn, TColumn>
        {
            if (property == null)
            {
                return;
            }

            if (item.Column.IsEncrypt)
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
                        if (mediaData.IsBase64())
                        {
                            var media = new MixMediaViewModel()
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
                            var saveMedia = media.SaveModel(true, _context, _transaction);
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
    }
}