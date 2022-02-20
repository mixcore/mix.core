namespace Mix.Lib.Models.Common
{
    public class MixNavigationModel
    {
        public string Id { get; set; }

        public string Specificulture { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();

        public List<MenuItemModel> ActivedMenuItems { get; set; } = new List<MenuItemModel>();

        public MenuItemModel ActivedMenuItem { get; set; }

        public MixNavigationModel()
        {
        }

        public MixNavigationModel(JObject obj, string culture)
        {
            Id = obj["id"].Value<string>();
            Specificulture = culture;
            Title = obj["title"].Value<string>();
            Name = obj["name"].Value<string>();
            MenuItems = obj["menu_items"].ToObject<List<MenuItemModel>>();
            MenuItems.ForEach(m => m.Specificulture = Specificulture);
        }
    }
}