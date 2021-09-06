using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class Helper
    {
        public static async Task<RepositoryResponse<string>> ExportTheme(
            int id, SiteStructureViewModel data,
            string culture, string scheme, string host)
        {
            var getTheme = await ReadViewModel.Repository.GetSingleModelAsync(
                 theme => theme.Id == id).ConfigureAwait(false);

            //path to temporary folder
            string tempPath = $"{MixFolders.WebRootPath}/{MixFolders.ExportFolder}/Themes/{getTheme.Data.Name}/temp";
            string outputPath = $"{MixFolders.ExportFolder}/Themes/{getTheme.Data.Name}";
            data.ThemeName = getTheme.Data.Name;
            data.Specificulture = culture;
            var result = data.ExportSelectedItemsAsync();
            if (result.IsSucceed)
            {
                string domain = MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain);
                string accessFolder = $"{MixFolders.SiteContentAssetsFolder}/{getTheme.Data.Name}/assets";
                string content = JObject.FromObject(data).ToString()
                    .Replace(accessFolder, "[ACCESS_FOLDER]")
                    .Replace($"/{culture}/", "/[CULTURE]/")
                    .Replace($"/{data.ThemeName}/", "/[THEME_NAME]/");
                if (!string.IsNullOrEmpty(domain))
                {
                    content = content.Replace(domain, string.Empty);
                }
                string filename = $"schema";
                var file = new FileViewModel()
                {
                    Filename = filename,
                    Extension = MixFileExtensions.Json,
                    FileFolder = $"{tempPath}/Data",
                    Content = content
                };

                // Delete Existing folder
                MixFileRepository.Instance.DeleteWebFolder(outputPath);

                if (data.IsIncludeAssets)
                {
                    // Copy current assets files
                    MixFileRepository.Instance.CopyDirectory($"{MixFolders.WebRootPath}/{getTheme.Data.AssetFolder}", $"{tempPath}/Assets");
                    // Copy current uploads files
                    MixFileRepository.Instance.CopyDirectory($"{MixFolders.WebRootPath}/{getTheme.Data.UploadsFolder}", $"{tempPath}/Uploads");
                }

                // Save Site Structures
                MixFileRepository.Instance.SaveFile(file);

                // Zip to [theme_name].zip ( wwwroot for web path)
                string filePath = MixFileRepository.Instance.ZipFolder($"{tempPath}", outputPath, $"{getTheme.Data.Name}-{Guid.NewGuid()}");

                // Delete temp folder
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Assets");
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Uploads");
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Data");

                return new RepositoryResponse<string>()
                {
                    IsSucceed = !string.IsNullOrEmpty(outputPath),
                    Data = $"{scheme}://{host}/{filePath}"
                };
            }
            else
            {
                return result;
            }
        }

        public static async Task<RepositoryResponse<InitViewModel>> InitTheme(string model, string username, string culture, IFormFile assets, IFormFile theme)
        {
            var json = JObject.Parse(model);
            var data = json.ToObject<InitViewModel>();
            if (data != null)
            {
                data.CreatedBy = username;
                data.Status = MixContentStatus.Published;
                string importFolder = MixFolders.ThemePackage;
                if (theme != null)
                {
                    MixFileRepository.Instance.SaveFile(theme, $"{importFolder}");
                    data.TemplateAsset = new FileViewModel(theme, importFolder);
                }
                else
                {
                    data.TemplateAsset = new FileViewModel()
                    {
                        Filename = "_blank",
                        Extension = MixFileExtensions.Zip,
                        FileFolder = MixFolders.DataFolder
                    };
                }

                data.Title = MixService.GetConfig<string>(MixAppSettingKeywords.SiteName, culture);
                data.Name = SeoHelper.GetSEOString(data.Title);
                data.Specificulture = culture;
                var result = await data.SaveModelAsync(true);
                if (result.IsSucceed)
                {
                    // MixService.SetConfig<string>(MixAppSettingKeywords.SiteName, _lang, data.Title);
                    MixService.LoadFromDatabase();
                    MixService.SetConfig("InitStatus", 3);
                    MixService.SetConfig(MixAppSettingKeywords.IsInit, false);
                    MixService.SaveSettings();
                    _ = Mix.Services.MixCacheService.RemoveCacheAsync();
                    MixService.Reload();
                }
                return result;
            }
            return new RepositoryResponse<InitViewModel>();
        }

        public static async Task<RepositoryResponse<UpdateViewModel>> InstallThemeAsync(JObject theme, string createdBy, string culture, IProgress<int> progress, HttpService httpService)
        {
            string name = theme.Value<string>("name");
            var newtheme = new UpdateViewModel()
            {
                Title = name,
                CreatedBy = createdBy,
                Specificulture = culture,
                Status = MixContentStatus.Published,
                TemplateAsset = new FileViewModel()
                {
                    Filename = name,
                    Extension = MixFileExtensions.Zip,
                    FileFolder = MixFolders.ThemePackage
                }
            };

            var cancellationToken = new CancellationToken();

            await httpService.DownloadAsync(
                theme.Value<string>("source"),
                newtheme.TemplateAsset.FileFolder,
                newtheme.TemplateAsset.Filename, MixFileExtensions.Zip,
                progress, cancellationToken);
            return await newtheme.SaveModelAsync(true);
        }

        public static async Task<RepositoryResponse<bool>> ActivedThemeAsync(
            int themeId,
            string themeName,
            string culture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true };
                var saveResult = await SaveNewConfigAsync(MixAppSettingKeywords.ThemeName, themeName, culture, context, transaction);
                if (saveResult.IsSucceed)
                {
                    saveResult = await SaveNewConfigAsync(MixAppSettingKeywords.ThemeFolder, themeName, culture, context, transaction);
                }

                ViewModelHelper.HandleResult(saveResult, ref result);

                if (result.IsSucceed)
                {
                    saveResult = await SaveNewConfigAsync(MixAppSettingKeywords.ThemeId, themeId.ToString(), culture, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<bool>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    context.Dispose();
                }
            }
        }

        private static async Task<RepositoryResponse<MixConfigurations.UpdateViewModel>> SaveNewConfigAsync(string keyword, string value, string culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            MixConfigurations.UpdateViewModel config = (await MixConfigurations.UpdateViewModel.Repository.GetSingleModelAsync(
                           c => c.Keyword == keyword && c.Specificulture == culture
                           , context, transaction)).Data;
            if (config == null)
            {
                config = new MixConfigurations.UpdateViewModel()
                {
                    Keyword = keyword,
                    Specificulture = culture,
                    Category = "Site",
                    DataType = MixDataType.Text,
                    Description = "Cms Theme",
                    Value = value
                };
            }
            else
            {
                config.Property.Value = value;
            }
            return await config.SaveModelAsync(false, context, transaction);
        }
    }
}