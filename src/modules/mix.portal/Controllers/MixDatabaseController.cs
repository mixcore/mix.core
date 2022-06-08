using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Entities.Cache;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseController
        : MixRestApiControllerBase<MixDatabaseViewModel, MixCmsContext, MixDatabase, int>
    {
        public MixDatabaseController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            GenericUnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            GenericUnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {

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

        #endregion
        #region Overrides

        protected override Task DeleteHandler(MixDatabaseViewModel data)
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
