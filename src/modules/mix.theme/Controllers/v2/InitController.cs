using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Identity.Models;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using System;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Theme.Domain.Dtos;

namespace Mix.Theme.Controllers.v2
{
    [Route("api/v2/mix-theme/setup")]
    public class InitController : MixApiControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public InitController(ILogger<MixApiControllerBase> logger,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator) : base(logger, appSettingService, mixService, translator)
        {
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
        public async Task<ActionResult<bool>> Step1([FromBody] InitCmsDto model)
        {
            if (model != null 
                && _appSettingService.GetConfig<int>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus) == 0)
            {
                return await InitSiteAsync(model);
            }
            return BadRequest();
        }

        private Task<ActionResult<bool>> InitSiteAsync(InitCmsDto model)
        {
            throw new NotImplementedException();
        }

        #endregion Helpers
    }
}
