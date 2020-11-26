using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSet
    {
        public MixAttributeSet()
        {
            MixAttributeFieldAttributeSet = new HashSet<MixAttributeField>();
            MixAttributeFieldReference = new HashSet<MixAttributeField>();
            MixAttributeSetData = new HashSet<MixAttributeSetData>();
            MixAttributeSetReference = new HashSet<MixAttributeSetReference>();
            MixRelatedAttributeSet = new HashSet<MixRelatedAttributeSet>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormTemplate { get; set; }
        public string EdmTemplate { get; set; }
        public string EdmSubject { get; set; }
        public string EdmFrom { get; set; }
        public bool? EdmAutoSend { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual ICollection<MixAttributeField> MixAttributeFieldAttributeSet { get; set; }
        public virtual ICollection<MixAttributeField> MixAttributeFieldReference { get; set; }
        public virtual ICollection<MixAttributeSetData> MixAttributeSetData { get; set; }
        public virtual ICollection<MixAttributeSetReference> MixAttributeSetReference { get; set; }
        public virtual ICollection<MixRelatedAttributeSet> MixRelatedAttributeSet { get; set; }
    }
}
