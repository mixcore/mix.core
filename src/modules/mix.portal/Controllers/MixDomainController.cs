using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-domain")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDomainController
        : MixRestfulApiControllerBase<MixDomainViewModel, MixCmsContext, MixDomain, int>
    {
        public MixDomainController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {

        }

        #region Overrides

        protected override Task DeleteHandler(MixDomainViewModel data)
        {
            if (data.Id == 1)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot delete root domain");
            }
            return base.DeleteHandler(data);
        }

        #endregion


    }
}
