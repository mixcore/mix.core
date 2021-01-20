using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels;
using Mix.Heart.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Mix.Cms.Lib.Extensions
{
    public static class MixAttributeValueModelExtensions
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

        public static void ToModelValue(this ViewModels.MixAttributeSetValues.UpdateViewModel item, JToken property)
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
                            ViewModels.MixMedias.UpdateViewModel media = new ViewModels.MixMedias.UpdateViewModel()
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
    }
}
