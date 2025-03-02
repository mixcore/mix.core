namespace Mix.Lib.Models.Common
{
    public sealed class DataValueModel
    {
        public MixDataType DataType { get; set; } = MixDataType.String;

        public string Value { get; set; }

        public string Name { get; set; }
    }
}
