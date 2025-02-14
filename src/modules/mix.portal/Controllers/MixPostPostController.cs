using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-post")]
    [ApiController]
    public class MixPostPostController
        : MixAssociationApiControllerBase<MixPostPostAssociationViewModel, MixCmsContext, MixPostPostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        public MixPostPostController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> cmsUow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService) 
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _cmsUow = cmsUow;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixPostPostAssociationViewModel data, CancellationToken cancellationToken = default)
        {
            if (_cmsUow.DbContext.MixPostPostAssociation.Any(
                m => m.TenantId == CurrentTenant.Id
                && m.ParentId == data.ParentId
                && m.ChildId == data.ChildId))
            {
                throw new MixException(MixErrorStatus.Badrequest, "Entity existed");
            }
            return base.CreateHandlerAsync(data, cancellationToken);
        }
        #endregion
    }
}
