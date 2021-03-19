using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public static class IQueryableHelper
    {
        public static IQueryable<string> GetPostIdsByValue(
            Expression<Func<MixDatabaseDataValue, bool>> valExp,
            MixCmsContext context,
            PagingDataModel pagingData,
            string culture = null)
        {
            culture = culture ?? MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);

            var dataIds = context.MixDatabaseDataValue.Where(valExp).Select(m => m.DataId);

            // TODO: Enhance query not using AsEnummerable to get int value
            Expression<Func<MixDatabaseDataAssociation, bool>> relatedExp =
                 m => m.Specificulture == culture && dataIds.Any(d => d == m.DataId)
                            && m.ParentType == MixDatabaseParentType.Post;

            var associations = context.MixDatabaseDataAssociation.Where(relatedExp);
            var parentIds = associations.Select(m => m.ParentId);

            if (pagingData.OrderBy.StartsWith("additionalData."))
            {
                parentIds = SortParentIds(parentIds, context, pagingData, culture);
            }
            return parentIds
                .Skip(pagingData.PageIndex * pagingData.PageSize)
                .Take(pagingData.PageSize);
        }

        private static IQueryable<string> SortParentIds(
            IQueryable<string> parentIds,
            MixCmsContext context,
            PagingDataModel pagingData,
            string culture)
        {
            string orderCol = pagingData.OrderBy.Split('.')[1];
            var sortQuery = context.MixDatabaseDataValue
                .Where(
                v => v.Specificulture == culture
                    && v.MixDatabaseName == MixDatabaseNames.ADDITIONAL_FIELD_POST
                    && v.MixDatabaseColumnName == orderCol);

            switch (pagingData.Direction)
            {
                case Heart.Enums.MixHeartEnums.DisplayDirection.Asc:
                    return (from association in
                            context.MixDatabaseDataAssociation.Where(
                                m => m.ParentType == MixDatabaseParentType.Post
                            && m.MixDatabaseName == MixDatabaseNames.ADDITIONAL_FIELD_POST
                            && parentIds.Any(p => p == m.ParentId))
                            join value in sortQuery
                            on association.DataId equals value.DataId
                            into navigations
                            from nav in navigations.DefaultIfEmpty()
                            orderby nav.StringValue
                            select association.ParentId);
                case Heart.Enums.MixHeartEnums.DisplayDirection.Desc:
                default:
                    return (from association in
                                   context.MixDatabaseDataAssociation.Where(
                                       m => m.ParentType == MixDatabaseParentType.Post
                                   && m.MixDatabaseName == MixDatabaseNames.ADDITIONAL_FIELD_POST
                                   && parentIds.Any(p => p == m.ParentId))
                            join value in sortQuery
                            on association.DataId equals value.DataId
                            into navigations
                            from nav in navigations.DefaultIfEmpty()
                            orderby nav.StringValue descending
                            select association.ParentId);
            };
        }
    }
}
