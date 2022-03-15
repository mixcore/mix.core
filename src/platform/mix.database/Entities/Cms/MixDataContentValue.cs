using Mix.Database.Entities.Base;
using Mix.Shared.Enums;

namespace Mix.Database.Entities.Cms
{
    public class MixDataContentValue : MultiLanguageContentBase<Guid>
    {
        public string MixDatabaseColumnName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public MixEncryptType EncryptType { get; set; }

        public int MixDatabaseColumnId { get; set; }
        public int MixDatabaseId { get; set; }

        public virtual MixDataContent MixDataContent { get; set; }
        public virtual MixDatabaseColumn MixDatabaseColumn { get; set; }
    }
}
