using Mix.Heart.Entities;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class AssociationBase<TPrimaryKey>: EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public TPrimaryKey LeftId { get; set; }
        public TPrimaryKey RightId { get; set; }
    }
}
