using Mix.Heart.Entities;

namespace Mix.Services.Permission.Domain.Entities
{
    public sealed class MixPermission : EntityBase<int>
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public int MixTenantId { get; set; }
    }
}
