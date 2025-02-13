using Microsoft.AspNetCore.Http;

namespace Mix.Shared.Dtos
{
    public class FilterContentRequestDto : SearchRequestDto
    {
        public FilterContentRequestDto()
        {

        }
        public FilterContentRequestDto(SearchRequestDto req)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            SortBy = req.SortBy;
            Direction = req.Direction;
            Status = req.Status;
        }
        public string MixDatabaseName { get; set; }
        public string MetadataAnd { get; set; }
        public string MetadataOr { get; set; }
        public List<MixQueryField> Queries { get; set; } = new();
        public List<MixQueryField> MetadataQueries { get; set; } = new();
    }
}
