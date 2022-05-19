using Microsoft.AspNetCore.Mvc;

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
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _mixDataService = mixDataService;
            _mixDataService.SetUnitOfWork(_uow);
            _colRepository = MixDatabaseColumnViewModel.GetRootRepository(context);
        }

        protected override SearchQueryModel<MixDataContentAssociation, Guid> BuildSearchRequest(SearchRequestDto req)
        {
            return new SearchDataContentAssociationModel(MixTenantId, req, Request);
        }
    }
}
