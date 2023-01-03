using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixPortalMenu : EntityBase<int>
    {
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Svg { get; set; }
        public string? Path { get; set; }
        public string? Role { get; set; }
        public int MixTenantId { get; set; }
        public int? PortalMenuId { get; set; }
    }
}
