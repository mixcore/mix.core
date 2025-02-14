namespace Mix.Database.Entities.MixDb
{
    public class MixMenuItem : EntityBase<int>
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public string Target { get; set; }
        public string TargetId { get; set; }
        public string Classes { get; set; }
        public string Description { get; set; }
        public string Alt { get; set; }
        public string Hreflang { get; set; }
        public string Group { get; set; }
        public string Image { get; set; }
        public int TenantId { get; set; }
    }
}