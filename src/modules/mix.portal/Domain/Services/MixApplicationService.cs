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
using System.Text.RegularExpressions;

namespace Mix.Portal.Domain.Services
{
    public sealed class MixApplicationService : TenantServiceBase, IMixApplicationService
    {
        #region Constants and Fields

        private static readonly string[] ExcludeFileNames = { "jquery", "index" };
        private static readonly string AllowExtensionsPattern = "json|js|css|webmanifest|ico|png|jpg|jpeg|gif|svg|webm|mp3|mp4|wmv|otf|ttf";

        // Compiled regex patterns for better performance
        private static readonly Regex AssetPathRegex = new($@"(\""|\\'|\()([\.])?(\/)?([[a-zA-z\/\-0-9]+)((\.)({AllowExtensionsPattern}))(\""|\\'|\))", RegexOptions.Compiled);
        private static readonly Regex BasePathRegex = new(@"(\[\[?basePath\]\]?/?)", RegexOptions.Compiled);
        private static readonly Regex ApiEndpointRegex = new(@"(\[\[?apiEndpoint\]\]?/?)", RegexOptions.Compiled);
        private static readonly Regex BaseHrefRegex = new(@"(base href=(\""?)([^\"",',`]+)(\""?))", RegexOptions.Compiled);

        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private readonly IThemeService _themeService;
        private readonly IMixThemeImportService _importService;
        private readonly MixIdentityService _mixIdentityService;
        private readonly IHubContext<MixThemeHub> _hubContext;
        private readonly HttpService _httpService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        #endregion

        #region Constructor

        public MixApplicationService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IHubContext<MixThemeHub> hubContext,
            HttpService httpService,
            MixIdentityService mixIdentityService,
            IThemeService themeService,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            IMixThemeImportService importService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _cmsUow = cmsUow ?? throw new ArgumentNullException(nameof(cmsUow));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _mixIdentityService = mixIdentityService ?? throw new ArgumentNullException(nameof(mixIdentityService));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
        }

        #endregion

        #region Public Methods

        public async Task<MixApplicationViewModel> Install(MixApplicationViewModel app, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(app);

            try
            {
                var name = SeoHelper.GetSEOString(app.DisplayName);
                var deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";

                await NotifyStatusAsync("Downloading package...");
                var filePath = await DownloadPackageAsync(name, app.PackageFilePath, deployUrl, cancellationToken);

                await NotifyStatusAsync($"Extracting package {filePath}...");
                MixFileHelper.UnZipFile(filePath, deployUrl);

                await NotifyStatusAsync("Importing schema...");
                await ImportSchemaAsync($"{deployUrl}/schema", app.CreatedBy, cancellationToken);

                await NotifyStatusAsync("Saving template...");
                app.TemplateId = await SaveTemplateAsync(app.TemplateId, name, deployUrl, app.BaseHref, cancellationToken);

                await SetupApplicationAsync(app, deployUrl, filePath, cancellationToken);

                await NotifyStatusAsync("Installation completed!", isFinished: true);
                return app;
            }
            catch (Exception ex)
            {
                await NotifyErrorAsync($"Installation failed: {ex.Message}");
                throw;
            }
        }

        public async Task<MixApplicationViewModel> UpdatePackage(MixApplicationViewModel app, string packageFileUrl, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentException.ThrowIfNullOrWhiteSpace(packageFileUrl);

            try
            {
                var name = SeoHelper.GetSEOString(app.DisplayName);
                var packages = app.AppSettings.Value<JArray>("packages") ?? new JArray();
                var deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";

                var package = await DownloadPackageAsync(name, app.PackageFilePath, deployUrl, cancellationToken);
                MixFileHelper.UnZipFile(package, deployUrl);

                await ImportSchemaAsync($"{deployUrl}/schema", app.CreatedBy, cancellationToken);
                await SaveTemplateAsync(app.TemplateId, name, deployUrl, app.BaseHref, cancellationToken);

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
            ArgumentNullException.ThrowIfNull(dto);

            try
            {
                await NotifyStatusAsync($"Restoring package {dto.PackageFilePath}...");

                var app = await GetApplicationAsync(dto.AppId, cancellationToken);
                ValidatePackageFile(dto.PackageFilePath);

                var name = SeoHelper.GetSEOString(app.DisplayName);
                var deployUrl = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";

                MixFileHelper.UnZipFile(dto.PackageFilePath, deployUrl);
                await NotifyStatusAsync($"Package extracted successfully");

                await ImportSchemaAsync($"{deployUrl}/schema", app.CreatedBy, cancellationToken);
                await SaveTemplateAsync(app.TemplateId, name, deployUrl, app.BaseHref, cancellationToken);

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

        #endregion

        #region Private Methods

        private async Task<MixApplicationViewModel> GetApplicationAsync(int appId, CancellationToken cancellationToken)
        {
            var app = await MixApplicationViewModel.GetRepository(_cmsUow, CacheService)
                .GetSingleAsync(m => m.Id == appId, cancellationToken);

            return app ?? throw new MixException(MixErrorStatus.NotFound, "App Not Found");
        }

        private static void ValidatePackageFile(string packageFilePath)
        {
            if (!File.Exists(packageFilePath))
            {
                throw new MixException(MixErrorStatus.NotFound, $"Package {packageFilePath} Not Found");
            }
        }

        private async Task SetupApplicationAsync(MixApplicationViewModel app, string deployUrl, string filePath, CancellationToken cancellationToken)
        {
            app.SetUowInfo(_cmsUow, CacheService);
            app.DeployUrl = deployUrl;
            app.TenantId = CurrentTenant.Id;
            app.AppSettings["activePackage"] = filePath;
            app.AppSettings["packages"] = new JArray { filePath };

            await app.SaveAsync(cancellationToken);
        }

        private async Task ImportSchemaAsync(string schemaFolder, string requestedBy, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(schemaFolder))
                return;

            var schema = await _importService.LoadSchema(schemaFolder);
            var themeId = CurrentTenant.Themes.FirstOrDefault()?.Id;
            if (themeId.HasValue)
            {
                schema.ThemeId = themeId.Value;
            }

            if (schema?.IsValid == true)
            {
                await _importService.ImportSelectedItemsAsync(schema, requestedBy, cancellationToken);
            }
        }

        private async Task<int?> SaveTemplateAsync(int? templateId, string name, string deployUrl, string baseHref, CancellationToken cancellationToken)
        {
            try
            {
                var folders = MixFileHelper.GetTopDirectories(deployUrl);
                var topFolderPattern = string.Join('|', folders);

                templateId = await ProcessIndexFileAsync(templateId, name, deployUrl, baseHref, cancellationToken);
                await ProcessFilesAndFoldersAsync(deployUrl, deployUrl, topFolderPattern, cancellationToken);

                return templateId;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task ProcessFilesAndFoldersAsync(string deployUrl, string currentFolder, string topFolderPattern, CancellationToken cancellationToken)
        {
            var files = MixFileHelper.GetTopFiles(currentFolder, true);
            var folders = MixFileHelper.GetTopDirectories(currentFolder);

            var supportedExtensions = new[] { MixFileExtensions.Js, MixFileExtensions.Css, MixFileExtensions.Html, MixFileExtensions.Json };

            foreach (var file in files.Where(f => supportedExtensions.Contains(f.Extension)))
            {
                await ProcessFileContentAsync(file, topFolderPattern, deployUrl);
            }

            foreach (var folder in folders)
            {
                await ProcessFilesAndFoldersAsync(deployUrl, $"{currentFolder}/{folder}", topFolderPattern, cancellationToken);
            }
        }

        private async Task<int?> ProcessIndexFileAsync(int? templateId, string name, string deployUrl, string baseHref, CancellationToken cancellationToken)
        {
            try
            {
                await NotifyStatusAsync($"Processing {name}.cshtml...");

                var indexFile = MixFileHelper.GetFileByFullName($"{deployUrl}/index.html");
                if (string.IsNullOrEmpty(indexFile.Content))
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Application Package - index.html is empty");
                }

                var webPath = GetWebPath(deployUrl);
                var processedContent = ProcessIndexContent(indexFile.Content, webPath, baseHref);

                var template = await CreateOrUpdateTemplateAsync(templateId, name, processedContent, cancellationToken);

                // Save the processed index file
                indexFile.Content = processedContent;
                MixFileHelper.SaveFile(indexFile);

                await NotifyStatusAsync($"Successfully processed {name}.cshtml");
                return template.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ProcessIndexContent(string content, string webPath, string baseHref)
        {
            // Replace asset paths
            content = AssetPathRegex.Replace(content, $"$1/{webPath}/$4$5$8");

            // Replace base href if present
            if (content.Contains("base href", StringComparison.OrdinalIgnoreCase))
            {
                content = BaseHrefRegex.Replace(content, $"base href=\"{baseHref}\"");
            }

            // Replace base path placeholders
            content = BasePathRegex.Replace(content, $"/{webPath}/");

            return content;
        }

        private async Task<MixTemplateViewModel> CreateOrUpdateTemplateAsync(int? templateId, string name, string content, CancellationToken cancellationToken)
        {
            var activeTheme = await _themeService.GetActiveTheme();
            var template = await MixTemplateViewModel.GetRepository(_cmsUow, CacheService)
                .GetSingleAsync(m => m.Id == templateId, cancellationToken);

            template ??= new MixTemplateViewModel(_cmsUow)
            {
                MixThemeId = activeTheme.Id,
                FileName = $"MixApp_{name}",
                FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{activeTheme.SystemName}/{MixTemplateFolderType.Pages}",
                FolderType = MixTemplateFolderType.Pages,
                Extension = MixFileExtensions.CsHtml,
                TenantId = CurrentTenant.Id,
                Scripts = string.Empty,
                Styles = string.Empty,
            };

            template.Content = ProcessTemplateContent(content);
            await template.SaveAsync(cancellationToken);

            _queueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixViewModelChanged, MixRestAction.Post.ToString(), template);

            return template;
        }

        private static string ProcessTemplateContent(string content)
        {
            return content.Replace("@", "@@", StringComparison.Ordinal)
                         .Replace("<body>", "<body><pre id=\"app-settings-container\" style=\"display:none\">@Model.AppSettings.ToString()</pre>", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ProcessFileContentAsync(FileModel file, string folders, string deployUrl)
        {
            if (string.IsNullOrEmpty(file.Content) || ExcludeFileNames.Contains(file.Filename, StringComparer.OrdinalIgnoreCase))
                return;

            try
            {
                await NotifyStatusAsync($"Processing {file.Filename}{file.Extension}...");

                var webPath = GetWebPath(deployUrl);
                var content = file.Content;

                // Process asset paths
                content = ProcessAssetPaths(content, webPath);

                // Process folder-specific paths
                if (!string.IsNullOrEmpty(folders))
                {
                    content = ProcessFolderPaths(content, folders, webPath);
                }

                // Replace placeholders
                content = ReplaceContentPlaceholders(content, webPath);

                file.Content = content;
                MixFileHelper.SaveFile(file);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Error processing file {file.Filename}: {ex.Message}", ex);
            }
        }

        private string ProcessAssetPaths(string content, string webPath)
        {
            return AssetPathRegex.IsMatch(content) ? AssetPathRegex.Replace(content, $"$1/{webPath}/$4$5$8") : content;
        }

        private string ProcessFolderPaths(string content, string folders, string webPath)
        {
            var folderRegex = new Regex($"((\\\"|\\'|\\(|\\`)([\\.])?(\\/)?(({folders})(([^\\`\\'\\\"]+)))(\\\"|\\'|\\)|\\`))", RegexOptions.Compiled);
            return folderRegex.IsMatch(content) ? folderRegex.Replace(content, $"$2/{webPath}/$5$9") : content;
        }

        private string ReplaceContentPlaceholders(string content, string webPath)
        {
            content = BasePathRegex.Replace(content, $"/{webPath}/");
            content = ApiEndpointRegex.Replace(content, CurrentTenant.Configurations.Domain.TrimEnd('/'));
            return content;
        }

        private static string GetWebPath(string deployUrl)
        {
            return deployUrl.Replace("wwwroot", string.Empty, StringComparison.OrdinalIgnoreCase)
                           .TrimStart('/')
                           .TrimEnd('/');
        }

        private async Task<string> DownloadPackageAsync(string name, string packageUrl, string appFolder, CancellationToken cancellationToken)
        {
            try
            {
                var progress = new Progress<int>();
                var lastPercent = 0;

                progress.ProgressChanged += async (_, value) =>
                {
                    if (value > lastPercent)
                    {
                        lastPercent = value;
                        await NotifyDownloadProgressAsync(value);
                    }
                };

                var fileName = $"{name}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";
                var filePath = $"{appFolder}/{fileName}{MixFileExtensions.Zip}";

                await _httpService.DownloadAsync(packageUrl, appFolder, fileName, MixFileExtensions.Zip, progress, cancellationToken);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Failed to download package: {ex.Message}", ex);
            }
        }

        #endregion

        #region Notification Methods

        private async Task NotifyStatusAsync(string message, bool isFinished = false)
        {
            var status = isFinished ? "Finished" : "Status";
            var statusCode = isFinished ? 200 : 200;
            await AlertAsync(_hubContext.Clients.Group("Theme"), status, statusCode, message);
        }

        private async Task NotifyErrorAsync(string message)
        {
            await AlertAsync(_hubContext.Clients.Group("Theme"), "Error", 500, message);
        }

        private async Task NotifyDownloadProgressAsync(int percentage)
        {
            await AlertAsync(_hubContext.Clients.Group("Theme"), "Downloading", 200, percentage);
        }

        public async Task AlertAsync<T>(IClientProxy clients, string action, int status, T message)
        {
            var address = HttpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(address))
            {
                address = HttpContextAccessor.HttpContext?.Request.Host.Value;
            }

            var messageValue = message switch
            {
                null => string.Empty,
                string str => str,
                _ => message.ToString() ?? string.Empty
            };

            var logMsg = new JObject
            {
                ["created_at"] = DateTime.UtcNow,
                ["address"] = address,
                ["action"] = action,
                ["status"] = status,
                ["message"] = messageValue
            };

            await clients.SendAsync(HubMethods.ReceiveMethod, logMsg.ToString(Formatting.None));
        }

        #endregion
    }
}
