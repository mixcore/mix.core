using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Common
{
    public class MenuItem
    {
        [JsonIgnore]
        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("href")]
        public string Href
        {
            get
            {
                var domain = MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain);
                return Uri != null && Uri.Contains(domain) ? Uri : $"{domain}{Uri}";
            }
        }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("type")]
        public MixMenuItemType Type { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("classes")]
        public string Classes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("target_id")]
        public string TargetId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("menu_items")]
        public List<MenuItem> MenuItems { get; set; }

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