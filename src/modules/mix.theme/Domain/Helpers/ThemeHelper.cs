﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.ViewModels;
using Mix.Theme.Domain.ViewModels.Init;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Database.Entities.Cms.v2;

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
                string domain = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain);
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

        public static async Task<RepositoryResponse<InitThemeViewModel>> InitTheme(InitThemePackageDto request, string userName, string culture)
        {
            var data = request.Model;
            if (data != null)
            {
                data.CreatedBy = userName;
                data.Status = MixContentStatus.Published;
                string importFolder = $"{MixFolders.ImportFolder}/" +
                    $"{DateTime.UtcNow:dd-MM-yyyy}";
                if (request.Theme != null)
                {
                    MixFileRepository.Instance.SaveWebFile(request.Theme, $"{importFolder}");
                    data.TemplateAsset = new FileViewModel(request.Theme, importFolder);
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

                data.Title = ConfigurationService.GetConfig<string>(MixAppSettingKeywords.SiteName, culture);
                data.Name = SeoHelper.GetSEOString(data.Title);
                data.Specificulture = culture;
                var result = await data.SaveModelAsync(true);
                if (result.IsSucceed)
                {
                    // MixService.SetConfig<string>(MixAppSettingKeywords.SiteName, _lang, data.Title);
                    MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "InitStatus", 3);
                    MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit, false);
                    MixAppSettingService.SaveSettings();
                    _ = Mix.Services.MixCacheService.RemoveCacheAsync();
                    MixAppSettingService.Reload();
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
            MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
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
                UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContextV2>.HandleException<bool>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    context.Dispose();
                }
            }
        }

        public static async Task<RepositoryResponse<bool>> ImportLanguages(
            List<MixLanguageContent> arrLanguage, string destCulture
           , MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(
                _context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);

            try
            {
                foreach (var item in arrLanguage)
                {
                    item.Specificulture = destCulture;
                    var lang = new MixLanguage()
                    {
                        SystemName = item.SystemName,
                        
                        MixLanguageContents = new List<MixLanguageContent>() { 
                            item 
                        },
                        CreatedDateTime = DateTime.UtcNow
                    };
                    context.Add(lang);
                }
                await context.SaveChangesAsync();
                if (!result.IsSucceed)
                {
                    result.Exception = saveResult.Exception;
                    result.Errors = saveResult.Errors;
                    break;
                }
                UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContextV2>.HandleException<bool>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<bool>> ImportConfigurations(
            List<MixConfigurationContent> arrConfiguration, string destCulture,
            MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                foreach (var item in arrConfiguration)
                {
                    var conf = new MixConfigurationViewModel(item, context, transaction)
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        Specificulture = destCulture
                    };
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
                UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContextV2>.HandleException<bool>(ex, isRoot, transaction);
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
            string keyword, string value, string culture, MixCmsContextV2 context, IDbContextTransaction transaction)
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