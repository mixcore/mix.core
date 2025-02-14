using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Shared.Extensions;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using Mix.Tenancy.Domain.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mix.Lib.Interfaces;
using Mix.Tenancy.Domain.Interfaces;
using Mix.Quartz.Interfaces;
using Mix.Auth.Models;
using Mix.Auth.Constants;
using Mix.Mq.Lib.Models;
using Mix.Shared.Models.Configurations;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Service.Models;
using Mix.Database.Services;

namespace Mix.Tenancy.Controllers
{
    [Route("api/v2/rest/mix-tenancy/setup")]
    [ApiController]
    public class InitApiController : MixTenantApiControllerBase
    {
        private readonly AppSettingsService _appSettingsService;
        private readonly GlobalSettingsService _globalSettingsService;
        private readonly DatabaseService _databaseService;
        private readonly MixEndpointService _mixEndpointService;
        private readonly IMixTenantService _mixTenantService;
        private readonly IInitCmsService _initCmsService;
        private readonly IQuartzService _quartzService;
        private readonly IMixThemeImportService _importService;
        private readonly MixConfigurationService _configService;
        private readonly HttpService _httpService;
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;

        protected readonly IHubContext<MixThemeHub> HubContext;

        public InitApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            IInitCmsService initCmsService,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixThemeImportService importService,
            IQuartzService quartzService,
            HttpService httpService,
            IHubContext<MixThemeHub> hubContext = null,
            IMixTenantService mixTenantService = null,
            MixEndpointService mixEndpointService = null,
            MixConfigurationService configService = null,
            UnitOfWorkInfo<MixCmsContext> uow = null,
            DatabaseService databaseService = null,
            AppSettingsService appSettingsService = null,
            GlobalSettingsService globalSettingsService = null)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {

            _initCmsService = initCmsService;
            _importService = importService;
            _httpService = httpService;
            HubContext = hubContext;
            _quartzService = quartzService;
            _mixTenantService = mixTenantService;
            _mixEndpointService = mixEndpointService;
            _configService = configService;
            _uow = uow;
            _databaseService = databaseService;
            _appSettingsService = appSettingsService;
            _globalSettingsService = globalSettingsService;
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
            if (model == null || Configuration.GetValue<InitStep>("InitStatus") != InitStep.Blank)
            {
                return BadRequest();
            }

            try
            {
                model.PrimaryDomain ??= Request.Headers.Host;
                await _initCmsService.InitDbContext(model);
                await _initCmsService.InitTenantAsync(model);
                await _quartzService.LoadScheduler();
                await _uow.CompleteAsync();
                await _mixTenantService.Reload();
                _mixEndpointService.SetDefaultDomain($"https://{model.PrimaryDomain}");
                _appSettingsService.SetConfig(nameof(AppSettingsModel.DatabaseProvider), model.DatabaseProvider.ToString());
                _appSettingsService.SetConfig(nameof(AppSettingsModel.InitStatus), InitStep.InitAccount);
                _appSettingsService.SaveSettings();
                return Ok();
            }
            catch (Exception ex)
            {
                _databaseService.ResetConnectionStrings();
                return BadRequest(ex.Message);
            }
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
        public async Task<ActionResult<bool>> InitAccount([FromBody] RegisterRequestModel model)
        {
            if (model == null || _appSettingsService.AppSettings.InitStatus != InitStep.InitAccount)
            {
                return BadRequest();
            }

            await _initCmsService.InitAccountAsync(model);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.InitStatus), InitStep.SelectTheme, true);
            return NoContent();
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
                    _ = AlertAsync(HubContext.Clients.Group("Theme"), "Downloading", 200, value);

                }
            };

            await _importService.DownloadThemeAsync(theme, progress, _httpService);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.InitStatus), InitStep.SelectTheme);
            return Ok();
        }



        /// <summary>
        /// When status = InitAcccount
        ///     - Upload or load default theme zip file
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("extract-theme")]
        [DisableRequestSizeLimit]
        public ActionResult<bool> ExtractThemeAsync(IFormFile theme = null)
        {
            _importService.ExtractTheme(theme);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.InitStatus), InitStep.InitTheme, true);
            return Ok();
        }

        /// <summary>
        /// When status = SelectTheme
        ///     - Load selected theme and show items will be installed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("load-theme")]
        public async Task<ActionResult<SiteDataViewModel>> LoadThemeAsync()
        {
            var data = await _importService.LoadSchema();
            data.Specificulture = _appSettingsService.AppSettings.DefaultCulture;
            return Ok(data);
        }


        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpPost("import-theme")]
        public async Task<ActionResult<SiteDataViewModel>> ImportThemeAsync([FromBody] SiteDataViewModel siteData, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            siteData.CreatedBy = User.Identity?.Name;
            siteData.Specificulture ??= CurrentTenant.Configurations.DefaultCulture;
            var result = await _importService.ImportSelectedItemsAsync(siteData, siteData.CreatedBy, cancellationToken);

            await _configService.Set(
                MixConfigurationNames.ThemeFolder,
                $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{siteData.ThemeSystemName}",
                CurrentTenant.Cultures.First().Specificulture,
                CurrentTenant.Cultures.First().Id,
                _uow, CurrentTenant.Id);

            await _configService.Set(
                MixConfigurationNames.DefaultDomain,
                CurrentTenant.PrimaryDomain,
                CurrentTenant.Cultures.First().Specificulture,
                CurrentTenant.Cultures.First().Id,
                _uow, CurrentTenant.Id);

            await _configService.Reload(CurrentTenant.Id, _uow);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.InitStatus), InitStep.Done);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.IsInit), false, true);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-init-status")]
        public ActionResult<InitStep> GetInitStatus()
        {
            return Ok(_appSettingsService.AppSettings.InitStatus);
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
                    new JProperty("id", Request.HttpContext.Connection.Id),
                    new JProperty("address", address),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress?.ToString()),
                    new JProperty("user", MixIdentityService.GetClaim(User, MixClaims.UserName)),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };

            // It's not possible to configure JSON serialization in the JavaScript client at this time (March 25th 2020).
            // https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.1&tabs=dotnet
            await clients.SendAsync(HubMethods.ReceiveMethod, logMsg.ToString(Formatting.None));
        }
        #endregion
    }
}
