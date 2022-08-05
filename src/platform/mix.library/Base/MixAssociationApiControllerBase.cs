using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public class MixAssociationApiControllerBase<TView, TDbContext, TEntity>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, int>
        where TDbContext : DbContext
        where TEntity : AssociationBase<int>
        where TView : AssociationViewModelBase<TDbContext, TEntity, int, TView>
    {
        public MixAssociationApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, uow, queueService)
        {
        }

        #region Routes

        [HttpGet("search")]
        public async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchAssociationDto req)
        {
            var searchRequest = BuildSearchByParentRequest(req);
            return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        }

        #endregion

        protected SearchQueryModel<TEntity, int> BuildSearchByParentRequest(SearchAssociationDto req)
        {
            var request = new SearchAssociationDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);
            int leftId = request.ParentId.HasValue ? request.ParentId.Value : 0;
            searchRequest.Predicate = searchRequest.Predicate.AndAlso(
                m => m.ParentId == leftId);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ChildId.HasValue,
                m => m.ChildId == request.ChildId.Value);

            return searchRequest;
        }
    }
}
