using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
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
            string deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
            string filePath = await DownloadPackage(name, app.PackageFilePath, deployUrl);
            _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {filePath} Successfully");
            MixFileHelper.UnZipFile(filePath, deployUrl);
            _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {filePath} Successfully");
            app.TemplateId = await SaveTemplate(app.TemplateId, name, deployUrl, app.BaseHref);
            app.SetUowInfo(_cmsUow, CacheService);
            app.DeployUrl = deployUrl;
            app.MixTenantId = CurrentTenant.Id;
            app.CreatedBy = _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext?.User, MixClaims.Username);
            app.AppSettings["activePackage"] = filePath;
            app.AppSettings["packages"] = new JArray
            {
                filePath
            };
            await app.SaveAsync();
            return app;
        }

        private async Task<int?> SaveTemplate(int? templateId, string name, string deployUrl, string baseHref)
        {
            try
            {
                templateId = await ReplaceIndex(templateId, name, deployUrl, baseHref);
                var files = MixFileHelper.GetTopFiles(deployUrl, true);
                var folders = string.Join('|', MixFileHelper.GetTopDirectories(deployUrl));
                foreach (var file in files)
                {
                    if (file.Extension == MixFileExtensions.Js || file.Extension == MixFileExtensions.Css)
                    {
                        await ReplaceContent(file, folders, deployUrl);
                    }
                }
                return templateId;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task<int?> ReplaceIndex(int? templateId, string name, string deployUrl, string baseHref)
        {
            try
            {
                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Modifying {name}.cshtml");

                var indexFile = MixFileHelper.GetFileByFullName($"{deployUrl}/index.html");

                if (string.IsNullOrEmpty(indexFile.Content))
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Application Package");
                }

                
                Regex regex = new("((?<=src=\")|(?<=href=\"))(?!(http[^\\s]+))(.+?)(\\.+?)");
                Regex baseHrefRegex = new("(base href=\"(.+?)\")");
                indexFile.Content = indexFile.Content.Replace("[basePath]/", string.Empty);
                indexFile.Content = regex.Replace(indexFile.Content, $"/{deployUrl}/$3$4");
                indexFile.Content = baseHrefRegex.Replace(indexFile.Content, $"base href=\"{baseHref}\"")
                    .Replace("[baseRoute]", deployUrl)

                    .Replace("options['baseRoute']", $"'{deployUrl}'")
                    .Replace("options['baseHref']", $"'/{baseHref}'");

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
                template.Content = indexFile.Content.Replace("@", "@@")
                                                    .Replace("<body>", "<body><pre id=\"app-settings-container\" style=\"display:none\">@Model.AppSettings.ToString()</pre>");
                await template.SaveAsync();
                _queueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixViewModelChanged, MixRestAction.Post.ToString(), template);
                MixFileHelper.SaveFile(indexFile);
                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Modified {name}.cshtml successfully");
                return template.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Task ReplaceContent(FileModel file, string folders, string appFolder)
        {
            if (!string.IsNullOrEmpty(file.Content))
            {
                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Modifying {file.Filename}{file.Extension}");
                Regex rg = new($"((\\\"|\\')(({folders})))");
                Regex rgSplash = new($"((\\.\\/)({folders}))");
                Regex rgSplash1 = new($"((\\(\\/|\\`\\/)(({folders})))");
                if (rgSplash.IsMatch(file.Content))
                {
                    file.Content = rgSplash.Replace(file.Content, $"/{appFolder}/$3");
                }
                if (rgSplash1.IsMatch(file.Content))
                {
                    file.Content = rgSplash1.Replace(file.Content, $"$2{appFolder}/$3");
                }
                if (rg.IsMatch(file.Content))
                {
                    file.Content = rg.Replace(file.Content, $"$2/{appFolder}/$3");
                }
                file.Content = file.Content.Replace("[basePath]", $"/{appFolder}");

                MixFileHelper.SaveFile(file);
            }
            return Task.CompletedTask;
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

                string deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
                string package = await DownloadPackage(name, app.PackageFilePath, deployUrl);
                MixFileHelper.UnZipFile(package, deployUrl);
                await SaveTemplate(app.TemplateId, name, deployUrl, app.BaseHref);
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
                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {dto.PackageFilePath}");
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
                string deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
                MixFileHelper.UnZipFile(dto.PackageFilePath, deployUrl);

                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {dto.PackageFilePath} Successfully");
                await SaveTemplate(app.TemplateId, name, deployUrl, app.BaseHref);
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
