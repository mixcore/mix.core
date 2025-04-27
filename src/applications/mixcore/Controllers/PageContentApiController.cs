using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Models;
using Mix.SignalR.Interfaces;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/mixcore/page-content")]
    public sealed class PageContentApiController : MixQueryApiControllerBase<PageContentViewModel, MixCmsContext, MixPageContent, int>
    {
        private readonly IMixMetadataService _metadataService;
        private readonly IMixDbDataService _mixDbDataService;
        public PageContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixMetadataService metadataService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbDataServiceFactory mixDbDataServiceFactory,
            DatabaseService databaseService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _metadataService = metadataService;
            _mixDbDataService = mixDbDataServiceFactory.Create(
            databaseService.DatabaseProvider,
            databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION));
        }

        protected override async Task<PageContentViewModel> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetById(id, cancellationToken);
            await result.LoadDataAsync(_mixDbDataService, _metadataService, new(), CacheService);
            return result;
        }
    }
}
