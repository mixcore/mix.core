using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public sealed class MixPermission : EntityBase<int>
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public int MixTenantId { get; set; }
    }

    public sealed class Metadata
    {
        public string Description { get; set; }
    }
}
