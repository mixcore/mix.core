using Mix.Lib.ViewModels;
using Mix.Heart.Exceptions;
using Mix.Lib.Dtos;
using Mix.Shared.Enums;
using System.Linq.Expressions;
using Mix.Heart.Extensions;
using Newtonsoft.Json.Linq;
using Mix.Heart.Repository;
using Mix.Heart.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Newtonsoft.Json;

namespace Mix.Lib.Services
{
    public class MixThemeExportService
    {
        private readonly Repository<MixCmsContext, MixTheme, int, MixThemeViewModel> _themeRepository;
        private readonly MixCmsContext _context;
        private SiteDataViewModel _siteData;
        private ExportThemeDto _dto;
        private MixThemeViewModel _exporTheme;
        private string tempPath;
        private string outputPath;
        private string webPath;
        private string fileName;

        public MixThemeExportService(MixCmsContext context)
        {
            _context = context;
            _themeRepository = MixThemeViewModel.GetRepository(new UnitOfWorkInfo(_context));
        }

        #region Export

        public async Task<string> ExportTheme(ExportThemeDto request)
        {
            _dto = request;
            _siteData = new();
            _exporTheme = await _themeRepository.GetSingleAsync(
                m => m.Id == request.ThemeId);

            //path to temporary folder
            fileName = $"{_exporTheme.SystemName}-{Guid.NewGuid()}";
            webPath = $"{MixFolders.ExportFolder}/Themes/{_exporTheme.SystemName}";
            tempPath = $"{MixFolders.WebRootPath}/{webPath}/temp";
            outputPath = $"{MixFolders.WebRootPath}/{webPath}";

            await ExportSelectedItemsAsync();

            ExportSchema(_siteData);

            if (_dto.IsIncludeAssets)
            {
                ExportAssets();
            }

            // Zip to [theme_name].zip ( wwwroot for web path)
            return ZipTheme();
        }

        private string ZipTheme()
        {
            // Zip to [theme_name].zip ( wwwroot for web path)
            string filePath = MixFileService.Instance.ZipFolder(tempPath, outputPath, fileName);

            // Delete temp folder
            MixFileService.Instance.DeleteFolder($"{outputPath}/{MixThemeFolders.Assets}");
            MixFileService.Instance.DeleteFolder($"{outputPath}/{MixThemeFolders.Uploads}");
            MixFileService.Instance.DeleteFolder($"{outputPath}/{MixThemeFolders.Schema}");
            return $"{webPath}/{fileName}.zip";
        }

        private void ExportAssets()
        {
            if (_dto.IsIncludeAssets)
            {
                // Copy current assets files
                MixFileService.Instance.CopyFolder(
                    $"{MixFolders.WebRootPath}/{_exporTheme.AssetFolder}", $"{tempPath}/{MixThemeFolders.Assets}");
                // Copy current uploads files
                MixFileService.Instance.CopyFolder(
                    $"{MixFolders.WebRootPath}/{_exporTheme.UploadsFolder}",
                    $"{tempPath}/Uploads");
            }
        }

        private void ExportSchema(SiteDataViewModel siteData)
        {
            string filename = $"schema";
            string accessFolder = $"{MixFolders.SiteContentAssetsFolder}/{_exporTheme.SystemName}/{MixThemeFolders.Assets}";
            string content = MixHelper.SerializeObject(siteData);
            content = content
                .Replace(accessFolder, "[ACCESS_FOLDER]")
                .Replace($"/{_dto.Specificulture}/", "/[CULTURE]/")
                .Replace($"/{siteData.ThemeName}/", "/[THEME_NAME]/");
            if (!string.IsNullOrEmpty(GlobalConfigService.Instance.AppSettings.Domain))
            {
                content = content.Replace(GlobalConfigService.Instance.AppSettings.Domain, string.Empty);
            }
            FileModel schema = new()
            {
                Filename = filename,
                Extension = MixFileExtensions.Json,
                FileFolder = $"{tempPath}/{MixThemeFolders.Schema}",
                Content = content
            };

            // Save Site Structures
            MixFileService.Instance.SaveFile(schema);
        }

        public async Task<SiteDataViewModel> ExportSelectedItemsAsync()
        {
            try
            {
                if (_dto.IsExportAll)
                {
                    LoadAllSiteData();
                }

                await ExportContents();
                await ExportDatas();

                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        #region Export Datas

        private async Task ExportDatas()
        {
            await ExportPageDatas();
            await ExportModuleDatas();
            await ExportDatabaseDatas();

            if (_dto.IsIncludeConfigurations)
            {
                await ExportConfigurationDataAsync();
                await ExportLanguageDataAsync();
            }

            await ExportUrlAliasAsync();
        }

        private async Task ExportPageDatas()
        {
            await ExportPageModules();
            await ExportPagePosts();
        }

        #region Export Page Data
        private async Task ExportPageModules()
        {
            _siteData.PageModules = await _context.MixPageModuleAssociation.Where(m => _dto.Content.PageIds.Any(p => p == m.LeftId)).ToListAsync();
            var unloadModuleIds = _siteData.PageModules.Where(m => !_dto.Content.ModuleIds.Any(p => m.RightId == p)).Select(m => m.RightId);

            var modules = await _context.MixModule.Where(m => unloadModuleIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.Modules.AddRange(modules);
        }
        private async Task ExportPagePosts()
        {
            _siteData.PagePosts = await _context.MixPagePostAssociation.Where(
                    m => _dto.Data.PageContentIds.Any(p => p == m.LeftId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected Pages.
            var postContentIds = _siteData.PagePosts
                .Where(m => !_dto.Data.PostContentIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();
            var postContents = _context.MixPostContent.Where(m => postContentIds.Contains(m.Id));
            var posts = _context.MixPost.Where(m => postContents.Any(p => p.ParentId == m.Id));
            _siteData.PostContents.Union(postContents);
            _siteData.Posts.Union(posts);
        }

        #endregion Export Page

        #region Export Module Data

        private async Task ExportModuleDatas()
        {
            await ExportModuleSimpleDatas();
            await ExportModulePosts();
        }
        private async Task ExportModuleSimpleDatas()
        {
            var data = await _context.MixModuleData.Where(
                    m => _dto.Data.ModuleIds.Any(p => p == m.ParentId)).ToListAsync();
            _siteData.ModuleDatas.AddRange(data);
        }

        private async Task ExportModulePosts()
        {
            _siteData.ModulePosts = await _context.MixModulePostAssociation.Where(
                    m => _dto.Data.ModuleContentIds.Any(p => p == m.LeftId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected modules.
            var postContentIds = _siteData.ModulePosts
                .Where(m => !_dto.Data.PostContentIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();
            var postContents = _context.MixPostContent.Where(m => postContentIds.Contains(m.Id));
            var posts = _context.MixPost.Where(m => postContents.Any(p=>p.ParentId == m.Id));
            _siteData.PostContents.Union(postContents);
            _siteData.Posts.Union(posts);
            
        }
        #endregion Export Module

        #region Export Database Data

        private async Task ExportDatabaseDatas()
        {
            await ExportMixDatasAsync();
            await ExportDataContentsAsync();
            await ExportValuesAsync();
            await ExportDataAssociationsAsync();
        }



        private async Task ExportMixDatasAsync()
        {
            _siteData.Datas = await _context.MixData.Where(
                        m => _dto.Data.MixDatabaseIds.Any(p => p == m.MixDatabaseId)).ToListAsync();
        }

        private async Task ExportDataContentsAsync()
        {
            _siteData.DataContents = await _context.MixDataContent.Where(
                   m => _dto.Data.MixDatabaseIds.Any(p => p == m.MixDatabaseId)).ToListAsync();
        }
        private async Task ExportValuesAsync()
        {
            _siteData.DataContentValues = await _context.MixDataContentValue.Where(
                            m => _dto.Data.MixDatabaseIds.Any(n => n == m.MixDatabaseId)).ToListAsync();
        }

        private async Task ExportDataAssociationsAsync()
        {
            Expression<Func<MixDataContentAssociation, bool>> predicate = m => _dto.Data.MixDatabaseIds.Any(p => p == m.MixDatabaseId);

            _siteData.DataContentAssociations = await _context.MixDataContentAssociation
                .Where(predicate).ToListAsync();
        }
        #endregion Export Module

        #region Export Configurations

        private async Task ExportConfigurationDataAsync()
        {
            _siteData.ConfigurationContents = await _context.MixConfigurationContent
                .Where(m => _dto.CultureIds.Contains(m.MixCultureId))
                .ToListAsync();
        }

        private async Task ExportLanguageDataAsync()
        {
            _siteData.LanguageContents = await _context.MixLanguageContent
                .Where(m => _dto.CultureIds.Contains(m.MixCultureId))
                .ToListAsync();
        }
        #endregion

        #region Export Alias
        private async Task ExportUrlAliasAsync()
        {
            Expression<Func<MixUrlAlias, bool>> predicate =
                m => m.Type == MixUrlAliasType.Page
                    && m.SourceContentId.HasValue
                    && _dto.Data.PageContentIds.Contains(m.SourceContentId.Value);
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Post
                    && m.SourceContentId.HasValue
                    && _dto.Data.PostContentIds.Contains(m.SourceContentId.Value));
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Module
                    && m.SourceContentId.HasValue
                    && _dto.Data.ModuleContentIds.Contains(m.SourceContentId.Value));

            _siteData.MixUrlAliases = await _context.MixUrlAlias.Where(predicate).ToListAsync();
        }
        #endregion
        #endregion

        #region Export Contents

        private async Task ExportContents()
        {
            await ExportPages();
            await ExportPosts();
            await ExportModules();
            await ExportDatabases();
        }

        private async Task ExportPages()
        {
            _siteData.Pages = await _context.MixPage.Where(m => _dto.Content.PageIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.PageContents = await _context.MixPageContent.Where(m => _dto.Content.PageContentIds.Any(p => p == m.Id)).ToListAsync();
        }
        private async Task ExportModules()
        {
            _siteData.Modules = await _context.MixModule.Where(m => _dto.Content.ModuleIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.ModuleContents = await _context.MixModuleContent.Where(m => _dto.Content.ModuleContentIds.Any(p => p == m.Id)).ToListAsync();
        }
        private async Task ExportPosts()
        {
            _siteData.Posts = await _context.MixPost.Where(m => _dto.Content.PostIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.PostContents = await _context.MixPostContent.Where(m => _dto.Content.PostContentIds.Any(p => p == m.Id)).ToListAsync();
        }
       
        private async Task ExportDatabases()
        {
            _siteData.MixDatabases = await _context.MixDatabase.Where(m => _dto.Content.MixDatabaseIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.MixDatabaseColumns = await _context.MixDatabaseColumn.Where(m => _dto.Content.MixDatabaseIds.Any(p => p == m.MixDatabaseId)).ToListAsync();
        }

        #endregion

        private void LoadAllSiteData()
        {
            _siteData.Posts = _context.MixPost.ToList();
            _siteData.Pages = _context.MixPage.ToList();
            _siteData.Modules = _context.MixModule.ToList();
            _siteData.MixDatabases = _context.MixDatabase.ToList();
            _siteData.Templates = _context.MixViewTemplate.ToList();
            _siteData.Configurations = _context.MixConfiguration.ToList();
            _siteData.Languages = _context.MixLanguage.ToList();
        }

        //private void ExportAdditionalData()
        //{
        //    Expression<Func<MixDataContentAssociation, bool>> predicate =
        //        m => m.IntParentId.HasValue &&
        //            (
        //                (_dto.Data.PageContentIds.Any(p => p == m.IntParentId.Value)
        //                && m.ParentType == MixDatabaseParentType.Page)
        //                || (_dto.Data.PostContentIds.Any(p => p == m.IntParentId.Value)
        //                    && m.ParentType == MixDatabaseParentType.Post)
        //                || (_dto.Data.ModuleContentIds.Any(p => p == m.IntParentId.Value)
        //                    && m.ParentType == MixDatabaseParentType.Module));
        //    var associations = _context.MixDataContentAssociation
        //            .Where(predicate);
        //}

        #endregion Export
    }
}
