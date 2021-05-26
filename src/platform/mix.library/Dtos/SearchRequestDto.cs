using Mix.Heart.Enums;
using Mix.Lib.Enums;
using System;

namespace Mix.Lib.Dtos
{
    public class SearchRequestDto
    {
        public string Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public DisplayDirection Direction { get; set; }
        public MixContentStatus? Status { get; set; }
    }
}
