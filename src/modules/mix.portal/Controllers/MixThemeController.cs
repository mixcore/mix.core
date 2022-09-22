using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using System.Text.RegularExpressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-theme")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixThemeController
        : MixRestApiControllerBase<MixThemeViewModel, MixCmsContext, MixTheme, int>
    {
        protected readonly IHubContext<MixThemeHub> _hubContext;
        private readonly HttpService _httpService;
        private readonly MixThemeExportService _exportService;
        private readonly MixThemeImportService _importService;

        public MixThemeController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixThemeImportService importService,
            MixThemeExportService exportService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService,
            HttpService httpService,
            IHubContext<MixThemeHub> hubContext)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {

            _exportService = exportService;
            _importService = importService;
            _httpService = httpService;
            _hubContext = hubContext;
        }

        // POST api/theme
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixThemeViewModel>> Save(MixThemeViewModel data)
        {
            data.SetUowInfo(_uow);
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            await data.SaveAsync();
            return Ok(data);
        }

        [HttpPost("export")]
        public async Task<ActionResult<SiteDataViewModel>> ExportThemeAsync([FromBody] ExportThemeDto dto)
        {
            var siteData = await _exportService.ExportTheme(dto);
            return Ok(siteData);
        }

        [HttpPost("load-theme")]
        public ActionResult<SiteDataViewModel> LoadTheme([FromForm] IFormFile theme)
        {
            _importService.ExtractTheme(theme);
            var siteData = _importService.LoadSchema();
            return Ok(siteData);
        }

        [HttpPost("import-theme")]
        public async Task<ActionResult<SiteDataViewModel>> ImportThemeAsync([FromBody] SiteDataViewModel siteData)
        {
            if (ModelState.IsValid)
            {
                siteData.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
                siteData.Specificulture ??= CurrentTenant.Configurations.DefaultCulture;
                var result = await _importService.ImportSelectedItemsAsync(siteData);
                return Ok(result);
            }
            return BadRequest(ModelState);
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
            return Ok();
        }

        [HttpPost]
        [Route("install-portal")]
        public async Task<ActionResult<string>> InstallPortal([FromBody] JObject theme)
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

            string name = SeoHelper.GetSEOString(theme.Value<string>("title"));
            string folder = $"{MixFolders.WebRootPath}/{MixFolders.PortalApps}/{name}";
            string appfolder = $"{MixFolders.PortalApps}/{name}";
            await _importService.DownloadThemeAsync(theme, progress, _httpService, folder);
            var indexFile = MixFileHelper.GetFileByFullName($"{folder}/index.html");
            Regex regex = new("((?<=src=\")|(?<=href=\"))(?!(http[^\\s]+))(.+?)(\\.+?)");

            if (indexFile.Content != null && regex.IsMatch(indexFile.Content))
            {
                indexFile.Content = regex.Replace(indexFile.Content, $"/{appfolder}/$3$4")
                    .Replace("options['baseHref']", $"'{appfolder}'");

            }
            MixFileHelper.SaveFile(indexFile);
            return Ok(appfolder);
        }

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
