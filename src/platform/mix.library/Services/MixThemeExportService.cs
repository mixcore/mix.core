using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.Dtos;
using Mix.Lib.Models;
using Mix.RepoDb.Repositories;
using System.Linq.Expressions;

namespace Mix.Lib.Services
{
    public class MixThemeExportService
    {
        private readonly Repository<MixCmsContext, MixTheme, int, MixThemeViewModel> _themeRepository;
        private readonly MixCmsContext _context;
        private readonly MixRepoDbRepository _repository;
        private SiteDataViewModel _siteData;
        private ExportThemeDto _dto;
        private MixThemeViewModel _exportTheme;
        private string _tempPath;
        private string _outputPath;
        private string _webPath;
        private string _fileName;
        private readonly ISession _session;
        public MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        private MixTenantSystemModel _currentTenant;
        public MixThemeExportService(IHttpContextAccessor httpContext, MixCmsContext context, MixRepoDbRepository repository)
        {
            _session = httpContext.HttpContext?.Session;
            _context = context;
            _themeRepository = MixThemeViewModel.GetRepository(new UnitOfWorkInfo(_context));
            _repository = repository;
        }

        #region Export

        public async Task<string> ExportTheme(ExportThemeDto request)
        {
            _dto = request;
            _siteData = new();
            _exportTheme = await _themeRepository.GetSingleAsync(
                m => m.Id == request.ThemeId);

            //path to temporary folder
            _siteData.ThemeName = _exportTheme.DisplayName;
            _siteData.ThemeSystemName = _exportTheme.SystemName;
            _fileName = $"{_exportTheme.SystemName}-{Guid.NewGuid()}";
            _webPath = $"{MixFolders.StaticFiles}/Themes/{_exportTheme.SystemName}";
            _tempPath = $"{_webPath}/temp";
            _outputPath = _webPath;

            await ExportSelectedItemsAsync();

            ExportSchema(_siteData);

            ExportAssets();

            // Zip to [theme_name].zip ( wwwroot for web path)
            return ZipTheme();
        }

        private string ZipTheme()
        {
            // Zip to [theme_name].zip ( wwwroot for web path)
            string filePath = MixFileHelper.ZipFolder(_tempPath, _outputPath, _fileName);

            // Delete temp folder
            MixFileHelper.DeleteFolder($"{_outputPath}/{MixThemePackageConstants.AssetFolder}");
            MixFileHelper.DeleteFolder($"{_outputPath}/{MixThemePackageConstants.UploadFolder}");
            MixFileHelper.DeleteFolder($"{_outputPath}/{MixThemePackageConstants.SchemaFolder}");
            return $"{_webPath}/{_fileName}.zip";
        }

        private void ExportAssets()
        {
            if (_dto.IsIncludeAssets)
            {
                // Copy current assets files
                MixFileHelper.CopyFolder(
                    _exportTheme.AssetFolder,
                    $"{_tempPath}/{MixThemePackageConstants.AssetFolder}");
                // Copy current uploads files
                MixFileHelper.CopyFolder(
                    $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{MixThemePackageConstants.UploadFolder}",
                    $"{_tempPath}/{MixThemePackageConstants.UploadFolder}");
            }
        }

        private void ExportSchema(SiteDataViewModel siteData)
        {
            string filename = MixThemePackageConstants.SchemaFilename;
            string content = MixHelper.SerializeObject(siteData);
            content = content
                .Replace($"/{siteData.ThemeName}", "/[THEME_NAME]")
                .Replace($"/{CurrentTenant.SystemName}", "/[TENANT_NAME]");
            if (!string.IsNullOrEmpty(CurrentTenant.Configurations.Domain))
            {
                content = content.Replace(CurrentTenant.Configurations.Domain, string.Empty);
            }
            FileModel schema = new()
            {
                Filename = filename,
                Extension = MixFileExtensions.Json,
                FileFolder = $"{_tempPath}/{MixThemePackageConstants.SchemaFolder}",
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
            //await ExportDatabaseDatas();


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
            _siteData.PageModules = await _context.MixPageModuleAssociation.Where(m => _dto.Content.PageIds.Any(p => p == m.ParentId)).ToListAsync();
            var unloadModuleIds = _siteData.PageModules.Where(m => !_dto.Content.ModuleIds.Any(p => m.ChildId == p)).Select(m => m.ChildId);

            var modules = await _context.MixModule.Where(m => unloadModuleIds.Any(p => p == m.Id)).ToListAsync();
            _siteData.Modules.AddRange(modules);
        }
        private async Task ExportPagePosts()
        {
            _siteData.PagePosts = await _context.MixPagePostAssociation.Where(
                    m => _dto.Associations.PageContentIds.Any(p => p == m.ParentId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected Pages.
            var postContentIds = _siteData.PagePosts
                .Where(m => !_dto.Associations.PostContentIds.Any(n => m.ChildId == n))
                .Select(m => m.ChildId).ToList();
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
                    m => _dto.Associations.ModuleContentIds.Any(p => p == m.ParentId)).ToListAsync();
            _siteData.ModuleDatas.AddRange(data);
        }

        private async Task ExportModulePosts()
        {
            _siteData.ModulePosts = await _context.MixModulePostAssociation.Where(
                    m => _dto.Associations.ModuleContentIds.Any(p => p == m.ParentId)).ToListAsync();
            // Get Posts unchecked when export but needed in selected modules.
            var postContentIds = _siteData.ModulePosts
                .Where(m => !_dto.Associations.PostContentIds.Any(n => m.ChildId == n))
                .Select(m => m.ChildId).ToList();
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

        private async Task ExportMixDbAsync()
        {
            foreach (var database in _siteData.MixDatabases)
            {
                _repository.InitTableName(database.SystemName);
                var data = await _repository.GetAllAsync();
                if (data != null)
                {
                    _siteData.MixDbModels.Add(new()
                    {
                        DatabaseName = database.SystemName,
                        Data = JArray.FromObject(data)
                    });
                }
            }
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
            await ExportMixDbAsync();
            if (_dto.IsIncludeConfigurations)
            {
                await ExportConfigurationDataAsync();
                await ExportLanguageDataAsync();
            }
        }

        private async Task ExportTemplates()
        {
            if (_dto.IsIncludeTemplates)
            {
                _siteData.Templates = await _context.MixViewTemplate
                    .Where(m => m.MixThemeId == _dto.ThemeId)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var item in _siteData.Templates)
                {
                    item.Content = item.Content.Replace($"/{_siteData.ThemeSystemName}", "/[THEME_NAME]");
                    item.FileFolder = item.FileFolder.Replace($"/{_siteData.ThemeSystemName}", "/[THEME_NAME]");
                }
            }
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
            _siteData.MixDatabaseRelationships = await _context.MixDatabaseRelationship
                .Where(m => _dto.Content.MixDatabaseIds.Any(p => p == m.ParentId))
                .AsNoTracking()
                .ToListAsync();
        }

        #endregion

        #region Generic

        private Task ExportAdditionalData(List<int> parentIds, MixDatabaseParentType type)
        {
            return Task.CompletedTask;
            //throw new MixException(MixErrorStatus.ServerError, $"Unhandled: {GetType().FullName} line 395");
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
