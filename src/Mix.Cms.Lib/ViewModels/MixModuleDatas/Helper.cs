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
                    case MixEnums.MixDataType.Reference:
                        //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                        val["value"] = new JArray();
                        break;
                    case MixEnums.MixDataType.Upload:
                        string fullUrl = val["value"].ToString().TrimStart('/');

                        fullUrl = fullUrl.IndexOf("http") >= 0 ? fullUrl : $"{MixService.GetConfig<string>("Domain")}/{fullUrl}";
                        val["value"] = fullUrl;
                        break;
                    case MixEnums.MixDataType.DateTime:
                    case MixEnums.MixDataType.Date:
                    case MixEnums.MixDataType.Time:
                    case MixEnums.MixDataType.Double:
                    case MixEnums.MixDataType.Boolean:
                    case MixEnums.MixDataType.Integer:
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
                        break;
                }
                result = val;
            }
            return result;
        }
    }
}
