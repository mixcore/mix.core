using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.ViewModels;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]

    public class MixDatabaseController
        : MixRestfulApiControllerBase<MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        private readonly IMixDbService _mixDbService;
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
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _mixDbService = mixDbService;
        }

        #region Routes
        [AllowAnonymous]
        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<MixDatabaseViewModel>> GetByName(string name)
        {
            var result = await Repository.GetSingleAsync(m => m.SystemName == name);
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
                //await _mixDbService.BackupDatabase(data.SystemName, cancellationToken);
                await base.UpdateHandler(id, data, cancellationToken);
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
    }
}
