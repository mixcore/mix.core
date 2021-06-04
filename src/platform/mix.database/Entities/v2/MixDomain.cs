using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDomain : SiteEntityBase<int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
