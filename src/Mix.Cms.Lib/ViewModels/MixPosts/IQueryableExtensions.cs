using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public static class IQueryableExtensions
    {
        public static IQueryable<MixPost> GetPostsByValue(
            this IQueryable<MixPost> query, 
            Expression<Func<MixDatabaseDataValue, bool>> valExp, 
            MixCmsContext context, string culture = null)
        {
           culture = culture ?? MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            var dataIds = context.MixDatabaseDataValue.Where(valExp).Select(m => m.DataId);

            // TODO: Enhance query not using AsEnummerable to get int value
            Expression<Func<MixDatabaseDataAssociation, bool>> relatedExp =
                 m => m.Specificulture == culture && dataIds.Any(d => d == m.DataId)
                            && m.ParentType == MixDatabaseParentType.Post;

            var postIds = context.MixDatabaseDataAssociation.Where(relatedExp).AsEnumerable().Select(m => int.Parse(m.ParentId));
            return query.Where(m => postIds.Any(p => p == m.Id) && m.Specificulture == culture);
        }
    }
}
