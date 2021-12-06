using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Dtos
{
    public class SearchModuleDataDto : SearchRequestDto
    {
        public SearchModuleDataDto()
        {

        }
        public SearchModuleDataDto(SearchRequestDto req, HttpRequest request)
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
            
            if (int.TryParse(request.Query[MixRequestQueryKeywords.ModuleContentId], out int themeId))
            {
                ModuleContentId = themeId;
            }
        }

        public int? ModuleContentId { get; set; }
    }
}
