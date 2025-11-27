using Mix.Constant.Enums;

namespace Mix.Lib.Entities.Base
{
    public abstract class EntityBase<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; } = default!;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public Guid? ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}
