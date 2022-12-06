using Microsoft.AspNetCore.Mvc;
using Mix.RepoDb.Services;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin},{MixRoles.Owner}")]
    public class MixDatabaseController
        : MixRestfulApiControllerBase<Lib.ViewModels.MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        private readonly ApplicationLifetime _applicationLifetime;
        private MixDbService _mixDbService;
        public MixDatabaseController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService, MixDbService mixDbService, ApplicationLifetime applicationLifetime)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {
            _mixDbService = mixDbService;
            _applicationLifetime = applicationLifetime;
        }

        #region Routes

        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<MixDatabaseViewModel>> GetByName(string name)
        {
            var result = await Repository.GetSingleAsync(m => m.SystemName == name);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("migrate/{name}")]
        public async Task<ActionResult> Migrate(string name)
        {
            //await _mixDbService.BackupDatabase(name);
            var result = await _mixDbService.MigrateDatabase(name);
            //await _mixDbService.RestoreFromLocal(name);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("backup/{name}")]
        public ActionResult Backup(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Backup, name);
            QueueService.PushQueue(msg);
            return Ok();
        }

        [HttpGet("restore/{name}")]
        public ActionResult RestoreAsync(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Restore, name);
            QueueService.PushQueue(msg);
            return Ok();
        }

        #endregion
        #region Overrides
        protected override async Task UpdateHandler(int id, MixDatabaseViewModel data, CancellationToken cancellationToken)
        {
            try
            {
                await _mixDbService.BackupDatabase(data.SystemName, cancellationToken);
                await base.UpdateHandler(id, data, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }
        protected override Task DeleteHandler(Lib.ViewModels.MixDatabaseViewModel data, CancellationToken cancellationToken = default)
        {
            if (data.Type == MixDatabaseType.System)
            {
                throw new MixException($"Cannot Delete System Database: {data.SystemName}");
            }
            return base.DeleteHandler(data, cancellationToken);
        }
        #endregion


    }
}
