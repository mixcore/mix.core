namespace Mix.Database.Entities.MixDb
{
    public class MixNavigation : EntityBase<int>
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }
    }
}