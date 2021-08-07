using Microsoft.EntityFrameworkCore;
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
            string culture = null,
            string postType = MixDatabaseNames.ADDITIONAL_COLUMN_POST)
        {
            culture = culture ?? MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);

            var dataIds = context.MixDatabaseDataValue.Where(valExp).Select(m => m.DataId);

            // TODO: Enhance query not using AsEnummerable to get int value
            Expression<Func<MixDatabaseDataAssociation, bool>> relatedExp =
                 m => m.Specificulture == culture && dataIds.Any(d => d == m.DataId)
                            && m.ParentType == MixDatabaseParentType.Post;

            var associations = context.MixDatabaseDataAssociation.Where(relatedExp);
            var parentIds = associations.Select(m => m.ParentId);


            return parentIds;
        }

        public static IEnumerable<string> SortParentIds(
            IEnumerable<string> parentIds,
            MixCmsContext context,
            PagingRequest pagingData,
            string culture,
            string postType)
        {
            if (!pagingData.OrderBy.StartsWith("additionalData."))
            {
                return parentIds;
            }

            string orderCol = pagingData.OrderBy.Split('.')[1];
            var sortQuery = context.MixDatabaseDataValue
                .Where(
                v => v.Specificulture == culture
                    && v.MixDatabaseName == postType
                    && v.MixDatabaseColumnName == orderCol);
            IEnumerable<string> result;
            switch (pagingData.Direction)
            {
                case Heart.Enums.DisplayDirection.Asc:
                    result = (from association in
                            context.MixDatabaseDataAssociation.Where(
                                m => m.ParentType == MixDatabaseParentType.Post
                            && m.MixDatabaseName == postType
                            && parentIds.Any(p => p == m.ParentId))
                              join value in sortQuery
                              on association.DataId equals value.DataId
                              into navigations
                              from nav in navigations.DefaultIfEmpty()
                              orderby nav.IntegerValue
                              select association.ParentId);
                    break;
                case Heart.Enums.DisplayDirection.Desc:
                default:
                    result = (from association in
                                   context.MixDatabaseDataAssociation.Where(
                                       m => m.ParentType == MixDatabaseParentType.Post
                                   && m.MixDatabaseName == postType
                                   && parentIds.Any(p => p == m.ParentId))
                              join value in sortQuery
                              on association.DataId equals value.DataId
                              into navigations
                              from nav in navigations.DefaultIfEmpty()
                              orderby nav.IntegerValue descending
                              select association.ParentId);
                    break;
            };
            return result;
        }

        public static IQueryable<MixPost> GetSortedPost<T>(
            IEnumerable<T> postIds,
            MixCmsContext context,
            SearchPostQueryModel searchPostData)
        {
            string orderColName = searchPostData.PagingData.OrderBy.Split('.')[1];
            var colType = context.MixDatabaseColumn.FirstOrDefault(c => c.Name == orderColName && c.MixDatabaseName == searchPostData.PostType).DataType;
            string orderCol = GetColumn(colType);
            string sql = $@"
                SELECT p.* FROM mix_post p
                INNER JOIN (
                SELECT m.ParentId, m.Specificulture, t.{orderCol}
                FROM mix_database_data_association AS m
                LEFT JOIN (
                    SELECT m0.DataId, m0.{orderCol}
                    FROM mix_database_data_value AS m0
                    WHERE ((m0.Specificulture = '{searchPostData.Specificulture}') AND (m0.MixDatabaseName = '{searchPostData.PostType}')) 
                        AND (m0.MixDatabaseColumnName = '{orderColName}')
                ) AS t ON m.DataId = t.DataId
                WHERE ((m.ParentType = 'Post') AND (m.MixDatabaseName = '{searchPostData.PostType}')) 
                    AND m.ParentId IN ({string.Join(",", postIds)})) AS n ON p.Id = n.ParentId AND p.Specificulture = n.Specificulture
                ORDER BY n.{orderCol} {searchPostData.PagingData.Direction}
                LIMIT {searchPostData.PagingData.PageSize} OFFSET {searchPostData.PagingData.PageIndex * searchPostData.PagingData.PageSize}";
            return context.MixPost.FromSqlRaw(sql);
        }

        private static string GetColumn(MixDataType colType)
        {
            switch (colType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return "DateTimeValue";
                case MixDataType.Double:
                    return "DoubleValue";
                case MixDataType.Boolean:
                    return "BooleanValue";
                case MixDataType.Integer:
                    return "IntegerValue";
                case MixDataType.Reference:
                case MixDataType.Upload:
                case MixDataType.Custom:
                case MixDataType.Duration:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.Html:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                default:
                    return "StringValue";
            }
        }
    }
}
