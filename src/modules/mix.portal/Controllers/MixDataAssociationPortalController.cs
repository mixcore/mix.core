using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content-association")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDataContentAssociationPortalController
        : MixRestApiControllerBase<MixDataContentAssociationViewModel, MixCmsContext, MixDataContentAssociation, Guid>
    {
        private readonly Repository<MixCmsContext, MixDatabaseColumn, int, MixDatabaseColumnViewModel> _colRepository;
        private readonly MixDataService _mixDataService;

        public MixDataContentAssociationPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _mixDataService = mixDataService;
            _mixDataService.SetUnitOfWork(_uow);
            _colRepository = MixDatabaseColumnViewModel.GetRootRepository(context);
        }

        protected override Expression<Func<MixDataContentAssociation, bool>> BuildAndPredicate(SearchRequestDto req)
        {
            SearchMixDataDto searchReq = new SearchMixDataDto(req, Request);

            var predicate = base.BuildAndPredicate(req);
            predicate = predicate.AndAlso(m => 
            m.MixDatabaseId == searchReq.MixDatabaseId 
            || m.MixDatabaseName == searchReq.MixDatabaseName);
            predicate = predicate.AndAlsoIf(searchReq.IntParentId.HasValue, m => m.IntParentId == searchReq.IntParentId);
            predicate = predicate.AndAlsoIf(searchReq.GuidParentId.HasValue, m => m.GuidParentId == searchReq.GuidParentId);
            return predicate;
        }
    }
}
