using Mix.Constant.Enums;
using Mix.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Dtos
{
    public class MixDbCollumnDto
    {
        #region Properties
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDbTableName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.String;
        public int? ReferenceId { get; set; }
        public string DefaultValue { get; set; }
        public int MixDbTableId { get; set; }
        [JsonIgnore]
        public JObject Configurations { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; } = new();
        #endregion

        #region Constructors

        public MixDbCollumnDto()
        {
        }

        public MixDbCollumnDto(AlterColumnDto dto)
        {
            SystemName = dto.SystemName;
            DisplayName = dto.DisplayName;
            MixDbTableName = dto.MixDbTableName;
            DataType = dto.DataType;
            DefaultValue = dto.DefaultValue;
            ColumnConfigurations = dto.ColumnConfigurations;
        }
        #endregion
    }
}