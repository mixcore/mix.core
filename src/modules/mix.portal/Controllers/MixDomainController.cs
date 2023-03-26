using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-domain")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDomainController
        : MixRestfulApiControllerBase<MixDomainViewModel, MixCmsContext, MixDomain, int>
    {
        public MixDomainController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixService mixService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> cmsUOW, IQueueService<MessageQueueModel> queueService, MixCacheService cacheService) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService, cacheService)
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
