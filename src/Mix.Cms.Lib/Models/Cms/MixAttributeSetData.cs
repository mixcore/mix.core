using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSetData
    {
        public MixAttributeSetData()
        {
            InverseParent = new HashSet<MixAttributeSetData>();
            MixAttributeSetValue = new HashSet<MixAttributeSetValue>();
        }

        public string Id { get; set; }
        public int AttributeSetId { get; set; }
        public string Specificulture { get; set; }
        public string ParentId { get; set; }
        public int? ParentType { get; set; }
        public int ModuleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
        public virtual MixAttributeSetData Parent { get; set; }
        public virtual ICollection<MixAttributeSetData> InverseParent { get; set; }
        public virtual ICollection<MixAttributeSetValue> MixAttributeSetValue { get; set; }
    }
}
