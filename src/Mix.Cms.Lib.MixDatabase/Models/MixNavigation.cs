using Mix.Cms.Lib.MixDatabase.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Mix.Cms.Lib.MixDatabase.Models
{
    [MixDatabase("sys_navigation")]
    public class MixNavigation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("menu_items")]
        public List<MixMenuItem> MenuItems { get; set; } = new List<MixMenuItem>();

        [JsonProperty("actived_menu_items")]
        public List<MixMenuItem> ActivedMenuItems { get; set; } = new List<MixMenuItem>();

        [JsonProperty("actived_menu_item")]
        public MixMenuItem ActivedMenuItem { get; set; }

        public MixNavigation()
        {
        }

        public MixNavigation(JObject obj, string culture)
        {
            Id = obj["id"].Value<string>();
            Specificulture = culture;
            Title = obj["title"].Value<string>();
            Name = obj["name"].Value<string>();
            MenuItems = obj["menu_items"].ToObject<List<MixMenuItem>>();
            MenuItems.ForEach(m => m.Specificulture = Specificulture);
        }
    }
}
