using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPageAttributeValue
    {
        public string Id { get; set; }
        public string DataId { get; set; }
        public int AttributeFieldId { get; set; }
        public string AttributeName { get; set; }
        public string Specificulture { get; set; }
        public int PageId { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public bool? BooleanValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual MixAttributeField AttributeField { get; set; }
        public virtual MixPageAttributeData Data { get; set; }
    }
}