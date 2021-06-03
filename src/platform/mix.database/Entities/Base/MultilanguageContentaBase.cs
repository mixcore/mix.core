using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageContentaBase<TPrimaryKey, TParentType>: EntityBase<TPrimaryKey>
    {
        public string Specificulture { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public TParentType Parent { get; set; }
    }
}
