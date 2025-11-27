using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Settings
{
    public class MixGlobalSetting : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime? LastModified { get; set; }
        public string ServiceName { get; set; }
        public string SectionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int TenantId { get; set; }
        public string SystemName { get; set; }
        public string Settings { get; set; }
        public bool IsEncrypt { get; set; }
    }
}
