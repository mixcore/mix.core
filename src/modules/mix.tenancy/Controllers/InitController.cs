using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Identity.Constants;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Enums;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.Tenancy.Controllers
{
    [Route("api/v2/rest/mix-tenancy/setup")]
    [ApiController]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;
        private readonly MixThemeImportService _importService;
        private readonly HttpService _httpService;
        protected readonly IHubContext<MixThemeHub> _hubContext;
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixThemeImportService importService,
            HttpService httpService, IHubContext<MixThemeHub> hubContext)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            
            _initCmsService = initCmsService;
            _importService = importService;
            _httpService = httpService;
            _hubContext = hubContext;
        }


        #region Routes

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
                    model.PrimaryDomain ??= Request.Headers.Host;
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
        ///     - Init Owner Account
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

        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<bool>> InstallTheme([FromBody] JObject theme)
        {
            var progress = new Progress<int>();
            var percent = 0;
            progress.ProgressChanged += (sender, value) =>
            {
                if (value > percent)
                {
                    percent = value;
                    _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Downloading", 200, value);

                }
            };

            await _importService.DownloadThemeAsync(theme, progress, _httpService);
            GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.SelectTheme;
            GlobalConfigService.Instance.SaveSettings();
            return Ok();
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
                siteData.Specificulture ??= GlobalConfigService.Instance.DefaultCulture;
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
                dto.TenantData.PrimaryDomain ??= Request.Headers.Host;
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

        #region Helpers
        public virtual async Task AlertAsync<T>(IClientProxy clients, string action, int status, T message)
        {
            var address = Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(address))
            {
                address = Request.Host.Value;
            }
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("id", Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", _mixIdentityService.GetClaim(User, MixClaims.Username)),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };

            //It's not possible to configure JSON serialization in the JavaScript client at this time (March 25th 2020).
            //https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.1&tabs=dotnet
            await clients.SendAsync(
                HubMethods.ReceiveMethod, logMsg.ToString(Formatting.None));
        }
        #endregion
    }
}
