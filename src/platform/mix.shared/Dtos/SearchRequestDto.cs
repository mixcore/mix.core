using Mix.Heart.Enums;

namespace Mix.Shared.Dtos
{
    public class SearchRequestDto
    {
        public string Keyword { get; set; }
        public string Culture { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public SortDirection Direction { get; set; }
        public MixContentStatus? Status { get; set; }
        public ExpressionMethod? SearchMethod { get; set; }
        public string Columns { get; set; }
        public string SearchColumns { get; set; }
    }
}
