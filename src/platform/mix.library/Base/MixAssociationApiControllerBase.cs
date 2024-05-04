using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Dtos;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Base
{
    public class MixAssociationApiControllerBase<TView, TDbContext, TEntity>
        : MixRestfulApiControllerBase<TView, TDbContext, TEntity, int>
        where TDbContext : DbContext
        where TEntity : AssociationBase<int>
        where TView : AssociationViewModelBase<TDbContext, TEntity, int, TView>
    {
        public MixAssociationApiControllerBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<TDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }

        #region Routes

        [HttpGet("search")]
        public async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchAssociationDto req, CancellationToken cancellationToken = default)
        {
            var searchRequest = BuildSearchByParentRequest(req);
            return await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
        }

        #endregion

        protected override SearchQueryModel<TEntity, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchRequest = base.BuildSearchRequest(req);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                int.TryParse(Request.Query["parentId"], out int parentId),
                m => m.ParentId == parentId);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                int.TryParse(Request.Query["childId"], out int childId),
                m => m.ChildId == childId);

            return searchRequest;
        }

        protected SearchQueryModel<TEntity, int> BuildSearchByParentRequest(SearchAssociationDto request)
        {
            var searchRequest = base.BuildSearchRequest(request);
            int leftId = request.ParentId ?? 0;
            searchRequest.Predicate = searchRequest.Predicate.AndAlso(
                m => m.ParentId == leftId);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ChildId.HasValue,
                m => m.ChildId == request.ChildId.Value);

            return searchRequest;
        }
    }
}
