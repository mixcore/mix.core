using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Mix.Theme.Domain.ViewModels;
using Mix.Theme.Domain.ViewModels.Init;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.Helpers
{
    public class ThemeHelper
    {
        public static async Task<RepositoryResponse<string>> ExportTheme(
            int id, SiteStructureViewModel data,
            string culture, string scheme, string host)
        {
            var getTheme = await InitThemeViewModel.Repository.GetSingleModelAsync(
                 theme => theme.Id == id).ConfigureAwait(false);

            //path to temporary folder
            string tempPath = $"{MixFolders.WebRootPath}/{MixFolders.ExportFolder}/Themes/{getTheme.Data.Name}/temp";
            string outputPath = $"{MixFolders.ExportFolder}/Themes/{getTheme.Data.Name}";
            data.ThemeName = getTheme.Data.Name;
            data.Specificulture = culture;
            var result = data.ExportSelectedItemsAsync();
            if (result.IsSucceed)
            {
                string domain = MixService.GetConfig<string>(MixAppSettingKeywords.Domain);
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
                // Copy current templates file
                MixFileRepository.Instance.CopyDirectory($"{getTheme.Data.TemplateFolder}", $"{tempPath}/Templates");
                // Copy current assets files
                MixFileRepository.Instance.CopyDirectory($"{MixFolders.WebRootPath}/{getTheme.Data.AssetFolder}", $"{tempPath}/Assets");
                // Copy current uploads files
                MixFileRepository.Instance.CopyDirectory($"{MixFolders.WebRootPath}/{getTheme.Data.UploadsFolder}", $"{tempPath}/Uploads");
                // Save Site Structures
                MixFileRepository.Instance.SaveFile(file);

                // Zip to [theme_name].zip ( wwwroot for web path)
                string filePath = MixFileRepository.Instance.ZipFolder($"{tempPath}", outputPath, $"{getTheme.Data.Name}-{Guid.NewGuid()}");

                // Delete temp folder
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Assets");
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Uploads");
                MixFileRepository.Instance.DeleteWebFolder($"{outputPath}/Templates");
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

        public static async Task<RepositoryResponse<InitThemeViewModel>> InitTheme(
            string model, string userName, string culture, IFormFile assets, IFormFile theme)
        {
            var json = JObject.Parse(model);
            var data = json.ToObject<InitThemeViewModel>();
            if (data != null)
            {
                data.CreatedBy = userName;
                data.Status = MixContentStatus.Published;
                string importFolder = $"{MixFolders.ImportFolder}/" +
                    $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}";
                if (theme != null)
                {
                    MixFileRepository.Instance.SaveWebFile(theme, $"{importFolder}");
                    data.TemplateAsset = new FileViewModel(theme, importFolder);
                }
                else
                {
                    if (data.IsCreateDefault)
                    {
                        data.TemplateAsset = new FileViewModel()
                        {
                            Filename = "default",
                            Extension = MixFileExtensions.Zip,
                            FileFolder = MixFolders.ImportFolder
                        };
                    }
                    else
                    {
                        data.TemplateAsset = new FileViewModel()
                        {
                            Filename = "default_blank",
                            Extension = MixFileExtensions.Zip,
                            FileFolder = MixFolders.ImportFolder
                        };
                    }
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
            return new RepositoryResponse<InitThemeViewModel>();
        }

        public static async Task<RepositoryResponse<InitThemeViewModel>> InstallThemeAsync(
            JObject theme, string createdBy, string culture, IProgress<int> progress, HttpService httpService)
        {
            string name = theme.Value<string>("title");
            var newtheme = new InitThemeViewModel()
            {
                Title = name,
                CreatedBy = createdBy,
                Specificulture = culture,
                Status = MixContentStatus.Published,
                TemplateAsset = new FileViewModel()
                {
                    Filename = name,
                    Extension = MixFileExtensions.Zip,
                    FileFolder = $"{MixFolders.ImportFolder}/{DateTime.UtcNow.ToShortDateString()}/{name}"
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

        public static async Task<RepositoryResponse<bool>> ImportLanguages(List<MixLanguage> arrLanguage, string destCulture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);

            try
            {
                foreach (var item in arrLanguage)
                {
                    var lang = new MixLanguageViewModel(item, context, transaction);
                    lang.Specificulture = destCulture;
                    lang.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await lang.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<bool>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                if (isRoot)
                {
                    context.Dispose();
                }
            }
            return result;
        }

        public static async Task<RepositoryResponse<bool>> ImportConfigurations(List<MixConfiguration> arrConfiguration, string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                foreach (var item in arrConfiguration)
                {
                    var conf = new MixConfigurationViewModel(item, context, transaction);
                    conf.CreatedDateTime = DateTime.UtcNow;
                    conf.Specificulture = destCulture;
                    var saveResult = await conf.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }
                }
                result.Data = true;
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<bool>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    context.Dispose();
                }
            }
            return result;
        }


        private static async Task<RepositoryResponse<MixConfigurationViewModel>> SaveNewConfigAsync(
            string keyword, string value, string culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            MixConfigurationViewModel config = (await MixConfigurationViewModel.Repository.GetSingleModelAsync(
                           c => c.Keyword == keyword && c.Specificulture == culture
                           , context, transaction)).Data;
            if (config == null)
            {
                config = new MixConfigurationViewModel()
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
