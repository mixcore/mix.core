using Mix.Database.Entities.Cms;

namespace Mix.Service.Models
{
    public class MixTenantSystemModel
    {
        public int Id { get; set; }

        public string PrimaryDomain { get; set; }

        public string SystemName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<MixDomain> Domains { get; set; }

        public List<MixCulture> Cultures { get; set; } = new();

        public TenantConfigurationModel Configurations { get; set; }
    }
}
