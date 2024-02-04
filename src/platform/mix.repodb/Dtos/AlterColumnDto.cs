using Mix.Constant.Enums;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.RepoDb.Dtos
{
    public class AlterColumnDto
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; }
        public int? ReferenceId { get; set; }

        public string? DefaultValue { get; set; }
        public int MixDatabaseId { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; }
    }
}
