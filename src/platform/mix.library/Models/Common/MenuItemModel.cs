using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Mix.Shared.Services;

namespace Mix.Lib.Models.Common
{
    public class MenuItemModel
    {
        public JObject Obj { get; set; }

        public string Id { get; set; }

        public string Specificulture { get; set; }

        public string Title { get; set; }

        public string Uri { get; set; }

        public string Href
        {
            get
            {
                var domain = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain);
                return Uri != null && Uri.Contains(domain) ? Uri : $"{domain}{Uri}";
            }
        }

        public string Icon { get; set; }

        public MixMenuItemType Type { get; set; }

        public string Target { get; set; }

        public string Classes { get; set; }

        public string Description { get; set; }

        public string TargetId { get; set; }

        public bool IsActive { get; set; }

        public List<MenuItemModel> MenuItems { get; set; }

        public T Property<T>(string fieldName)
        {
            if (Obj != null)
            {
                var field = Obj.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
    }
}