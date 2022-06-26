using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Entities.Cache;
using Mix.RepoDb.Services;
using Mix.RepoDb.ViewModels;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseController
        : MixRestApiControllerBase<Lib.ViewModels.MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
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
            IQueueService<MessageQueueModel> queueService, MixDbService mixDbService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _mixDbService = mixDbService;
        }

        #region Routes

        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<Lib.ViewModels.MixDatabaseViewModel>> GetByName(string name)
        {
            var result = await _repository.GetSingleAsync(m => m.SystemName == name);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("migrate/{id}")]
        public async Task<ActionResult> Migrate(int id)
        {
            var result = await _mixDbService.MigrateDatabase(id);
            return result ? Ok() : BadRequest();
        }

        #endregion
        #region Overrides

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
