using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-domain")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDomainController
        : MixRestApiControllerBase<MixDomainViewModel, MixCmsContext, MixDomain, int>
    {
        public MixDomainController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
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
