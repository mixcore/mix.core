using Mix.Heart.Enums;

namespace Mix.Shared.Dtos
{
    public class SearchRequestDto
    {
        public string Keyword { get; set; } = default;
        public string Culture { get; set; } = default;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; } = default;
        public SortDirection Direction { get; set; }
        public MixContentStatus? Status { get; set; }
        public ExpressionMethod? SearchMethod { get; set; } = ExpressionMethod.Equal;
        public string Columns { get; set; } = default;
        public string SearchColumns { get; set; } = default;

    }
}
