using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Database.Services;
using Mix.Identity.Constants;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Extensions;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Quartz.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;

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
    public class InitController : MixTenantApiControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly MixEndpointService _mixEndpointService;
        private readonly MixTenantService _mixTenantService;
        private readonly InitCmsService _initCmsService;
        private readonly QuartzService _quartzService;
        private readonly MixThemeImportService _importService;
        private readonly MixConfigurationService _configService;
        private readonly HttpService _httpService;
        protected readonly IHubContext<MixThemeHub> _hubContext;
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        IHostApplicationLifetime _appLifetime;
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            InitCmsService initCmsService,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixThemeImportService importService,
            IHostApplicationLifetime appLifetime,
            QuartzService quartzService,
            HttpService httpService, IHubContext<MixThemeHub> hubContext = null,
            MixTenantService mixTenantService = null, MixEndpointService mixEndpointService = null, DatabaseService databaseService = null, MixConfigurationService configService = null, UnitOfWorkInfo<MixCmsContext> uow = null)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {

            _initCmsService = initCmsService;
            _importService = importService;
            _httpService = httpService;
            _hubContext = hubContext;
            _appLifetime = appLifetime;
            _quartzService = quartzService;
            _mixTenantService = mixTenantService;
            _mixEndpointService = mixEndpointService;
            _databaseService = databaseService;
            _configService = configService;
            _uow = uow;
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
                    await _initCmsService.InitDbContext(model);
                    await _initCmsService.InitTenantAsync(model);
                    await _quartzService.LoadScheduler();
                    var uow = new UnitOfWorkInfo(new MixCmsContext(_databaseService));
                    await _mixTenantService.Reload(uow);
                    _session.Put(MixRequestQueryKeywords.Tenant, _mixTenantService.AllTenants.First());
                    _mixEndpointService.SetDefaultDomain($"//{model.PrimaryDomain}");
                    await uow.CompleteAsync();
                    return Ok();
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
        [DisableRequestSizeLimit]
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

        [MixAuthorize(roles: $"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
        [HttpPost("import-theme")]
        public async Task<ActionResult<SiteDataViewModel>> ImportThemeAsync([FromBody] SiteDataViewModel siteData)
        {
            if (ModelState.IsValid && GlobalConfigService.Instance.IsInit)
            {
                siteData.CreatedBy = User.Identity.Name;
                siteData.Specificulture ??= CurrentTenant.Configurations.DefaultCulture;
                var result = await _importService.ImportSelectedItemsAsync(siteData);

                await _configService.Set(
                    MixConfigurationNames.ThemeFolder,
                    $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{siteData.ThemeSystemName}", 
                    CurrentTenant.Cultures.First().Specificulture, 
                    CurrentTenant.Cultures.First().Id, 
                    _uow);
                
                await _configService.Set(
                    MixConfigurationNames.DefaultDomain, 
                    CurrentTenant.PrimaryDomain, 
                    CurrentTenant.Cultures.First().Specificulture, 
                    CurrentTenant.Cultures.First().Id, 
                    _uow);
                await _configService.Reload(_uow);

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
            if (dto == null)
            {
                return BadRequest();
            }

            try
            {
                dto.TenantData.PrimaryDomain ??= Request.Headers.Host;
                await _initCmsService.InitDbContext(dto.TenantData);
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
