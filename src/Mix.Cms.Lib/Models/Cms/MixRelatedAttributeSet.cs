using Mix.Cms.Lib.Enums;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixRelatedAttributeSet
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public int ParentId { get; set; }
        public MixDatabaseType ParentType { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixDatabase IdNavigation { get; set; }
    }
}