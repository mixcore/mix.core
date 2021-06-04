using Mix.Database.Entities.Cms.v2;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageContentaBase<TPrimaryKey>: EntityBase<TPrimaryKey>
    {
        public string Specificulture { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public virtual MixCulture MixCulture { get; set; }
    }
}
