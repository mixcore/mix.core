using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Base;
using Mix.Heart.Repository;
using Mix.Database.Entities.Cms;
using Mix.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;

namespace Mix.Tenancy.Controllers
{
    [Route("api/v2/rest/mix-tenancy/setup")]
    [ApiController]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;

        public InitController(
            IConfiguration configuration,

            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _initCmsService = initCmsService;
        }


        #region Post

        /// <summary>
        /// When status = 0
        ///     - Init Cms Database
        ///     - Init Cms Site
        ///     - Init Selected Culture as default
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-tenant")]
        public async Task<ActionResult<bool>> InitTenant([FromBody] InitCmsDto model)
        {
            if (model != null
                && GlobalConfigService.Instance.AppSettings.InitStatus == InitStep.Blank)
            {
                await _initCmsService.InitTenantAsync(model);
                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// When status = 1
        ///     - Init Account Database
        ///     - Init Superadmin Account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-account")]
        public async Task<ActionResult<bool>> InitAccount([FromBody] RegisterViewModel model)
        {
            if (model != null
                && GlobalConfigService.Instance.AppSettings.InitStatus == InitStep.InitTenant)
            {
                await _initCmsService.InitAccountAsync(model);
                return NoContent();
            }
            return BadRequest();
        }

        /// <returns status> init status </returns>

        [HttpGet]
        [Route("get-init-status")]
        public ActionResult<InitStep> GetInitStatus()
        {
            var initStatus = GlobalConfigService.Instance.AppSettings.InitStatus;
            return Ok(initStatus);
        }


        #endregion Helpers
    }
}
