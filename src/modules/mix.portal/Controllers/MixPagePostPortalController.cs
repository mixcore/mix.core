using Microsoft.AspNetCore.Mvc;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-post")]
    [ApiController]
    public class MixPagePostController
        : MixAssociationApiControllerBase<MixPagePostAssociationViewModel, MixCmsContext, MixPagePostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        public MixPagePostController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> cmsUow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub) : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub)
        {
            _cmsUow = cmsUow;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixPagePostAssociationViewModel data, CancellationToken cancellationToken = default)
        {
            if (_cmsUow.DbContext.MixPagePostAssociation.Any(
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
