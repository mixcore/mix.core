namespace Mix.Lib.Models.Common
{
    public sealed class SearchContentValueModel
    {
        public string ColumnName { get; set; }
        public object Value { get; set; }
        public ExpressionMethod SearchMethod { get; set; }
    }
}
