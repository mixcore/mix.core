using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Repository;
using Mix.Identity.Constants;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Enums;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;

namespace Mix.Tenancy.Controllers
{
    [Route("api/v2/rest/mix-tenancy/setup")]
    [ApiController]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;
        private readonly MixThemeImportService _importService;

        public InitController(
            IConfiguration configuration,

            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixThemeImportService importService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _initCmsService = initCmsService;
            _importService = importService;
        }


        #region Post

        /// <summary>
        /// When status = Blank
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
                try
                {
                    await _initCmsService.InitTenantAsync(model);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// When status = InitTenant
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

        /// <summary>
        /// When status = InitAcccount
        ///     - Upload or load default theme zip file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("extract-theme")]
        public ActionResult<bool> ExtractThemeAsync([FromForm] IFormFile theme = null)
        {
            _importService.ExtractTheme(theme);
            GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.SelectTheme;
            GlobalConfigService.Instance.SaveSettings();
            return Ok();
        }

        /// <summary>
        /// When status = SelectTheme
        ///     - Load selected theme and show items will be installed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("load-theme")]
        public ActionResult<SiteDataViewModel> LoadThemeAsync()
        {
            var data = _importService.LoadSchema();
            return Ok(data);
        }

        [HttpPost("import-theme")]
        public async Task<ActionResult<SiteDataViewModel>> ImportThemeAsync([FromBody] SiteDataViewModel siteData)
        {
            if (ModelState.IsValid)
            {
                siteData.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
                var result = await _importService.ImportSelectedItemsAsync(siteData);
                GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.InitTheme;
                GlobalConfigService.Instance.AppSettings.IsInit = false;
                GlobalConfigService.Instance.SaveSettings();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        /// <returns status> init status </returns>

        [HttpGet]
        [Route("get-init-status")]
        public ActionResult<InitStep> GetInitStatus()
        {
            var initStatus = GlobalConfigService.Instance.AppSettings.InitStatus;
            return Ok(initStatus);
        }

        [HttpPost]
        [Route("init-full-tenant")]
        public async Task<ActionResult> InitFullTenant([FromBody] InitFullSiteDto dto)
        {
            if (dto == null || GlobalConfigService.Instance.AppSettings.InitStatus != InitStep.Blank)
            {
                return BadRequest();
            }

            try
            {
                await _initCmsService.InitTenantAsync(dto.TenantData);
                await _initCmsService.InitAccountAsync(dto.AccountData);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion Helpers
    }
}
