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
            SearchPostDto request,
            HttpRequest httpRequest,
            Expression<Func<MixPostContent, bool>> andPredicate = null, 
            Expression<Func<MixPostContent, bool>> orPredicate = null) 
            : base(tenantId, request, httpRequest, andPredicate, orPredicate)
        {
            if (!string.IsNullOrEmpty(request.Categories))
            {
                Categories = request.Categories.Split(',', StringSplitOptions.TrimEntries);
            }
            if (!string.IsNullOrEmpty(request.Tags))
            {
                Tags = request.Tags.Split(',', StringSplitOptions.TrimEntries);
            }
        }

        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
    }
}
