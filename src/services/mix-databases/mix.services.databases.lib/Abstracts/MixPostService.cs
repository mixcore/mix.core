using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Models;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Models.Common;
using System.Linq.Expressions;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Interfaces;

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
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
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
                searchRequest.Predicate = searchRequest.Predicate.AndAlso(ApplyMetadataQueries(searchRequest));
                var result = await postRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        private Expression<Func<MixPostContent, bool>> ApplyMetadataQueries(SearchPostQueryModel searchRequest)
        {
            Expression<Func<MixPostContent, bool>> predicate = m => true;
            if (searchRequest.AndMetadataQueries.Count == 0 && searchRequest.OrMetadataQueries.Count == 0)
            {
                return predicate;
            }

            IQueryable<int>? allowedIdQuery = default;
            if (searchRequest.OrMetadataQueries.Any())
            {
                allowedIdQuery = MetadataService.GetQueryableContentIdByMetadataSeoContent(searchRequest.OrMetadataQueries, MixContentType.Post, false);
            }
            if (searchRequest.AndMetadataQueries.Any())
            {
                var andQuery = MetadataService.GetQueryableContentIdByMetadataSeoContent(searchRequest.AndMetadataQueries, MixContentType.Post, true);
                allowedIdQuery = allowedIdQuery != default ? allowedIdQuery.Where(m => andQuery.Contains(m)) : andQuery;
            }
            predicate = predicate.AndAlsoIf(allowedIdQuery != null, m => allowedIdQuery!.ToList().Contains(m.Id));
            return predicate;
        }
    }
}
