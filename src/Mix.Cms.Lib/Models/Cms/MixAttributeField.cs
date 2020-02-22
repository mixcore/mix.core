using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeField
    {
        public MixAttributeField()
        {
            MixModuleAttributeValue = new HashSet<MixModuleAttributeValue>();
            MixPageAttributeValue = new HashSet<MixPageAttributeValue>();
            MixPostAttributeValue = new HashSet<MixPostAttributeValue>();
        }

        public int Id { get; set; }
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public string Regex { get; set; }
        public string Title { get; set; }
        public int DataType { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public string Options { get; set; }
        public bool IsRequire { get; set; }
        public bool IsEncrypt { get; set; }
        public bool IsSelect { get; set; }
        public bool IsUnique { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public int? ReferenceId { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
        public virtual MixAttributeSet Reference { get; set; }
        public virtual ICollection<MixModuleAttributeValue> MixModuleAttributeValue { get; set; }
        public virtual ICollection<MixPageAttributeValue> MixPageAttributeValue { get; set; }
        public virtual ICollection<MixPostAttributeValue> MixPostAttributeValue { get; set; }
    }
}