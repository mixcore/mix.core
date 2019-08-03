using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSet
    {
        public MixAttributeSet()
        {
            MixArticleAttributeSet = new HashSet<MixPostAttributeSet>();
            MixAttributeField = new HashSet<MixAttributeField>();
            MixModuleAttributeData = new HashSet<MixModuleAttributeData>();
            MixModuleAttributeSet = new HashSet<MixModuleAttributeSet>();
            MixPageAttributeSet = new HashSet<MixPageAttributeSet>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual ICollection<MixPostAttributeSet> MixArticleAttributeSet { get; set; }
        public virtual ICollection<MixAttributeField> MixAttributeField { get; set; }
        public virtual ICollection<MixModuleAttributeData> MixModuleAttributeData { get; set; }
        public virtual ICollection<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public virtual ICollection<MixPageAttributeSet> MixPageAttributeSet { get; set; }
    }
}
