using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.SignalR;
using Mix.Heart.Constants;
using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Interfaces;
using Mix.Shared.Helpers;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace Mix.Portal.Domain.Services
{
    public sealed class MixApplicationService : TenantServiceBase, IMixApplicationService
    {
        private readonly IQueueService<MessageQueueModel> _queueService;
        private readonly IThemeService _themeService;
        private readonly MixIdentityService _mixIdentityService;
        private readonly IHubContext<MixThemeHub> _hubContext;
        private readonly HttpService _httpService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        public MixApplicationService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IHubContext<MixThemeHub> hubContext,
            HttpService httpService,
            MixIdentityService mixIdentityService,
            IThemeService themeService,
            IQueueService<MessageQueueModel> queueService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _cmsUow = cmsUow;
            _hubContext = hubContext;
            _httpService = httpService;
            _mixIdentityService = mixIdentityService;
            _themeService = themeService;
            _queueService = queueService;
        }
        public async Task<MixApplicationViewModel> Install(MixApplicationViewModel app, CancellationToken cancellationToken = default)
        {
            string name = SeoHelper.GetSEOString(app.DisplayName);
            string appFolder = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
            string filePath = await DownloadPackage(name, app.PackageFilePath, appFolder);
            MixFileHelper.UnZipFile(filePath, appFolder);

            var template = await SaveTemplate(app.TemplateId, name, appFolder, app.BaseRoute);
            if (template != null)
            {
                app.SetUowInfo(_cmsUow, CacheService);
                app.BaseRoute ??= name;
                app.BaseHref = appFolder;
                app.MixTenantId = CurrentTenant.Id;
                app.TemplateId = template.Id;
                app.CreatedBy = _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext?.User, MixClaims.Username);
                await app.SaveAsync();
                return app;
            }
            return null;
        }

        private async Task<MixTemplateViewModel> SaveTemplate(int? templateId, string name, string appFolder, string baseRoute)
        {
            try
            {
                var indexFile = MixFileHelper.GetFileByFullName($"{appFolder}/index.html");
                Regex regex = new("((?<=src=\")|(?<=href=\"))(?!(http[^\\s]+))(.+?)(\\.+?)");
                Regex baseHrefRegex = new("(base href=\"(.+?)\")");

                if (indexFile.Content != null && regex.IsMatch(indexFile.Content))
                {
                    indexFile.Content = regex.Replace(indexFile.Content, $"/{appFolder}/$3$4");
                    indexFile.Content = baseHrefRegex.Replace(indexFile.Content, $"base href=\"{baseRoute}\"")
                        .Replace("[baseRoute]", $"/app/{baseRoute}")
                        .Replace("base href=\"/\"", $"/app/{baseRoute}")
                        .Replace("[basePath]", appFolder)
                        .Replace("options['baseRoute']", $"'/app/{baseRoute}'")
                        .Replace("options['baseHref']", $"'{appFolder}'");

                    var activeTheme = await _themeService.GetActiveTheme();
                    MixTemplateViewModel template = await MixTemplateViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.Id == templateId);
                    template ??= new(_cmsUow)
                    {
                        MixThemeId = activeTheme.Id,
                        FileName = name,
                        FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{activeTheme.SystemName}/{MixTemplateFolderType.Pages}",
                        FolderType = MixTemplateFolderType.Pages,
                        Extension = MixFileExtensions.CsHtml,
                        MixTenantId = CurrentTenant.Id,
                        Scripts = string.Empty,
                        Styles = string.Empty,
                        CreatedBy = _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext.User, MixClaims.Username)
                    };
                    template.Content = indexFile.Content.Replace("@", "@@");
                    await template.SaveAsync();
                    _queueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixViewModelChanged, MixRestAction.Post.ToString(), template);
                    MixFileHelper.SaveFile(indexFile);

                    return template;
                }
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Application Package");
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task<string> DownloadPackage(string name, string packageUrl, string appFolder)
        {
            try
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
                var cancellationToken = new CancellationToken();

                string fileName = $"{name}-{DateTime.UtcNow.ToString("dd-MM-yyyy-hh-mm-ss")}";
                string filePath = $"{appFolder}/{fileName}{MixFileExtensions.Zip}";
                await _httpService.DownloadAsync(packageUrl, appFolder, fileName, MixFileExtensions.Zip, progress, cancellationToken);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<MixApplicationViewModel> UpdatePackage(MixApplicationViewModel app, string packageFileUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (app == null)
                {
                    throw new MixException(MixErrorStatus.NotFound, "App not found");
                }
                string name = SeoHelper.GetSEOString(app.DisplayName);
                var packages = app.AppSettings.Value<JArray>("packages") ?? new();

                string appFolder = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{app.DisplayName}";
                string package = await DownloadPackage(name, app.PackageFilePath, appFolder);
                MixFileHelper.UnZipFile(package, appFolder);
                var template = await SaveTemplate(app.TemplateId, name, appFolder, app.BaseRoute);
                packages.Add(package);
                app.AppSettings["activePackage"] = package;
                app.AppSettings["packages"] = packages;
                return app;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<MixApplicationViewModel> RestorePackage(RestoreMixApplicationPackageDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var app = await MixApplicationViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.Id == dto.AppId);
                if (app == null)
                {
                    throw new MixException(MixErrorStatus.NotFound, "App Not Found");
                }
                if (!File.Exists(dto.PackageFilePath))
                {
                    throw new MixException(MixErrorStatus.NotFound, $"Package {dto.PackageFilePath} Not Found");
                }
                string name = SeoHelper.GetSEOString(app.DisplayName);
                string appFolder = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
                MixFileHelper.UnZipFile(dto.PackageFilePath, appFolder);
                var template = await SaveTemplate(app.TemplateId, name, appFolder, app.BaseRoute);
                app.AppSettings["activePackage"] = dto.PackageFilePath;

                await app.SaveAsync(cancellationToken);

                return app;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        #region Helpers
        public async Task AlertAsync<T>(IClientProxy clients, string action, int status, T message)
        {
            var address = HttpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(address))
            {
                address = HttpContextAccessor.HttpContext?.Request.Host.Value;
            }
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("id",  HttpContextAccessor.HttpContext?.Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    new JProperty("ip_address",  HttpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext.User, MixClaims.Username)),
                    new JProperty("request_url", HttpContextAccessor.HttpContext.Request.Path.Value),
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
