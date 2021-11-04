using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.Services;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Abstracts;
using Mix.Heart.Repository;
using Mix.Database.Entities.Cms;
using Mix.Shared.Enums;
using Microsoft.Extensions.Configuration;

namespace Mix.Theme.Controllers
{
    [Route("api/v2/mix-theme/setup")]
    [ApiController]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;

        public InitController(
            IConfiguration configuration,
            GlobalConfigService globalConfigService, 
            MixService mixService, 
            TranslatorService translator, 
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService)
            : base(configuration, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
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
        [Route("init-site")]
        public async Task<ActionResult<bool>> InitSite([FromBody] InitCmsDto model)
        {
            if (model != null
                && _globalConfigService.AppSettings.InitStatus == InitStep.Blank)
            {
                await _initCmsService.InitSiteAsync(model);
                _globalConfigService.AppSettings.DefaultCulture = model.Culture.Specificulture;
                _globalConfigService.AppSettings.InitStatus = InitStep.InitSite;
                _globalConfigService.SaveSettings();
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
                && _globalConfigService.AppSettings.InitStatus == InitStep.InitSite)
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

            var initStatus = _globalConfigService.AppSettings.InitStatus;



            return Ok(initStatus);

        }


        #endregion Helpers
    }
}
