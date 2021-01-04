using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
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
        /// <summary>
        /// Gets the modelist by meta.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <param name="culture">The culture.</param>
        /// <param name="metaName">Name of the meta. Ex: sys_tag / sys_category</param>
        /// <param name="metaValue">The meta value.</param>
        /// <param name="orderByPropertyName">Name of the order by property.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByMeta<TView>(
            string metaName, string metaValue
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
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
                var getVal = await MixAttributeSetValues.ReadViewModel.Repository.GetSingleModelAsync(
                    m => m.Specificulture == culture && m.Status == MixEnums.MixContentStatus.Published
                        && m.AttributeSetName == metaName
                        && m.AttributeFieldName == "title" && m.StringValue == metaValue
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
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixAttributeSetDataType.Post)
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetPostListByValueId<TView>(
            string valueId
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
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
                var getVal = await MixAttributeSetValues.ReadViewModel.Repository.GetSingleModelAsync(
                    m => m.Specificulture == culture
                    && m.Status == MixEnums.MixContentStatus.Published
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
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixAttributeSetDataType.Post)
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
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
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
                Expression<Func<MixAttributeSetValue, bool>> predicate = m => m.Specificulture == culture
                   && m.Status == MixEnums.MixContentStatus.Published;
                foreach (var item in valueIds)
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.Id == item;

                    predicate = ReflectionHelper.CombineExpression(
                        predicate
                        , pre
                        , Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                }
                var getVal = await MixAttributeSetValues.ReadViewModel.Repository.GetModelListByAsync(predicate, context, transaction);
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
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixAttributeSetDataType.Post)
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
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPagePost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
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
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
                var getRelatedData = await MixRelatedAttributeDatas.ReadViewModel.Repository.GetModelListByAsync(
                            m => m.Specificulture == culture && m.DataId == dataId
                            && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
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
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
                var result = new RepositoryResponse<PaginationModel<TView>>();
                Expression<Func<MixRelatedAttributeData, bool>> predicate = m => m.Specificulture == culture && dataIds.Contains(m.DataId)
                            && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString();
                foreach (var id in dataIds)
                {
                    Expression<Func<MixRelatedAttributeData, bool>> pre = m => m.DataId == id;

                    predicate = ReflectionHelper.CombineExpression(
                        predicate
                        , pre
                        , Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                }
                var getRelatedData = await MixRelatedAttributeDatas.ReadViewModel.Repository.GetModelListByAsync(
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> SearchPost<TView>(
            string keyword
            , List<string> dataIds
            , List<int> pageIds = null
            , string culture = null
            , string orderByPropertyName = "CreatedDateTime"
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                };
                List<int> postIds = SearchPostByDataIds(dataIds, culture, context);
                if (pageIds!= null &&  pageIds.Count>0)
                {
                    postIds.AddRange(SearchPostByPageIds(pageIds, culture, context));
                    postIds = postIds.Distinct().ToList();
                }
                // Load Posts
                
                if (postIds != null && postIds.Count > 0)
                {
                    Expression<Func<MixPost, bool>> postPredicate = m => m.Specificulture == culture
                            && (string.IsNullOrEmpty(keyword)
                             || (EF.Functions.Like(m.Title, $"%{keyword}%"))
                             || (EF.Functions.Like(m.Excerpt, $"%{keyword}%"))
                             || (EF.Functions.Like(m.Content, $"%{keyword}%")))
                            && postIds.Any(n => n == m.Id);

                    var getPosts = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                            postPredicate
                            , orderByPropertyName, direction
                            , pageSize, pageIndex
                            , _context: context, _transaction: transaction);
                    result = getPosts;
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

        private static List<int> SearchPostByPageIds(List<int> pageIds, string culture, MixCmsContext context)
        {
            List<int> postIds = new List<int>();

            foreach (var id in pageIds)
            {
                // Get list related post ids by data id
                Expression<Func<MixPagePost, bool>> predicate =
                    m => m.Specificulture == culture
                        && m.Status == MixEnums.MixContentStatus.Published
                        && m.PageId == id;
                var ids = context.MixPagePost
                    .Where(predicate)
                    .Select(m => m.PostId).ToList();
                postIds.AddRange(ids);
            }
            return postIds.Distinct().ToList();
        }
        
        private static List<int> SearchPostByDataIds(List<string> dataIds, string culture, MixCmsContext context)
        {
            List<int> postIds = new List<int>();

            foreach (var id in dataIds)
            {
                // Get list related post ids by data id
                Expression<Func<MixRelatedAttributeData, bool>> dataPredicate =
                    m => m.Specificulture == culture
                        && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
                        && m.DataId == id;
                var ids = context.MixRelatedAttributeData
                    .Where(dataPredicate)
                    .Select(m => int.Parse(m.ParentId)).ToList();

                // if first id in list => return ids
                if (postIds.Count == 0)
                {
                    postIds = ids.Distinct().ToList();
                }
                else
                {
                    // filter ids by new data id ( for 'AND' condition)
                    postIds = postIds.Where(m => ids.Contains(m)).Distinct().ToList();
                    // if there is no items => no need to filter more
                    if (postIds.Count == 0)
                    {
                        break;
                    }
                }
            }
            return postIds;
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByAddictionalField<TView>(
            string fieldName, string value, string culture
            , string orderByPropertyName = "CreatedDateTime", Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Desc
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
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
                var dataIds = await context.MixAttributeSetValue.Where(
                    m => m.AttributeSetName == MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST && m.Specificulture == culture
                        && m.StringValue == value && m.AttributeFieldName == fieldName)
                    .Select(m => m.DataId)?.ToListAsync();
                if (dataIds != null && dataIds.Count > 0)
                {
                    var getRelatedData = await MixRelatedAttributeDatas.ReadViewModel.Repository.GetModelListByAsync(
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
            using(var context = new MixCmsContext())
            {
                var now = DateTime.UtcNow;
                now = now.AddSeconds(-now.Second);
                var sheduledPosts = context.MixPost
                    .Where(m => m.Status == MixEnums.MixContentStatus.Schedule
                        && Equals(m.PublishedDateTime.Value, now));
                var posts = await sheduledPosts.ToListAsync();
                foreach (var post in posts)
                {
                    post.Status = MixEnums.MixContentStatus.Published;
                }
                _ = await context.SaveChangesAsync();
            }
        }
    }
}