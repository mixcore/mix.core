using Mix.Heart.Enums;
using Mix.Heart.Model;

namespace Mix.Shared.Dtos
{
    public class SearchRequestDto
    {
        public bool LoadNestedData { get; set; }
        public string? MixDatabaseName { get; set; } = default;
        public string? Keyword { get; set; } = default;
        public string? Culture { get; set; } = default;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; } = 0;
        public int? PageSize { get; set; }
        public string? PagingState { get; set; }
        public string? SortBy { get; set; }
        public string? OrderBy { get; set; }
        public MixConjunction? Conjunction { get; set; } = MixConjunction.And;
        public SortDirection Direction { get; set; } = SortDirection.Desc;
        public MixContentStatus? Status { get; set; }
        public MixCompareOperator? SearchMethod { get; set; } = MixCompareOperator.Equal;
        public string? Columns { get; set; }
        public string? SearchColumns { get; set; }
        public object? After { get; set; }
        public object? Before { get; set; }
        public List<MixQueryField>? Queries { get; set; }
        public List<MixSortByColumn>? SortByFields { get; set; }
    }
}
