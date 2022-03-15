using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Base;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public class MixAssociationApiControllerBase<TView, TDbContext, TEntity>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, int>
        where TDbContext : DbContext
        where TEntity : AssociationBase<int>
        where TView : ViewModelBase<TDbContext, TEntity, int, TView>
    {
        public MixAssociationApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
        }
        protected override SearchQueryModel<TEntity, int> BuildSearchRequest(SearchRequestDto req)
        {
            var request = new SearchAssociationDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);
            int leftId = request.LeftId.HasValue ? request.LeftId.Value : 0;
            searchRequest.Predicate = searchRequest.Predicate.AndAlso(
                m => m.LeftId == leftId);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.RightId.HasValue,
                m => m.RightId == request.RightId.Value);

            return searchRequest;
        }


    }
}
