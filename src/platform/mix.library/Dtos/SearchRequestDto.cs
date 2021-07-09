using Mix.Heart.Enums;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.Dtos
{
    public class SearchRequestDto
    {
        public string Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public SortDirection Direction { get; set; }
        public MixContentStatus? Status { get; set; }
    }
}
