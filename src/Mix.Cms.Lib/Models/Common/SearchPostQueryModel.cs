using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Constants;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.Models.Common
{
    public class SearchPostQueryModel : SearchQueryModel
    {
        public string Category { get; set; }
        public List<string> Categories { get; set; }
        public string Tag { get; set; }
        public string PostType { get; set; }
        public JObject Query { get; set; }
        public int? PageId { get; set; }

        public SearchPostQueryModel()
        {

        }
        public SearchPostQueryModel(HttpRequest request, string culture) : base(request, culture)
        {
            PageId = request.Query.TryGetValue("pageId", out var page)
               ? int.Parse(page) : null;
            PostType = request.Query.TryGetValue("postType", out var postType)
                ? postType : string.Empty;
            Category = request.Query.TryGetValue(MixRequestQueryKeywords.Category, out var category)
                ? category : string.Empty;
            Categories = request.Query.TryGetValue(MixRequestQueryKeywords.Categories, out var categories)
                ? categories.ToString().Split(',').ToList()
                : new();
            Tag = request.Query.TryGetValue(MixRequestQueryKeywords.Tag, out var tag)
                ? tag : string.Empty;
            Query = request.Query.TryGetValue("query", out var query)
                ? JObject.Parse(query) : null;
        }
    }
}
