using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Dtos
{
    public class SearchAssociationDto: SearchRequestDto
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
            
            if (int.TryParse(request.Query[MixRequestQueryKeywords.LeftId], out int leftId))
            {
                LeftId = leftId;
            }
            if (int.TryParse(request.Query[MixRequestQueryKeywords.RightId], out int rightId))
            {
                RightId = rightId;
            }
        }

        public int? LeftId { get; set; }
        public int? RightId { get; set; }
    }
}
