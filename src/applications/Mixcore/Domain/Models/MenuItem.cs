namespace Mixcore.Domain.Models
{
    public class MenuItem
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
                var domain = "/";
                return Uri != null && Uri.Contains(domain[0]) ? Uri : $"{domain}{Uri}";
            }
        }

        public string Icon { get; set; }

        public MixMenuItemType Type { get; set; }

        public string Target { get; set; }

        public string Classes { get; set; }

        public string Description { get; set; }

        public string TargetId { get; set; }

        public bool IsActive { get; set; }

        public List<MenuItem> MenuItems { get; set; }

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
