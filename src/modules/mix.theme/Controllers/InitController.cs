using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.Enums;
using Mix.Theme.Domain.Services;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Abstracts;
using Mix.Heart.Repository;
using Mix.Database.Entities.Cms;
using Mix.Identity.Services;

namespace Mix.Theme.Controllers
{
    [Route("api/v2/mix-theme/setup")]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;

        public InitController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService, 
            MixService mixService, 
            TranslatorService translator, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
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
                && _globalConfigService.GetConfig<int>(MixAppSettingKeywords.InitStatus) == 0)
            {
                await _initCmsService.InitSiteAsync(model);
                _globalConfigService.SetConfig(MixAppSettingKeywords.DefaultCulture, model.Culture.Specificulture);
                _globalConfigService.SetConfig(MixAppSettingKeywords.InitStatus, InitStep.InitSite);
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
                && _globalConfigService.GetEnumConfig<InitStep>(
                    MixAppSettingKeywords.InitStatus) == InitStep.InitSite)
            {
                await _initCmsService.InitAccountAsync(model);
                _globalConfigService.SetConfig(MixAppSettingKeywords.InitStatus, InitStep.InitAccount);
                return NoContent();
            }
            return BadRequest();
        }




        #endregion Helpers
    }
}
