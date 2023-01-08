using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixContactAddress : EntityBase<int>
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsDefault { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
        public string Note { get; set; }
        public int SysUserDataId { get; set; }
        public int MixTenantId { get; set; }
    }
}
