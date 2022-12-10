using Microsoft.AspNetCore.Http;
using RepoDb.Extensions;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public sealed class SearchPostQueryModel : SearchQueryModel<MixPostContent, int>
    {
        public SearchPostQueryModel(
            HttpRequest httpRequest,
            SearchRequestDto request = null,
            int? tenantId = default,
            Expression<Func<MixPostContent, bool>> andPredicate = null,
            Expression<Func<MixPostContent, bool>> orPredicate = null)
            : base(httpRequest, request, tenantId, andPredicate, orPredicate)
        {
            ApplyMetadataFilter(httpRequest);
        }

        private void ApplyMetadataFilter(HttpRequest httpRequest)
        {
            string prefix = "md_";
            var metadata = httpRequest.Query.Keys.Where(m => m.StartsWith(prefix));
            foreach (var key in metadata)
            {
                MetadataQueries.Add(key.Replace(prefix, string.Empty), httpRequest.Query[key].ToString().Split(',', StringSplitOptions.TrimEntries));
            }
        }

        public Dictionary<string, string[]> MetadataQueries { get; set; } = new();
    }
}
