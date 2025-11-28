namespace Mix.Database.Entities.MixDb
{
    public class MixPortalMenu : EntityBase<int>
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Svg { get; set; }
        public string Path { get; set; }
        public string Role { get; set; }
        public int TenantId { get; set; }
        public int? PortalMenuId { get; set; }
    }
}
