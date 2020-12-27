using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeField
    {
        public int Id { get; set; }
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public string Configurations { get; set; }
        public string Regex { get; set; }
        public string Title { get; set; }
        public MixEnums.MixDataType DataType { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public string Options { get; set; }
        public bool IsRequire { get; set; }
        public bool IsEncrypt { get; set; }
        public bool IsMultiple { get; set; }
        public bool IsSelect { get; set; }
        public bool IsUnique { get; set; }
        public int? ReferenceId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }
    }
}
