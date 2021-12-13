using Mix.Lib.ViewModels;
using Mix.Heart.Exceptions;
using Mix.Lib.Dtos;
using Mix.Shared.Enums;
using System.Linq.Expressions;
using Mix.Heart.Extensions;
using Mix.Heart.Repository;
using Mix.Heart.Models;
using Microsoft.EntityFrameworkCore;

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
            _siteData.ThemeName = _exporTheme.DisplayName;
            _siteData.ThemeSystemName = _exporTheme.SystemName;
            fileName = $"{_exporTheme.SystemName}-{Guid.NewGuid()}";
            webPath = $"{MixFolders.ExportFolder}/Themes/{_exporTheme.SystemName}";
            tempPath = $"{MixFolders.WebRootPath}/{webPath}/temp";
            outputPath = $"{MixFolders.WebRootPath}/{webPath}";

            await ExportSelectedItemsAsync();

            ExportSchema(_siteData);

            ExportAssets();

            // Zip to [theme_name].zip ( wwwroot for web path)
            return ZipTheme();
        }

        private string ZipTheme()
        {
            // Zip to [theme_name].zip ( wwwroot for web path)
            string filePath = MixFileHelper.ZipFolder(tempPath, outputPath, fileName);

            // Delete temp folder
            MixFileHelper.DeleteFolder($"{outputPath}/{MixThemePackageConstants.AssetFolder}");
            MixFileHelper.DeleteFolder($"{outputPath}/{MixThemePackageConstants.UploadFolder}");
            MixFileHelper.DeleteFolder($"{outputPath}/{MixThemePackageConstants.SchemaFolder}");
            return $"{webPath}/{fileName}.zip";
        }

        private void ExportAssets()
        {
            if (_dto.IsIncludeAssets)
            {
                // Copy current assets files
                MixFileHelper.CopyFolder(
                    $"{MixFolders.WebRootPath}/{_exporTheme.AssetFolder}", $"{tempPath}/{MixThemePackageConstants.AssetFolder}");
                // Copy current uploads files
                MixFileHelper.CopyFolder(
                    $"{MixFolders.WebRootPath}/{_exporTheme.UploadsFolder}",
                    $"{tempPath}/Uploads");
            }
        }

        private void ExportSchema(SiteDataViewModel siteData)
        {
            string filename = MixThemePackageConstants.SchemaFilename;
            string accessFolder = $"{MixFolders.SiteContentAssetsFolder}/{_exporTheme.SystemName}";
            string content = MixHelper.SerializeObject(siteData);
            content = content
                .Replace(accessFolder, "[ACCESS_FOLDER]")
                .Replace($"/{siteData.ThemeName}", "/[THEME_NAME]");
            if (!string.IsNullOrEmpty(GlobalConfigService.Instance.AppSettings.Domain))
            {
                content = content.Replace(GlobalConfigService.Instance.AppSettings.Domain, string.Empty);
            }
            FileModel schema = new()
            {
                Filename = filename,
                Extension = MixFileExtensions.Json,
                FileFolder = $"{tempPath}/{MixThemePackageConstants.SchemaFolder}",
                Content = content
            };

            // Save Site Structures
            MixFileHelper.SaveFile(schema);
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
                await ExportAssociations();

                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        #region Export Associations

        private async Task ExportAssociations()
        {
            await ExportAdditionalData(_dto.Content.PostIds, MixDatabaseParentType.Post);
            await ExportPageDatas();
            await ExportModuleDatas();
            await ExportDatabaseDatas();


            await ExportUrlAliasAsync();
        }

        #region Export Page Data

        private async Task ExportPageDatas()
        {
            await ExportPageModules();
            await ExportPagePosts();
            await ExportAdditionalData(_dto.Associations.PageIds, MixDatabaseParentType.Page);
        }

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
                    m => _dto.Associations.PageContentIds.Any(p => p == m.LeftId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected Pages.
            var postContentIds = _siteData.PagePosts
                .Where(m => !_dto.Associations.PostContentIds.Any(n => m.RightId == n))
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
            await ExportAdditionalData(_dto.Associations.ModuleContentIds, MixDatabaseParentType.Module);
        }
        private async Task ExportModuleSimpleDatas()
        {
            var data = await _context.MixModuleData.Where(
                    m => _dto.Associations.ModuleIds.Any(p => p == m.ParentId)).ToListAsync();
            _siteData.ModuleDatas.AddRange(data);
        }

        private async Task ExportModulePosts()
        {
            _siteData.ModulePosts = await _context.MixModulePostAssociation.Where(
                    m => _dto.Associations.ModuleContentIds.Any(p => p == m.LeftId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected modules.
            var postContentIds = _siteData.ModulePosts
                .Where(m => !_dto.Associations.PostContentIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();
            var postContents = _context.MixPostContent.Where(m => postContentIds.Contains(m.Id));
            var posts = _context.MixPost.Where(m => postContents.Any(p => p.ParentId == m.Id));
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
            var datas = await _context.MixData
                .Where(m => _dto.Associations.MixDatabaseIds.Any(p => p == m.MixDatabaseId))
                .AsNoTracking()
                .ToListAsync();
            _siteData.Datas = _siteData.Datas.Union(datas).ToList();
        }

        private async Task ExportDataContentsAsync()
        {
            _siteData.DataContents = await _context.MixDataContent
                .Where(m => _dto.Associations.MixDatabaseIds.Any(p => p == m.MixDatabaseId))
                .AsNoTracking()
                .ToListAsync();
        }
        private async Task ExportValuesAsync()
        {
            _siteData.DataContentValues = await _context.MixDataContentValue
                .Where(m => _dto.Associations.MixDatabaseIds.Any(n => n == m.MixDatabaseId))
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task ExportDataAssociationsAsync()
        {
            Expression<Func<MixDataContentAssociation, bool>> predicate = m => _dto.Associations.MixDatabaseIds.Any(p => p == m.MixDatabaseId);

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
                    && _dto.Associations.PageContentIds.Contains(m.SourceContentId.Value);
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Post
                    && m.SourceContentId.HasValue
                    && _dto.Associations.PostContentIds.Contains(m.SourceContentId.Value));
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Module
                    && m.SourceContentId.HasValue
                    && _dto.Associations.ModuleContentIds.Contains(m.SourceContentId.Value));

            _siteData.MixUrlAliases = await _context.MixUrlAlias.Where(predicate).ToListAsync();
        }
        #endregion
        #endregion

        #region Export Contents

        private async Task ExportContents()
        {
            await ExportTemplates();
            await ExportPages();
            await ExportPosts();
            await ExportModules();
            await ExportDatabases();
            if (_dto.IsIncludeConfigurations)
            {
                await ExportConfigurationDataAsync();
                await ExportLanguageDataAsync();
            }

        }

        private async Task ExportTemplates()
        {
            _siteData.Templates = await _context.MixViewTemplate
                .Where(m => m.MixThemeId == _dto.ThemeId)
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task ExportPages()
        {
            _siteData.Pages = await _context.MixPage
                .Where(m => _dto.Content.PageIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
            _siteData.PageContents = await _context.MixPageContent
                .Where(m => _dto.Content.PageContentIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
        }
        private async Task ExportModules()
        {
            _siteData.Modules = await _context.MixModule
                .Where(m => _dto.Content.ModuleIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
            _siteData.ModuleContents = await _context.MixModuleContent
                .Where(m => _dto.Content.ModuleContentIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
        }
        private async Task ExportPosts()
        {
            _siteData.Posts = await _context.MixPost
                .Where(m => _dto.Content.PostIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
            _siteData.PostContents = await _context.MixPostContent
                .Where(m => _dto.Content.PostContentIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task ExportDatabases()
        {
            _siteData.MixDatabaseColumns = await _context.MixDatabaseColumn
                .Where(m => _dto.Content.MixDatabaseIds.Any(p => p == m.MixDatabaseId))
                .AsNoTracking()
                .ToListAsync();
            _siteData.MixDatabases = await _context.MixDatabase
                .Where(m => _dto.Content.MixDatabaseIds.Any(p => p == m.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        #endregion

        #region Generic

        private async Task ExportAdditionalData(List<int> parentIds, MixDatabaseParentType type)
        {
            var associations = await _context.MixDataContentAssociation
                .Where(m =>
                    m.IntParentId.HasValue
                    && m.ParentType == type
                    && parentIds.Any(p => p == m.IntParentId.Value))
                .AsNoTracking()
                .ToListAsync();
            var dataIds = associations.Select(x => x.ParentId).ToList();
            var contentIds = associations.Select(x => x.DataContentId).ToList();
            var datas = await _context.MixData
                .Where(m => dataIds.Contains(m.Id))
                .AsNoTracking()
                .ToListAsync();
            var dataContents = await _context.MixDataContent
                .Where(m => contentIds.Contains(m.Id))
                .AsNoTracking()
                .ToListAsync();

            _siteData.Datas = _siteData.Datas.Union(datas).ToList();
            _siteData.DataContents = _siteData.DataContents.Union(dataContents).ToList();
            _siteData.DataContentAssociations = _siteData.DataContentAssociations.Union(associations).ToList();
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

        #endregion Export
    }
}
