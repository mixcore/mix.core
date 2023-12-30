using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-domain")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDomainController
        : MixRestfulApiControllerBase<MixDomainViewModel, MixCmsContext, MixDomain, int>
    {
        public MixDomainController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService) 
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides

        protected override Task DeleteHandler(MixDomainViewModel data, CancellationToken cancellationToken = default)
        {
            if (data.Id == 1)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot delete root domain");
            }
            return base.DeleteHandler(data, cancellationToken);
        }

        #endregion
    }
}
