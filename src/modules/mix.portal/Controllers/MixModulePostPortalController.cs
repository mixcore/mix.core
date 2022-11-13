using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-module-post")]
    [ApiController]
    public class MixModulePostController
        : MixAssociationApiControllerBase<MixModulePostAssociationViewModel, MixCmsContext, MixModulePostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;

        public MixModulePostController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {
            _cmsUOW = cmsUOW;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixModulePostAssociationViewModel data)
        {
            if (_cmsUOW.DbContext.MixModulePostAssociation.Any(
                m => m.MixTenantId == CurrentTenant.Id
                && m.ParentId == data.ParentId
                && m.ChildId == data.ChildId))
            {
                throw new MixException(MixErrorStatus.Badrequest, "Entity existed");
            }
            return base.CreateHandlerAsync(data);
        }
        #endregion


    }
}
