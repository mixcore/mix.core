using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using Microsoft.AspNetCore.SignalR;
using Mix.Auth.Constants;
using Mix.Heart.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Portal.Domain.Interfaces;
using Mix.Shared.Helpers;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using System.Configuration;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using static NuGet.Packaging.PackagingConstants;

namespace Mix.Portal.Domain.Services
{
    public sealed class MixApplicationService : TenantServiceBase, IMixApplicationService
    {
        static string[] excludeFileNames = { "jquery", "index" };
        static string allowExtensionsPattern = "json|js|css|webmanifest|ico|png|jpg|jpeg|gif|svg|webm|mp3|mp4|wmv";
        private readonly IQueueService<MessageQueueModel> _queueService;
        private readonly IThemeService _themeService;
        private readonly IMixThemeImportService _importService;
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
            IMixTenantService mixTenantService,
            IMixThemeImportService importService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _cmsUow = cmsUow;
            _hubContext = hubContext;
            _httpService = httpService;
            _mixIdentityService = mixIdentityService;
            _themeService = themeService;
            _queueService = queueService;
            _importService = importService;
        }
        public async Task<MixApplicationViewModel> Install(MixApplicationViewModel app, CancellationToken cancellationToken = default)
        {
            string name = SeoHelper.GetSEOString(app.DisplayName);
            string deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
            string filePath = await DownloadPackage(name, app.PackageFilePath, deployUrl);
            _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {filePath} Successfully");
            MixFileHelper.UnZipFile(filePath, deployUrl);
            _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Extract Package {filePath} Successfully");

            await ImportSchema($"{deployUrl}/schema", cancellationToken);
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
            _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Finished", 200, string.Empty);
            return app;
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
                
                await ImportSchema($"{deployUrl}/schema", cancellationToken);
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
                
                await ImportSchema($"{deployUrl}/schema", cancellationToken);
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

        private async Task ImportSchema(string schemaFolder, CancellationToken cancellationToken)
        {
            if (Directory.Exists(schemaFolder))
            {
                var schema = await _importService.LoadSchema(schemaFolder);
                schema.ThemeId = CurrentTenant.Themes.FirstOrDefault().Id;
                if (schema != null && schema.IsValid)
                {
                    await _importService.ImportSelectedItemsAsync(schema);
                }
            }
        }

        private async Task<int?> SaveTemplate(int? templateId, string name, string deployUrl, string baseHref)
        {
            try
            {
                var folders = MixFileHelper.GetTopDirectories(deployUrl);
                var topFolderPattern = string.Join('|', folders);
                templateId = await ReplaceIndex(templateId, name, deployUrl, baseHref);

                await ModifyFilesAndFolders(deployUrl, deployUrl, topFolderPattern);

                return templateId;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task ModifyFilesAndFolders(string deployUrl, string topFolder, string topFolderPattern)
        {
            var files = MixFileHelper.GetTopFiles(topFolder, true);
            var folders = MixFileHelper.GetTopDirectories(topFolder);

            foreach (var file in files)
            {
                switch (file.Extension)
                {
                    case MixFileExtensions.Js:
                    case MixFileExtensions.Css:
                    case MixFileExtensions.Html:
                    case MixFileExtensions.Json:
                        await ReplaceContent(file, topFolderPattern, deployUrl);
                        break;
                }
            }
            foreach (var folder in folders)
            {
                await ModifyFilesAndFolders(deployUrl, $"{topFolder}/{folder}", topFolderPattern);
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


                Regex regex = new($"((\\\"|\\'|\\(\\/|\\`)(\\.)?(\\/)?(([0-9a-zA-Z\\/\\.\\$\\{{\\}}_-])+)\\.({allowExtensionsPattern})(\\\"|\\'|\\)|\\`))");
                Regex baseHrefRegex = new("(base href=\"(.{0,})\")");
                Regex basePathRegex = new("(\\[\\[?basePath\\]\\]?\\/?)");
                indexFile.Content = regex.Replace(indexFile.Content, $"$2/{deployUrl}/$5.$7$2");
                indexFile.Content = baseHrefRegex.Replace(indexFile.Content, $"base href=\"{baseHref}\"");
                indexFile.Content = basePathRegex.Replace(indexFile.Content, $"/{deployUrl}/");

                var activeTheme = await _themeService.GetActiveTheme();
                MixTemplateViewModel template = await MixTemplateViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.Id == templateId);
                template ??= new(_cmsUow)
                {
                    MixThemeId = activeTheme.Id,
                    FileName = $"MixApp_{name}",
                    FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{activeTheme.SystemName}/{MixTemplateFolderType.Pages}",
                    FolderType = MixTemplateFolderType.Pages,
                    Extension = MixFileExtensions.CsHtml,
                    MixTenantId = CurrentTenant.Id,
                    Scripts = string.Empty,
                    Styles = string.Empty,
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

        private Task ReplaceContent(FileModel file, string folders, string deployUrl)
        {
            if (!string.IsNullOrEmpty(file.Content) && !excludeFileNames.Contains(file.Filename))
            {
                try
                {
                    _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Status", 200, $"Modifying {file.Filename}{file.Extension}");
                    Regex rg = new($"((\\\"|\\'|\\(\\/|\\`)(\\.)?(\\/)?(([0-9a-zA-Z\\/\\.\\$\\{{\\}}_-])+)\\.({allowExtensionsPattern})(\\\"|\\'|\\)|\\`))");
                    Regex basePathRegex = new("(\\[\\[?basePath\\]\\]?\\/?)");
                    Regex apiEndpointRegex = new("(\\[\\[?apiEndpoint\\]\\]?\\/?)");
                    if (rg.IsMatch(file.Content))
                    {
                        file.Content = rg.Replace(file.Content, $"$2/{deployUrl}/$5.$7$2");
                    }
                    if (!string.IsNullOrEmpty(folders))
                    {
                        rg = new($"((\\\"|\\'|\\(|\\`)(\\.)?(\\/)?({folders})(([0-9a-zA-Z\\/\\._-])+)(\\\"|\\'|\\(|\\`))");
                        if (rg.IsMatch(file.Content))
                        {
                            file.Content = rg.Replace(file.Content, $"$2/{deployUrl}/$5$6$2");
                        }
                    }

                    file.Content = basePathRegex.Replace(file.Content, $"/{deployUrl}/");
                    file.Content = apiEndpointRegex.Replace(file.Content, $"{CurrentTenant.Configurations.Domain.TrimEnd('/')}");

                    MixFileHelper.SaveFile(file);
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.ServerError, ex);
                }
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
                    //new JProperty("id",  HttpContextAccessor.HttpContext?.Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    //new JProperty("ip_address",  HttpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    //new JProperty("user", _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext.User, MixClaims.Username)),
                    //new JProperty("request_url", HttpContextAccessor.HttpContext.Request.Path.Value),
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
