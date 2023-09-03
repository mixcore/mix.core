using Microsoft.AspNetCore.Http;

namespace Mix.Shared.Dtos
{
    public class SearchMixDbRequestDto : SearchRequestDto
    {
        public SearchMixDbRequestDto()
        {

        }
        public SearchMixDbRequestDto(SearchRequestDto req, HttpRequest request)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            OrderBy = req.OrderBy;
            Direction = req.Direction;
            Status = req.Status;

            if (int.TryParse(request.Query[MixRequestQueryKeywords.ParentId], out int parentId))
            {
                ParentId = parentId;
            }

            if (Guid.TryParse(request.Query[MixRequestQueryKeywords.ParentId], out Guid guidParentId))
            {
                GuidParentId = guidParentId;
            }
        }

        public List<SearchQueryField> Queries { get; set; } = new();
        public int? ParentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public string ParentName { get; set; } = default;
    }
}
