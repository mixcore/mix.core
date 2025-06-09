

using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms
{
    public class MixDbColumn : EntityBase<int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDbTableName { get; set; }
        public MixDataType DataType { get; set; }
        public JObject Configurations { get; set; }
        public int? ReferenceId { get; set; }

        public string DefaultValue { get; set; }

        public int MixDbTableId { get; set; }
    }
}
