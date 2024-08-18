using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.ViewModels;
using Mix.Service.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]

    public class MixDatabaseController
        : MixRestfulApiControllerBase<MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        private readonly IMixDbService _mixDbService;
        private readonly IMixMemoryCacheService _memoryCache;
        public MixDatabaseController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixDbService mixDbService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixMemoryCacheService memoryCache)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _mixDbService = mixDbService;
            _memoryCache = memoryCache;
        }

        #region Routes
        [AllowAnonymous]
        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<MixDatabaseViewModel>> GetByName(string name)
        {
            var result = await GetMixDatabase(name);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("duplicate/{id}")]
        public async Task<ActionResult<MixDatabaseViewModel>> Duplicate(int id, CancellationToken cancellationToken = default)
        {
            var data = await GetById(id);
            if (data != null)
            {
                data.Duplicate();
                var newId = await CreateHandlerAsync(data, cancellationToken);
                var result = await GetById(newId);
                return Ok(result);
            }
            throw new MixException(MixErrorStatus.NotFound, id);
        }

        //[MixAuthorize(MixRoles.Owner)]
        [HttpGet("export-entity/{dbContextName}")]
        public async Task<ActionResult> ExportEntity(string dbContextName)
        {
            MixDatabaseContextViewModel dbContext = await MixDatabaseContextViewModel
                       .GetRepository(Uow, CacheService).GetSingleAsync(m => m.SystemName == dbContextName);
            if (dbContext == null)
            {
                return BadRequest(dbContextName);
            }
            var runtimeDbContextService = new RuntimeDbContextService(HttpContextAccessor, _databaseService);
            string cnn = dbContext != null
                ? dbContext.DecryptedConnectionString
                : _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            var sourceFiles = runtimeDbContextService.CreateDynamicDbContext(cnn);
            return Ok(sourceFiles);
        }
        
        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("migrate/{name}")]
        public async Task<ActionResult> Migrate(string name)
        {
            //await _mixDbService.BackupDatabase(name);
            RepoDbMixDatabaseViewModel database = await RepoDbMixDatabaseViewModel
                       .GetRepository(Uow, CacheService).GetSingleAsync(m => m.SystemName == name);
            var result = await _mixDbService.MigrateDatabase(database);
            //await _mixDbService.RestoreFromLocal(name);
            return result ? Ok() : BadRequest();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("backup/{name}")]
        public ActionResult Backup(string name)
        {
            var msg = new MessageQueueModel(CurrentTenant.Id, MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Backup, name);
            QueueService.PushMemoryQueue(msg);
            return Ok();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("restore/{name}")]
        public ActionResult RestoreAsync(string name)
        {
            var msg = new MessageQueueModel(CurrentTenant.Id, MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Restore, name);
            QueueService.PushMemoryQueue(msg);
            return Ok();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("update/{name}")]
        public ActionResult UpdateAsync(string name)
        {
            var msg = new MessageQueueModel(CurrentTenant.Id, MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Update, name);
            QueueService.PushMemoryQueue(msg);
            return Ok();
        }

        #endregion

        #region Overrides
        protected override SearchQueryModel<MixDatabase, int> BuildSearchRequest(SearchRequestDto req)
        {
            var predicate = base.BuildSearchRequest(req);
            if (int.TryParse(HttpContext.Request.Query["MixDatabaseContextId"], out int mixDatabaseContextId))
            {
                predicate.Predicate = predicate.Predicate.AndAlso(m => m.MixDatabaseContextId == mixDatabaseContextId);
            }
            else
            {
                predicate.Predicate = predicate.Predicate.AndAlso(m => m.MixDatabaseContextId == null);
            }
            return predicate;
        }
        protected override async Task UpdateHandler(int id, MixDatabaseViewModel data, CancellationToken cancellationToken = default)
        {
            try
            {
                var newColumnNames = data.Columns
                                    .Where(m => m.Id == 0)
                                    .Select(m => m.SystemName)
                                    .ToList();
                await base.UpdateHandler(id, data, cancellationToken);
                var newColumns = data.Columns.Where(m => newColumnNames.Any(n => n == m.SystemName));
                foreach (var col in newColumns)
                {
                    QueueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixViewModelChanged, MixRestAction.Post.ToString(), col);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }
        protected override Task DeleteHandler(MixDatabaseViewModel data, CancellationToken cancellationToken = default)
        {
            //if (data.Type == MixDatabaseType.System)
            //{
            //    throw new MixException($"Cannot Delete System Database: {data.SystemName}");
            //}
            return base.DeleteHandler(data, cancellationToken);
        }
        #endregion

        #region Privates
        private async Task<RepoDbMixDatabaseViewModel> GetMixDatabase(string tableName)
        {
            string name = $"{typeof(RepoDbMixDatabaseViewModel).FullName}_{tableName}";
            return await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return RepoDbMixDatabaseViewModel.GetRepository(Uow, CacheService).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
        }

        #endregion
    }
}
