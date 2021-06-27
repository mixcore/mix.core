using Mix.Heart.Entities;
using Mix.Shared.Enums;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDatabaseColumn : EntityBase<int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; }
        public string Configurations { get; set; }
        public int? ReferenceId { get; set; }

        public int MixDatabaseId { get; set; }
        public virtual MixDatabase MixDatabase { get; set; }
    }
}
