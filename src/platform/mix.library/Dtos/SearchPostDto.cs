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
            ReflectionHelper.Map(req, this);
            Categories = request.Query[MixRequestQueryKeywords.Categories];
            Tags = request.Query[MixRequestQueryKeywords.Tag];
            if (Enum.TryParse(request.Query["compareKind"], out MixCompareOperator compareKind))
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
        public MixCompareOperator CompareKind { get; set; }
        public JObject Fields { get; set; }

    }
}
