using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Common
{
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
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_items")]
        public List<MenuItem> ActivedMenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_item")]
        public MenuItem ActivedMenuItem { get; set; }

        public MixNavigation()
        {
        }

        public MixNavigation(JObject obj, string culture)
        {
            Id = obj["id"].Value<string>();
            Specificulture = culture;
            Title = obj["title"].Value<string>();
            Name = obj["name"].Value<string>();
            MenuItems = obj["menu_items"].ToObject<List<MenuItem>>();
            MenuItems.ForEach(m => m.Specificulture = Specificulture);
        }
    }
}