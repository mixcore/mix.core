using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixDatabaseDataValue: AuditedEntity
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int MixDatabaseColumnId { get; set; }
        public string MixDatabaseColumnName { get; set; }
        public string MixDatabaseName { get; set; }
        public string Regex { get; set; }
        public MixDataType DataType { get; set; }
        public bool? BooleanValue { get; set; }
        public string DataId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EditorValue { get; set; }
        public MixEditorType? EditorType { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}