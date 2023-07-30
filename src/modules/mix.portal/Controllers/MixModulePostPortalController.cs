using Microsoft.AspNetCore.Mvc;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/module-post")]
    [ApiController]
    public class MixModulePostController
        : MixAssociationApiControllerBase<MixModulePostAssociationViewModel, MixCmsContext, MixModulePostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        public MixModulePostController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub)
        {
            _cmsUow = cmsUow;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixModulePostAssociationViewModel data, CancellationToken cancellationToken = default)
        {
            if (_cmsUow.DbContext.MixModulePostAssociation.Any(
                m => m.MixTenantId == CurrentTenant.Id
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
