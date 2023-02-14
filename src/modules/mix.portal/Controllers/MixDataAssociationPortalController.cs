using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content-association")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDataContentAssociationPortalController
        : MixRestfulApiControllerBase<MixDataContentAssociationViewModel, MixCmsContext, MixDataContentAssociation, Guid>
    {
        public MixDataContentAssociationPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            IMixDataService mixDataService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService)
        {
            mixDataService.SetUnitOfWork(Uow);
        }

        protected override SearchQueryModel<MixDataContentAssociation, Guid> BuildSearchRequest(SearchRequestDto req)
        {
            return new SearchDataContentAssociationModel(req, Request);
        }
    }
}
