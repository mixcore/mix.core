using Mix.Lib.Enums;

namespace Mix.Lib.Models.Common
{
    public class DataValueModel
    {
        public MixDataType DataType { get; set; } = MixDataType.Text;

        public string Value { get; set; }

        public string Name { get; set; }
    }
}
