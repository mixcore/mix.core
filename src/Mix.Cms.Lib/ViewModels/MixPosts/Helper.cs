using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class Helper
    {
        public static async Task<RepositoryResponse<PaginationModel<TView>>> SearchPosts<TView>(
            SearchPostQueryModel searchPostData,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {

                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                valPredicate = valPredicate.AndAlsoIf(
                    !string.IsNullOrEmpty(searchPostData.Category),
                     Expressions.GetMetaExpression(MixDatabaseNames.SYSTEM_CATEGORY, searchPostData.Category, searchPostData.Specificulture));
                valPredicate = valPredicate.AndAlsoIf(
                    !string.IsNullOrEmpty(searchPostData.Tag),
                     Expressions.GetMetaExpression(MixDatabaseNames.SYSTEM_TAG, searchPostData.Tag, searchPostData.Specificulture));

                if (valPredicate != null)
                {
                    return await SearchPostByValue<TView>(valPredicate, searchPostData.PagingData, searchPostData.PostType, searchPostData.Specificulture, context, transaction);
                }
                else
                {
                    Expression<Func<MixPost, bool>> predicate = BuildPostExpression(searchPostData);
                    if (searchPostData.PagingData.OrderBy.StartsWith("additionalData."))
                    {
                        var total = context.MixPost.Count(predicate);
                        var allPostIds = context.MixPost.Where(predicate)
                            .AsEnumerable()
                            .Select(m => m.Id);

                        var posts = IQueryableHelper.GetSortedPost(allPostIds, context, searchPostData).ToList();
                        return new RepositoryResponse<PaginationModel<TView>>()
                        {
                            IsSucceed = true,
                            Data = new PaginationModel<TView>()
                            {
                                Items = DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetCachedData(posts, context, transaction),
                                PageSize = searchPostData.PagingData.PageSize,
                                PageIndex = searchPostData.PagingData.PageIndex
                            }
                        };
                    }
                    return await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                        predicate,
                        searchPostData.PagingData.OrderBy,
                        searchPostData.PagingData.Direction,
                        searchPostData.PagingData.PageSize,
                        searchPostData.PagingData.PageIndex,
                        null, null,
                        context,
                        transaction);
                }
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private static async Task<RepositoryResponse<PaginationModel<TView>>> SearchPostByValue<TView>(
             Expression<Func<MixDatabaseDataValue, bool>> valPredicate,
            PagingRequest pagingData,
            string postType,
            string specificulture,
            MixCmsContext context,
            IDbContextTransaction transaction)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            var allPostIds = IQueryableHelper.GetPostIdsByValue(
                        valPredicate,
                        context,
                        specificulture,
                        postType);
            var resultIds = IQueryableHelper.SortParentIds(
                allPostIds.Skip(pagingData.PageIndex * pagingData.PageSize)
                            .Take(pagingData.PageSize),
                context,
                pagingData,
                specificulture,
                postType)
                .AsEnumerable()
                .Select(p => int.Parse(p))
                .ToList();

            var getPosts = (await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                        m => resultIds.Any(p => p == m.Id) && m.Specificulture == specificulture,
                        context,
                        transaction));
            var items = getPosts.Data.OrderBy(
                m => resultIds.IndexOf((int)ReflectionHelper.GetPropertyValue(m, "Id"))).ToList();
            var total = allPostIds.Count();
            var result = new RepositoryResponse<PaginationModel<TView>>()
            {
                IsSucceed = true,
                Data = new PaginationModel<TView>()
                {
                    Items = items,
                    PageIndex = pagingData.PageIndex,
                    PageSize = pagingData.PageSize,
                    TotalItems = total,
                    TotalPage = (int)Math.Ceiling((double)total / pagingData.PageSize)
                }
            };
            return result;
        }

        private static Expression<Func<MixPost, bool>> BuildPostExpression(SearchPostQueryModel searchPostData)
        {
            Expression<Func<MixPost, bool>> predicate = model => model.Specificulture == searchPostData.Specificulture;
            predicate = predicate.AndAlsoIf(searchPostData.Status.HasValue, model => model.Status == searchPostData.Status.Value);
            predicate = predicate.AndAlsoIf(searchPostData.FromDate.HasValue, model => model.CreatedDateTime >= searchPostData.FromDate.Value);
            predicate = predicate.AndAlsoIf(searchPostData.ToDate.HasValue, model => model.CreatedDateTime <= searchPostData.ToDate.Value);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(searchPostData.PostType), model => model.Type == searchPostData.PostType);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(searchPostData.Keyword), model =>
                (EF.Functions.Like(model.Title, $"%{searchPostData.Keyword}%"))
                    || (EF.Functions.Like(model.Excerpt, $"%{searchPostData.Keyword}%"))
                    || (EF.Functions.Like(model.Content, $"%{searchPostData.Keyword}%")));
            return predicate;
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByMeta<TView>(
            string metaName,
            string metaValue,
            string postType,
            PagingRequest pagingData,
            string culture = null,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                postType ??= MixDatabaseNames.ADDITIONAL_COLUMN_POST;

                var valExp = Expressions.GetMetaExpression(metaName, metaValue, culture);

                return await SearchPostByValue<TView>(valExp, pagingData, postType, culture, context, transaction);
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }


        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByValueId<TView>(
            string valueId
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                // Get Tag
                var getVal = await MixDatabaseDataValues.ReadViewModel.Repository.GetSingleModelAsync(
                    m => m.Specificulture == culture
                    && m.Status == MixContentStatus.Published
                    && m.Id == valueId
                , context, transaction);
                if (getVal.IsSucceed)
                {
                    result = await GetPostListByDataId<TView>(
                            dataId: getVal.Data.DataId,
                            culture: culture,
                            orderByPropertyName: orderByPropertyName,
                            direction: direction,
                            pageSize: pageSize,
                            pageIndex: pageIndex,
                            _context: context,
                            _transaction: transaction);
                    //var query = context.MixRelatedAttributeData.Where(m=> m.Specificulture == culture
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixDatabaseDataType.Post)
                    //    .Select(m => m.ParentId).Distinct().ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByValueIds<TView>(
            List<string> valueIds
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                // Get Data
                Expression<Func<MixDatabaseDataValue, bool>> predicate = m => m.Specificulture == culture
                   && m.Status == MixContentStatus.Published;
                foreach (var item in valueIds)
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.Id == item;
                    predicate = predicate.AndAlso(pre);
                }
                var getVal = await MixDatabaseDataValues.ReadViewModel.Repository.GetModelListByAsync(predicate, context, transaction);
                if (getVal.IsSucceed)
                {
                    var dataIds = getVal.Data.Select(m => m.DataId).Distinct();
                    if (dataIds.Count() == 1)
                    {
                        result = await GetPostListByDataIds<TView>(
                                dataIds: dataIds.ToList(),
                                culture: culture,
                                orderByPropertyName: orderByPropertyName,
                                direction: direction,
                                pageSize: pageSize,
                                pageIndex: pageIndex,
                                _context: context,
                                _transaction: transaction);
                    }
                    //var query = context.MixRelatedAttributeData.Where(m=> m.Specificulture == culture
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixDatabaseDataType.Post)
                    //    .Select(m => m.ParentId).Distinct().ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByPageId<TView>(
            int pageId
            , string keyword = null
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPagePost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var result = await DefaultRepository<MixCmsContext, MixPagePost, TView>.Instance.GetModelListByAsync(
                            m => m.Specificulture == culture && m.PageId == pageId
                             && (string.IsNullOrEmpty(keyword)
                             || (EF.Functions.Like(m.MixPost.Title, $"%{keyword}%"))
                             || (EF.Functions.Like(m.MixPost.Excerpt, $"%{keyword}%"))
                             || (EF.Functions.Like(m.MixPost.Content, $"%{keyword}%"))
                             )
                            , orderByPropertyName, direction, pageSize, pageIndex
                            , _context: context, _transaction: transaction
                            );
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByDataId<TView>(
            string dataId
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var getRelatedData = await MixDatabaseDataAssociations.ReadViewModel.Repository.GetModelListByAsync(
                            m => m.Specificulture == culture && m.DataId == dataId
                            && m.ParentType == MixDatabaseParentType.Post
                            , orderByPropertyName = "CreatedDateTime", direction, pageSize, pageIndex
                            , _context: context, _transaction: transaction
                            );
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        Items = new List<TView>(),
                        PageIndex = pageIndex ?? 0,
                        PageSize = pageSize
                    }
                };
                if (getRelatedData.IsSucceed)
                {
                    foreach (var item in getRelatedData.Data.Items)
                    {
                        if (int.TryParse(item.ParentId, out int postId))
                        {
                            var getData = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetSingleModelAsync(
                            m => m.Specificulture == item.Specificulture && m.Id == postId
                                , context, transaction);
                            if (getData.IsSucceed)
                            {
                                result.Data.Items.Add(getData.Data);
                            }
                        }
                    }
                    result.Data.TotalItems = getRelatedData.Data.TotalItems;
                    result.Data.TotalPage = getRelatedData.Data.TotalPage;
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByDataIds<TView>(
            List<string> dataIds
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var result = new RepositoryResponse<PaginationModel<TView>>();
                Expression<Func<MixDatabaseDataAssociation, bool>> predicate = m => m.Specificulture == culture && dataIds.Contains(m.DataId)
                            && m.ParentType == MixDatabaseParentType.Post;
                foreach (var id in dataIds)
                {
                    Expression<Func<MixDatabaseDataAssociation, bool>> pre = m => m.DataId == id;
                    predicate = predicate.AndAlso(pre);
                }
                var getRelatedData = await MixDatabaseDataAssociations.ReadViewModel.Repository.GetModelListByAsync(
                            predicate
                            , orderByPropertyName = "CreatedDateTime", direction, pageSize, pageIndex
                            , _context: context, _transaction: transaction
                            );
                if (getRelatedData.IsSucceed)
                {
                    foreach (var item in getRelatedData.Data.Items)
                    {
                        if (int.TryParse(item.ParentId, out int postId))
                        {
                            var getData = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetSingleModelAsync(
                            m => m.Specificulture == item.Specificulture && m.Id == postId
                                , context, transaction);
                            if (getData.IsSucceed)
                            {
                                result = new RepositoryResponse<PaginationModel<TView>>()
                                {
                                    IsSucceed = true,
                                    Data = new PaginationModel<TView>()
                                    {
                                        Items = new List<TView>(),
                                        PageIndex = pageIndex ?? 0,
                                        PageSize = pageSize
                                    }
                                };
                                result.Data.Items.Add(getData.Data);
                            }
                        }
                    }
                    result.Data.TotalItems = getRelatedData.Data.TotalItems;
                    result.Data.TotalPage = getRelatedData.Data.TotalPage;
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> SearchPostByIds<TView>(
            string keyword
            , List<string> dataIds
            , List<string> nestedIds
            , List<int> pageIds = null
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                Expression<Func<MixPost, bool>> postPredicate = m => m.Specificulture == culture
                            && (string.IsNullOrEmpty(keyword)
                             || (EF.Functions.Like(m.Title, $"%{keyword}%"))
                             || (EF.Functions.Like(m.Excerpt, $"%{keyword}%"))
                             || (EF.Functions.Like(m.Content, $"%{keyword}%")));
                var searchPostByDataIds = SearchPostByDataIdsPredicate(dataIds, nestedIds, culture, context);
                postPredicate = postPredicate.AndAlsoIf(searchPostByDataIds != null, searchPostByDataIds);

                if (pageIds != null && pageIds.Count > 0)
                {
                    var searchPostByPageIds = SearchPostByPageIdsPredicate(pageIds, culture, context);
                    postPredicate = searchPostByPageIds.AndAlso(postPredicate);
                }

                if (!typeof(MixPost).GetProperties().Any(p => p.Name.ToLower() == orderByPropertyName.ToLower()))
                {
                    var postIds = context.MixPost.Where(postPredicate).Select(p => p.Id);
                    var orderedPostIds = context.MixDatabaseDataAssociation.Where(
                            m => m.Specificulture == culture && postIds.Any(p => p.ToString() == m.ParentId))
                        .Join(context.MixDatabaseDataValue, r => r.DataId, v => v.DataId, (r, v) => new { r, v })
                        .OrderBy(rv => rv.v.StringValue)
                        .Select(rv => rv.r.ParentId);
                    postPredicate = p => orderedPostIds.Distinct().Any(o => o == p.Id.ToString() && p.Specificulture == culture);
                }

                return await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                        postPredicate
                        , orderByPropertyName, direction
                        , pageSize, pageIndex
                        , _context: context, _transaction: transaction);
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private static Expression<Func<MixPost, bool>> SearchPostByPageIdsPredicate(List<int> pageIds, string culture, MixCmsContext context)
        {
            Expression<Func<MixPost, bool>> postPredicate = null;
            Expression<Func<MixPagePost, bool>> predicate = null;
            foreach (var id in pageIds)
            {
                // Get list related post ids by data id
                Expression<Func<MixPagePost, bool>> pre =
                    m => m.Specificulture == culture
                        && m.Status == MixContentStatus.Published
                        && m.PageId == id;
                predicate = predicate == null ? pre : predicate.Or(pre);
            }
            if (predicate != null)
            {
                var postIds = context.MixPagePost
                .Where(predicate)
                .Select(m => m.PostId).Distinct();
                postPredicate = p => postIds.Any(m => m == p.Id);
            }
            return postPredicate;
        }

        private static Expression<Func<MixPost, bool>> SearchPostByDataIdsPredicate(List<string> dataIds, List<string> nestedIds, string culture, MixCmsContext context)
        {
            Expression<Func<MixPost, bool>> postPredicate = null;
            Expression<Func<MixDatabaseDataAssociation, bool>> predicate = m => m.ParentType == MixDatabaseParentType.Post
                && m.Specificulture == culture;

            if (nestedIds != null && nestedIds.Count > 0)
            {
                var nestedQuery = context.MixDatabaseDataAssociation
                .Where(m =>
                    m.Specificulture == culture
                   && nestedIds.Any(n => n == m.DataId))
                .Select(m => m.ParentId).Distinct();

                dataIds.AddRange(nestedQuery);
            }

            if (dataIds.Count > 0)
            {
                predicate = predicate.AndAlso(m => dataIds.Any(n => n == m.DataId));
                var postIds = context.MixDatabaseDataAssociation
                .Where(predicate)
                .GroupBy(m => m.ParentId)
                .Select(g => new { ParentId = g.Key, Count = g.Count() })
                .Where(c => c.Count == dataIds.Count)
                .Select(m => m.ParentId);

                postPredicate = p => postIds.Any(m => p.Id.ToString() == m);
            }
            return postPredicate;
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> SearchPostByField<TView>(
            string fieldName, string value
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime", Heart.Enums.DisplayDirection direction = Heart.Enums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                // Get Value
                var dataIds = await context.MixDatabaseDataValue.Where(
                    m => m.MixDatabaseName == MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST && m.Specificulture == culture
                        && EF.Functions.Like(m.StringValue, value) && m.MixDatabaseColumnName == fieldName)
                    .Select(m => m.DataId)?.ToListAsync();
                if (dataIds != null && dataIds.Count > 0)
                {
                    var getRelatedData = await MixDatabaseDataAssociations.ReadViewModel.Repository.GetModelListByAsync(
                        m => dataIds.Contains(m.DataId)
                        , orderByPropertyName, direction, pageSize, pageIndex
                        , _context: context, _transaction: transaction
                        );
                    if (getRelatedData.IsSucceed)
                    {
                        foreach (var item in getRelatedData.Data.Items)
                        {
                            if (int.TryParse(item.ParentId, out int postId))
                            {
                                var getData = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetSingleModelAsync(
                                m => m.Specificulture == item.Specificulture && m.Id == postId
                                    , context, transaction);
                                if (getData.IsSucceed)
                                {
                                    result.Data.Items.Add(getData.Data);
                                }
                            }
                        }
                        result.Data.TotalItems = getRelatedData.Data.TotalItems;
                        result.Data.TotalPage = getRelatedData.Data.TotalPage;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static async Task PublishScheduledPosts()
        {
            using (var context = new MixCmsContext())
            {
                var now =  DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"));
                var sheduledPosts = context.MixPost
                    .Where(m => m.Status == MixContentStatus.Schedule
                        && Equals(m.PublishedDateTime.Value, now));
                var posts = await sheduledPosts.ToListAsync();
                foreach (var post in posts)
                {
                    post.Status = MixContentStatus.Published;
                }
                _ = await context.SaveChangesAsync();
            }
        }
    }
}