using System;

namespace Mix.Database.Entities.Base
{
    public abstract class SiteEntityUniqueNameBase<TPrimaryKey>: SiteEntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string SystemName { get; set; }
    }
}
