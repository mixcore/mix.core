using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-post")]
    [ApiController]
    public class MixPostPostController
        : MixAssociationApiControllerBase<MixPostPostAssociationViewModel, MixCmsContext, MixPostPostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;

        public MixPostPostController(
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

        protected override Task<int> CreateHandlerAsync(MixPostPostAssociationViewModel data)
        {
            if (_cmsUOW.DbContext.MixPostPostAssociation.Any(
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
