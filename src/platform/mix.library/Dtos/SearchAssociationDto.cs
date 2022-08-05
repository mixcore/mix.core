using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Dtos
{
    public class SearchAssociationDto : SearchRequestDto
    {
        public SearchAssociationDto()
        {

        }
        public SearchAssociationDto(SearchRequestDto req, HttpRequest request)
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

            if (int.TryParse(request.Query[MixRequestQueryKeywords.LeftId], out int parentId))
            {
                ParentId = parentId;
            }
            if (int.TryParse(request.Query[MixRequestQueryKeywords.RightId], out int childId))
            {
                ChildId = childId;
            }
        }

        public int? ParentId { get; set; }
        public int? ChildId { get; set; }
    }
}
