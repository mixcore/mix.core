using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;
using MySqlX.XDevAPI.Common;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mixdb-context")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseContextController
        : MixRestfulApiControllerBase<MixDatabaseContextViewModel, MixCmsContext, MixDatabaseContext, int>
    {
        private readonly IMixdbStructure _mixDbService;
        public MixDatabaseContextController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixdbStructure mixDbService)
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
                data.ConnectionString = data.DecryptedConnectionString.Encrypt(Configuration.AesKey());
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

        protected override async Task UpdateHandler(int id, MixDatabaseContextViewModel data, CancellationToken cancellationToken = default)
        {
            await base.UpdateHandler(id, data, cancellationToken);
            if (data.DatabaseProvider == MixDatabaseProvider.PostgreSQL)
            {
                _mixDbService.InitDbStructureService(data.ConnectionString.Decrypt(Configuration.AesKey()), data.DatabaseProvider);
                await _mixDbService.ExecuteCommand("CREATE EXTENSION IF NOT EXISTS \"unaccent\";", cancellationToken);
            }

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
