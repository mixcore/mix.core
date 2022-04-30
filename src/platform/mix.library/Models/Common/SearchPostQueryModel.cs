using Microsoft.AspNetCore.Http;
using Mix.Lib.Dtos;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public class SearchPostQueryModel : SearchQueryModel<MixPostContent, int>
    {
        public SearchPostQueryModel(int tenantId) : base(tenantId)
        {
        }

        public SearchPostQueryModel(
            int tenantId,
            SearchRequestDto request,
            HttpRequest httpRequest,
            Expression<Func<MixPostContent, bool>> andPredicate = null, 
            Expression<Func<MixPostContent, bool>> orPredicate = null) 
            : base(tenantId, request, httpRequest, andPredicate, orPredicate)
        {
            var strCategories = httpRequest.Query[MixRequestQueryKeywords.Categories].ToString();
            var strTags = httpRequest.Query[MixRequestQueryKeywords.Tag].ToString();

            if (!string.IsNullOrEmpty(strCategories))
            {
                Categories = strCategories.Split(',', StringSplitOptions.TrimEntries);
            }
            if (!string.IsNullOrEmpty(strTags))
            {
                Tags = strTags.Split(',', StringSplitOptions.TrimEntries);
            }
        }

        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
    }
}
