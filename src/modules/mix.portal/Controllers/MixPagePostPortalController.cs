using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-post")]
    [ApiController]
    public class MixPagePostController
        : MixAssociationApiControllerBase<MixPagePostAssociationViewModel, MixCmsContext, MixPagePostAssociation>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;

        public MixPagePostController(
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
            _cmsUOW = cmsUOW;
        }

        #region Overrides

        protected override Task<int> CreateHandlerAsync(MixPagePostAssociationViewModel data)
        {
            if (_cmsUOW.DbContext.MixPagePostAssociation.Any(
                m => m.MixTenantId == _currentTenant.Id
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
