using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Heart.Entities.Cache;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Universal.Lib.Entities;
using Mix.Universal.Lib.ViewModels;

namespace Mix.Universal.Controllers
{
    [Route("api/v2/rest/mix-universal/organization")]
    [ApiController]
    //[MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class OrganizationController
        : MixRestApiControllerBase<OrganizationViewModel, MixUniversalDbContext, Organization, int>
    {
        UnitOfWorkInfo<MixUniversalDbContext> _cmsUOW;
        public OrganizationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixUniversalDbContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _cmsUOW = cmsUOW;
        }

        #region Overrides

        #endregion


    }
}
