using Mix.Heart.Enums;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.Entities.Base
{
    public abstract class EntityBase<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}
