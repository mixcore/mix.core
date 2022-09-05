using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.RepoDb.Services;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseController
        : MixRestApiControllerBase<Lib.ViewModels.MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        private readonly ApplicationLifetime _applicationLifetime;
        private MixDbService _mixDbService;
        public MixDatabaseController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService, MixDbService mixDbService, RuntimeDbContextService runtimeDbContextService, ApplicationLifetime applicationLifetime)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _mixDbService = mixDbService;
            _applicationLifetime = applicationLifetime;
        }

        #region Routes

        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<MixDatabaseViewModel>> GetByName(string name)
        {
            var result = await _repository.GetSingleAsync(m => m.SystemName == name);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("migrate/{name}")]
        public async Task<ActionResult> Migrate(string name)
        {
            var result = await _mixDbService.MigrateDatabase(name);
            
            // TODO: Check repodb not load latest database schema when modified column, reload application to let repodb load latest schema
            //_applicationLifetime.StopApplication();
            return result ? Ok() : BadRequest();
        }

        [HttpGet("backup/{name}")]
        public ActionResult Backup(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Backup, name);
            _queueService.PushQueue(msg);
            return Ok();
        }
        
        [HttpGet("restore/{name}")]
        public async Task<ActionResult> RestoreAsync(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Restore, name);
            _queueService.PushQueue(msg);
            return Ok();
        }

        #endregion
        #region Overrides
        protected override async Task UpdateHandler(int id, MixDatabaseViewModel data)
        {
            await _mixDbService.BackupDatabase(data.SystemName);
            await base.UpdateHandler(id, data);
        }
        protected override Task DeleteHandler(Lib.ViewModels.MixDatabaseViewModel data)
        {
            if (data.Type == MixDatabaseType.System)
            {
                throw new MixException($"Cannot Delete System Database: {data.SystemName}");
            }
            return base.DeleteHandler(data);
        }
        #endregion


    }
}
