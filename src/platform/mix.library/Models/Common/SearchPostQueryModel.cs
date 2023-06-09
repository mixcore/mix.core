using Microsoft.AspNetCore.Http;
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
                string[] arr = key.Split('_');
                if (arr.Length == 3)
                {
                    string seoContent = arr[2];
                    var contents = httpRequest.Query[key].ToString().TrimEnd(',').Split(',', StringSplitOptions.TrimEntries);
                    
                    MetadataQueries.Add(new()
                    {
                        FieldName = seoContent,
                        Value = contents,
                        CompareOperator = MixCompareOperator.InRange,
                        IsRequired = arr[1] == "and"
                    });
                }
            }
        }
        public List<SearchQueryField> MetadataQueries { get; set; } = new();
    }
}
