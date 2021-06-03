using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Lib.Helpers
{
    public class MixAssociationHelper
    {
        public static IQueryable<MixDatabaseData> GetAssociationDatas(string parentId, string culture, string databaseName, MixCmsContextV2 context)
        {
            return context.MixDatabaseDataAssociation.Where(
                        a => a.ParentId == parentId && a.Specificulture == culture && a.MixDatabaseName == databaseName)
                        .Join(context.MixDatabaseData, a => a.DataId, d => d.Id, (a, d) => new { a, d })
                        .Select(ad => ad.d);
        }

        public static IQueryable<MixModule> GetAssociationModules(int pageId, string culture, MixCmsContextV2 context)
        {
            return context.MixPageModule.Where(
                        a => a.PageId == pageId && a.Specificulture == culture)
                        .Join(context.MixModule.Where(m => m.Specificulture == culture),
                            association => association.ModuleId,
                            module => module.Id,
                            (association, module) => new { association, d = module })
                        .Select(association => association.d);
        }

        public static IQueryable<MixPost> GetAssociationModulePosts(int parentId, string culture, MixCmsContextV2 context)
        {
            return context.MixModulePost.Where(
                        a => a.ModuleId == parentId && a.Specificulture == culture)
                        .Join(context.MixPost.Where(m => m.Specificulture == culture),
                            association => association.ModuleId,
                            post => post.Id,
                            (association, post) => new { association, post })
                        .Select(association => association.post);
        }

        public static IQueryable<MixPost> GetAssociationPagePosts(int parentId, string culture, MixCmsContextV2 context)
        {
            return context.MixPagePost.Where(
                        a => a.PageId == parentId && a.Specificulture == culture)
                        .Join(context.MixPost.Where(m => m.Specificulture == culture),
                            association => association.PageId,
                            post => post.Id,
                            (association, post) => new { association, post })
                        .Select(association => association.post);
        }

        public static PaginationModel<T> ApplyPaging<T>(IQueryable<T> query,
            string orderByPropertyName, DisplayDirection direction,
            int? pageIndex, int? pageSize)
        {
            var count = query.Count();
            PaginationModel<T> result = new()
            {
                TotalItems = count,
                PageIndex = pageIndex ?? 0,
                PageSize = pageSize ?? count
            };

            if (pageSize.HasValue && pageSize.Value > 0)
            {
                result.TotalPage = (result.TotalItems / pageSize.Value) + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
            }

            IQueryable<T> sorted;
            dynamic orderBy = ReflectionHelper.GetLambda<T>(orderByPropertyName);
            switch (direction)
            {
                case DisplayDirection.Desc:
                    sorted = Queryable.OrderByDescending(query, orderBy);
                    if (pageSize.HasValue && pageSize.Value > 0)
                    {
                        sorted = sorted.Skip(result.PageIndex * pageSize.Value)
                        .Take(pageSize.Value);
                    }
                    break;

                default:
                    sorted = Queryable.OrderBy(query, orderBy);
                    if (pageSize.HasValue && pageSize.Value > 0)
                    {
                        sorted = sorted
                            .Skip(result.PageIndex * pageSize.Value)
                            .Take(pageSize.Value);
                    }

                    break;
            }
            result.Items = sorted;
            return result;
        }
    }
}
