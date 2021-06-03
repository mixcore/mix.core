using Mix.Shared.Constants;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using Mix.Shared.Services;

namespace Mix.Lib.Helpers
{
    public class MixCmsHelper
    {
        public static string GetAssetFolder(string culture = null)
        {
            culture ??= MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            return $"{MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain)}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/assets";
        }

        public static string GetUploadFolder(string culture = null)
        {
            culture ??= MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            return $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/uploads/" +
                $"{DateTime.UtcNow.ToString(MixConstants.CONST_UPLOAD_FOLDER_DATE_FORMAT)}";
        }

        public static string GetTemplateFolder(string culture)
        {
            return $"/{MixFolders.TemplatesFolder}/{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}";
        }

        public static T Property<T>(JObject obj, string fieldName)
        {
            if (obj != null && obj.ContainsKey(fieldName) && obj[fieldName] != null)
            {
                return obj.Value<T>(fieldName);
            }
            else
            {
                return default;
            }
        }

        public static string FormatPrice(double? price, string oldPrice = "0")
        {
            string strPrice = price?.ToString();
            if (string.IsNullOrEmpty(strPrice))
            {
                return "0";
            }
            string s1 = strPrice.Replace(",", string.Empty);
            if (CheckIsPrice(s1))
            {
                Regex rgx = new("(\\d+)(\\d{3})");
                while (rgx.IsMatch(s1))
                {
                    s1 = rgx.Replace(s1, "$1" + "," + "$2");
                }
                return s1;
            }
            return oldPrice;
        }

        public static bool CheckIsPrice(string number)
        {
            if (number == null)
            {
                return false;
            }
            number = number.Replace(",", "");
            return double.TryParse(number, out _);
        }

        public static double ReversePrice(string formatedPrice)
        {
            try
            {
                if (string.IsNullOrEmpty(formatedPrice))
                {
                    return 0;
                }
                return double.Parse(formatedPrice.Replace(",", string.Empty));
            }
            catch
            {
                return 0;
            }
        }
    }
}
