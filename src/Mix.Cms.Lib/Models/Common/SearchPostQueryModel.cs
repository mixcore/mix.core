using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Cms.Lib.Models.Common
{
    public class SearchPostQueryModel: SearchQueryModel
    {
        public string Category { get; set; }
        public string Tag { get; set; }
        public string PostType { get; set; }
        public JObject Query { get; set; }

        public SearchPostQueryModel()
        {

        }
        public SearchPostQueryModel(HttpRequest request, string culture): base(request, culture)
        {
            PostType = request.Query.TryGetValue("postType", out var postType)
                ? postType : string.Empty;
            Category = request.Query.TryGetValue(MixRequestQueryKeywords.Category, out var category)
                ? category: string.Empty;
            Tag = request.Query.TryGetValue(MixRequestQueryKeywords.Tag, out var tag)
                ? tag : string.Empty;
            Query= request.Query.TryGetValue("query", out var query)
                ? JObject.Parse(query): null;
        }
    }
}
