namespace Mixcore.Domain.Models
{
    public sealed class MixNavigation
    {
        public string Id { get; set; }

        public string Specificulture { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public List<MenuItem> ActivedMenuItems { get; set; } = new List<MenuItem>();

        public MenuItem ActivedMenuItem { get; set; }

        public JObject Obj { get; set; }

        public MixNavigation()
        {
        }

        public MixNavigation(JObject obj, string culture)
        {
            Obj = obj;
            Id = obj["id"].Value<string>();
            Specificulture = culture;
            Title = obj["title"].Value<string>();
            Name = obj["name"].Value<string>();
            var arr = obj["menuItems"].ToObject<JArray>();
            foreach (JObject item in arr)
            {
                var menuItem = item.ToObject<MenuItem>();
                menuItem.Obj = item;
                menuItem.Specificulture = Specificulture;
                MenuItems.Add(menuItem);
            }
        }

        public T Property<T>(string fieldName)
        {
            if (Obj != null)
            {
                return Obj.Value<T>(fieldName);
            }
            else
            {
                return default;
            }
        }
    }
}
