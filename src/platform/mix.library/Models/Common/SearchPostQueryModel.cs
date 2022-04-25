using Mix.Database.Entities.Cms;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using System;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public class SearchPostQueryModel : SearchQueryModel<MixPostContent, int>
    {
        public SearchPostQueryModel()
        {
        }

        public SearchPostQueryModel(
            SearchPostDto request, 
            Expression<Func<MixPostContent, bool>> andPredicate = null, 
            Expression<Func<MixPostContent, bool>> orPredicate = null) 
            : base(request, andPredicate, orPredicate)
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
