using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Dtos
{
    public class SearchTemplateDto : SearchRequestDto
    {
        public SearchTemplateDto()
        {

        }
        public SearchTemplateDto(SearchRequestDto req, HttpRequest request)
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
            
            if (int.TryParse(request.Query[MixRequestQueryKeywords.ThemeId], out int themeId))
            {
                ThemeId = themeId;
            }
            if (Enum.TryParse(request.Query["folderType"], out MixTemplateFolderType folder))
            {
                 Folder = folder;
            }
        }

        public int? ThemeId { get; set; }
        public MixTemplateFolderType? Folder { get; set; }
    }
}
