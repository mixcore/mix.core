using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mixdb-context")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseContextController
        : MixRestfulApiControllerBase<MixDatabaseContextViewModel, MixCmsContext, MixDatabaseContext, int>
    {
        private readonly IMixDbService _mixDbService;
        public MixDatabaseContextController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService, IMixDbService mixDbService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _mixDbService = mixDbService;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixDatabaseContextViewModel data, CancellationToken cancellationToken = default)
        {
            if (data != null && !string.IsNullOrEmpty(data.DecryptedConnectionString))
            {
                data.ConnectionString = AesEncryptionHelper.EncryptString(data.DecryptedConnectionString, GlobalConfigService.Instance.AesKey);
            }

            var result = await base.CreateHandlerAsync(data, cancellationToken);
            if (result > 0)
            {
                var dbContext = new MixDatabaseContext();
                ReflectionHelper.Map(data, dbContext);
                await _mixDbService.MigrateInitNewDbContextDatabases(dbContext);
            }
            return result;
        }

        #endregion

        #region Routes

        [HttpPost("migrate")]
        public async Task<ActionResult<object>> MigrateInitDatabases([FromBody] MixDatabaseContext dbContext)
        {
            await _mixDbService.MigrateInitNewDbContextDatabases(dbContext);
            return Ok();
        }

        #endregion
    }
}
