using System;
using System.Collections.Generic;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSetValue
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeFieldId { get; set; }
        public string AttributeFieldName { get; set; }
        public string AttributeSetName { get; set; }
        public string Regex { get; set; }
        public MixDataType DataType { get; set; }
        public bool? BooleanValue { get; set; }
        public string DataId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }
    }
}
