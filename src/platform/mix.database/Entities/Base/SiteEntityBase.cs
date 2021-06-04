using Mix.Database.Entities.Cms.v2;

namespace Mix.Database.Entities.Base
{
    public abstract class SiteEntityBase<TPrimaryKey>: EntityBase<TPrimaryKey>
    {
        public int MixSiteId { get; set; }
        public MixSite MixSite { get; set; }
    }
}
