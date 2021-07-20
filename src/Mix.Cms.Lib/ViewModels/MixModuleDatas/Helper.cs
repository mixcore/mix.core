using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Lib.ViewModels.MixModuleDatas
{
    public class Helper
    {
        public static JToken ParseValue(JObject JItem, ApiModuleDataValueViewModel item)
        {
            JToken result = null;
            bool isHaveValue = JItem.TryGetValue(item.Name, out JToken val);
            if (isHaveValue)
            {
                switch (item.DataType)
                {
                    case MixDataType.Reference:
                        //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                        val["value"] = new JArray();
                        break;

                    case MixDataType.Upload:
                        string fullUrl = val["value"].ToString().TrimStart('/');

                        fullUrl = fullUrl.IndexOf("http") >= 0 
                            ? fullUrl 
                            : $"{MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain.TrimEnd('/'))}/{fullUrl.TrimStart('/')}";
                        val["value"] = fullUrl;
                        break;

                    case MixDataType.DateTime:
                    case MixDataType.Date:
                    case MixDataType.Time:
                    case MixDataType.Double:
                    case MixDataType.Boolean:
                    case MixDataType.Integer:
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
                        break;
                }
                result = val;
            }
            return result;
        }
    }
}