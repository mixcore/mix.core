using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public sealed class MixUserPermission : EntityBase<int>
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
        public string Description { get; set; }
        public int MixTenantId { get; set; }
    }
}
