using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPageAttributeData
    {
        public MixPageAttributeData()
        {
            MixPageAttributeValue = new HashSet<MixPageAttributeValue>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public int PageId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual MixPage MixPage { get; set; }
        public virtual ICollection<MixPageAttributeValue> MixPageAttributeValue { get; set; }
    }
}