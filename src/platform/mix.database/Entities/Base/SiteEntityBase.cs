using Mix.Database.Entities.Cms;
using Mix.Heart.Entities;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class SiteEntityBase<TPrimaryKey>: EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string DisplayName { get; set; }
        public virtual string Description { get; set; }

        public int MixSiteId { get; set; }
        public MixSite MixSite { get; set; }
    }
}
