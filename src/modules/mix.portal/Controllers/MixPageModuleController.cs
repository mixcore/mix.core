using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-module")]
    [ApiController]
    public class MixPageModuleController
        : MixAssociationApiControllerBase<MixPageModuleViewModel, MixCmsContext, MixPageModuleAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        public MixPageModuleController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, cmsUow, queueService)
        {
            _cmsUow = cmsUow;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixPageModuleViewModel data, CancellationToken cancellationToken = default)
        {
            if (_cmsUow.DbContext.MixPageModuleAssociation.Any(
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
