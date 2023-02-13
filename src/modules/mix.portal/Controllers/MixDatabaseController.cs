using Microsoft.AspNetCore.Mvc;
using Mix.RepoDb.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]

    public class MixDatabaseController
        : MixRestfulApiControllerBase<MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        private readonly MixDbService _mixDbService;
        public MixDatabaseController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService, MixDbService mixDbService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService)
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
        [HttpGet("migrate/{name}")]
        public async Task<ActionResult> Migrate(string name)
        {
            //await _mixDbService.BackupDatabase(name);
            var result = await _mixDbService.MigrateDatabase(name);
            //await _mixDbService.RestoreFromLocal(name);
            return result ? Ok() : BadRequest();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("backup/{name}")]
        public ActionResult Backup(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Backup, name);
            QueueService.PushQueue(msg);
            return Ok();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("restore/{name}")]
        public ActionResult RestoreAsync(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Restore, name);
            QueueService.PushQueue(msg);
            return Ok();
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpGet("update/{name}")]
        public ActionResult UpdateAsync(string name)
        {
            var msg = new MessageQueueModel(MixQueueTopics.MixRepoDb, MixRepoDbQueueAction.Update, name);
            QueueService.PushQueue(msg);
            return Ok();
        }

        #endregion
        #region Overrides
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
        protected override Task DeleteHandler(Lib.ViewModels.MixDatabaseViewModel data, CancellationToken cancellationToken = default)
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
