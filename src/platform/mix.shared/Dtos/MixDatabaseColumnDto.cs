using Mix.Constant.Enums;
using Mix.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Dtos
{
    public class MixDatabaseColumnDto
    {
        #region Properties
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.String;
        public int? ReferenceId { get; set; }
        public string DefaultValue { get; set; }
        public int MixDatabaseId { get; set; }
        [JsonIgnore]
        public JObject Configurations { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; } = new();
        #endregion

        #region Constructors

        public MixDatabaseColumnDto()
        {
        }

        public MixDatabaseColumnDto(AlterColumnDto dto)
        {
            SystemName = dto.SystemName;
            DisplayName = dto.DisplayName;
            MixDatabaseName = dto.MixDatabaseName;
            DataType = dto.DataType;
            DefaultValue = dto.DefaultValue;
            ColumnConfigurations = dto.ColumnConfigurations;
        }
        #endregion
    }
}