using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostAttributeData
    {
        public MixPostAttributeData()
        {
            MixPostAttributeValue = new HashSet<MixPostAttributeValue>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPost MixPost { get; set; }
        public virtual ICollection<MixPostAttributeValue> MixPostAttributeValue { get; set; }
    }
}