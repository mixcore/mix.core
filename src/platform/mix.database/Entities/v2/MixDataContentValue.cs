using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDataContentValue : EntityBase<int>
    {
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
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }

        public virtual MixDataContent MixDataContent { get; set; }
    }
}
