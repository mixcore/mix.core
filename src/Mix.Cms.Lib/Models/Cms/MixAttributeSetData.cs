using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSetData
    {
        public MixAttributeSetData()
        {
            MixRelatedAttributeData = new HashSet<MixRelatedAttributeData>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
        public virtual ICollection<MixRelatedAttributeData> MixRelatedAttributeData { get; set; }
    }
}