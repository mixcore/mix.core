using Mix.Shared.Models;

namespace Mix.Portal.Domain.Models
{
    public sealed class ModuleColumnModel
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.Text;

        public string DefaultValue { get; set; }
        public int Priority { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; }
    }
}
