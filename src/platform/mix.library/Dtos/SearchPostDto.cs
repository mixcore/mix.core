using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Dtos
{
    public class SearchPostDto : SearchRequestDto
    {
        public SearchPostDto()
        {

        }
        public SearchPostDto(SearchRequestDto req, HttpRequest request)
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
            
            if (Enum.TryParse(request.Query["compareKind"], out MixCompareOperatorKind compareKind))
            {
                CompareKind = compareKind;
            }
            if (!string.IsNullOrEmpty(request.Query["fields"]))
            {
                Fields = JObject.Parse(request.Query["fields"]);
            }

        }
        public string Categories { get; set; }
        public string Tags { get; set; }
        public MixCompareOperatorKind CompareKind { get; set; }
        public JObject Fields { get; set; }

    }
}
