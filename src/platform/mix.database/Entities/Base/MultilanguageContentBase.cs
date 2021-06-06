using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Entity;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageContentBase<TPrimaryKey>: EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public int MixCultureId { get; set; }
        public virtual MixCulture MixCulture { get; set; }
    }
}
