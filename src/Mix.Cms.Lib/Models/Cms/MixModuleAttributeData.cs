using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixModuleAttributeData
    {
        public MixModuleAttributeData()
        {
            MixModuleAttributeValue = new HashSet<MixModuleAttributeValue>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int ModuleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Fields { get; set; }
        public int Priority { get; set; }
        public int? ProductId { get; set; }
        public int Status { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string Value { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual ICollection<MixModuleAttributeValue> MixModuleAttributeValue { get; set; }
    }
}
