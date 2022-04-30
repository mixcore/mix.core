using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Models.Common
{
    public class SearchContentValueModel
    {
        public string ColumnName { get; set; }
        public object Value { get; set; }
        public ExpressionMethod SearchMethod { get; set; }
    }
}
