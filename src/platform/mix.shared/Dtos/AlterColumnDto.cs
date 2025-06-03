using Mix.Constant.Enums;
using Mix.Shared.Models;

namespace Mix.Shared.Dtos
{
    public class AlterColumnDto
    {
        #region Properties
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDbTableName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.String;
        public string? DefaultValue { get; set; }
        public ColumnConfigurations? ColumnConfigurations { get; set; } = new();
        public bool IsDrop { get; set; }
        #endregion

        #region Constructors

        public AlterColumnDto()
        {
        }

        public AlterColumnDto(MixDbCollumnDto model)
        {
            SystemName = model.SystemName;
            DisplayName = model.DisplayName;
            MixDbTableName = model.MixDbTableName;
            DataType = model.DataType;
            DefaultValue = model.DefaultValue;
            ColumnConfigurations = model.ColumnConfigurations;
        }
        #endregion
    }
}