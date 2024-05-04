using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/p4ps/page-content")]
    public sealed class PageContentApiController : MixQueryApiControllerBase<PageContentViewModel, MixCmsContext, MixPageContent, int>
    {
        private readonly IMixMetadataService _metadataService;
        private readonly IMixDbDataService _mixDbDataService;
        public PageContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixMetadataService metadataService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbDataService mixDbDataService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _metadataService = metadataService;
            _mixDbDataService = mixDbDataService;
        }

        protected override async Task<PageContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            await result.LoadDataAsync(_mixDbDataService, _metadataService, new(), CacheService);
            return result;
        }
    }
}
