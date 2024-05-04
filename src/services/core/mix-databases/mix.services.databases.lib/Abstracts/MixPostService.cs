using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Models;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Dtos;

namespace Mix.Services.Databases.Lib.Abstracts
{
    public abstract class MixPostServiceBase<TView> : TenantServiceBase
        where TView : ViewModelBase<MixCmsContext, MixPostContent, int, TView>
    {
        protected readonly UnitOfWorkInfo<MixCmsContext> Uow;
        protected readonly IMixMetadataService MetadataService;

        protected MixPostServiceBase(
            UnitOfWorkInfo<MixCmsContext> uow,
            IMixMetadataService metadataService,
            IHttpContextAccessor httpContextAccessor,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            Uow = uow;
            MetadataService = metadataService;
        }

        public virtual async Task<List<TView>> GetRelatedPosts(int postId, CancellationToken cancellationToken = default)
        {
            using var postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(Uow);
            var relatedIds = from links in Uow.DbContext.MixPostPostAssociation
                             where links.ParentId == postId && links.MixTenantId == CurrentTenant.Id
                             select links.ChildId;
            var result = await postRepo.GetListAsync(m => relatedIds.Contains(m.Id), cancellationToken);
            return result;
        }

        public virtual async Task<PagingResponseModel<TView>> Search(HttpRequest httpRequest)
        {
            return await SearchPosts(new(httpRequest));
        }

        public virtual async Task<PagingResponseModel<TView>> SearchPosts(SearchPostQueryModel searchRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(Uow);
                postRepo.CacheService = CacheService;
                var result = await postRepo.GetPagingAsync(
                        searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public IQueryable<int>? ParseMetadataQueriesPredicate(List<SearchQueryField> MetadataQueries)
        {
            IQueryable<int>? allowedIdQuery = MetadataService.GetQueryableContentIdByMetadataSeoContent(
                                                MetadataQueries, MixContentType.Post);
            return allowedIdQuery;
        }
    }
}
