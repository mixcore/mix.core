﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Lib.Extensions;
using Mix.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;

namespace Mix.Lib.Services
{
    public class MixThemeImportService
    {
        private readonly MixRepoDbRepository _repository;
        private readonly UnitOfWorkInfo _uow;
        private readonly CancellationTokenSource _cts;
        private readonly DatabaseService _databaseService;
        private readonly MixDbService _mixDbService;
        private readonly MixCmsContext _context;
        private SiteDataViewModel _siteData;
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
        private MixCulture _currentCulture;
        #region Dictionaries

        private readonly Dictionary<int, int> _dicAliasIds = new();
        private readonly Dictionary<int, int> _dicConfigurationIds = new();
        private readonly Dictionary<int, int> _dicConfigurationContentIds = new();
        private readonly Dictionary<int, int> _dicLanguageIds = new();
        private readonly Dictionary<int, int> _dicLanguageContentIds = new();
        private readonly Dictionary<int, int> _dicModuleIds = new();
        private readonly Dictionary<int, int> _dicModuleContentIds = new();
        private readonly Dictionary<int, int> _dicModuleDataIds = new();
        private readonly Dictionary<int, int> _dicPostIds = new();
        private readonly Dictionary<int, int> _dicPostContentIds = new();
        private readonly Dictionary<int, int> _dicPageIds = new();
        private readonly Dictionary<int, int> _dicTemplateIds = new();
        private readonly Dictionary<int, int> _dicPageContentIds = new();
        private readonly Dictionary<int, int> _dicMixDatabaseIds = new();
        private readonly Dictionary<string, string> _dicMixDatabaseNames = new();
        private readonly Dictionary<int, int> _dicMixDatabaseContextIds = new();
        private readonly Dictionary<int, int> _dicColumnIds = new();

        public MixThemeImportService(UnitOfWorkInfo<MixCmsContext> uow, IHttpContextAccessor httpContext, DatabaseService databaseService,
            MixDbService mixDbService, MixRepoDbRepository repository)
        {
            _uow = uow;
            _context = uow.DbContext;
            _cts = new CancellationTokenSource();
            _session = httpContext.HttpContext?.Session;
            _databaseService = databaseService;
            _mixDbService = mixDbService;
            _repository = repository;
        }

        #region Import

        public async Task DownloadThemeAsync(
            JObject theme, IProgress<int> progress, HttpService httpService, string folder = MixFolders.ThemePackage)
        {
            string name = theme.Value<string>("title");
            MixFileHelper.EmptyFolder(MixFolders.ThemePackage);
            var cancellationToken = new CancellationToken();
            string filePath = $"{MixFolders.ThemePackage}/{name}{MixFileExtensions.Zip}";
            await httpService.DownloadAsync(
                theme.Value<string>("source"),
                MixFolders.ThemePackage,
                name, MixFileExtensions.Zip,
                progress, cancellationToken);
            MixFileHelper.UnZipFile(filePath, folder);
        }


        public SiteDataViewModel LoadSchema()
        {
            var strSchema = MixFileHelper.GetFile(MixThemePackageConstants.SchemaFilename, MixFileExtensions.Json, $"{MixFolders.ThemePackage}/{MixThemePackageConstants.SchemaFolder}");
            var siteStructures = JObject.Parse(strSchema.Content).ToObject<SiteDataViewModel>();
            return siteStructures;
        }

        public void ExtractTheme(IFormFile themeFile)
        {
            MixFileHelper.EmptyFolder(MixFolders.ThemePackage);
            if (themeFile != null)
            {
                var templateAsset = MixHelper.GetFileModel(themeFile, MixFolders.ThemePackage);
                var saveResult = MixFileHelper.SaveFile(themeFile, MixFolders.ThemePackage);
                templateAsset.Filename = saveResult.Filename;
                MixFileHelper.UnZipFile(templateAsset.FullPath, MixFolders.ThemePackage);
            }
            else
            {
                MixFileHelper.UnZipFile(MixThemePackageConstants.DefaultThemeFilePath, MixFolders.ThemePackage);
            }
        }

        #endregion

        public async Task<SiteDataViewModel> ImportSelectedItemsAsync(SiteDataViewModel siteData)
        {
            try
            {
                _uow.Begin();
                _siteData = ParseSiteData(siteData);

                if (_siteData.ThemeId == 0)
                {
                    _siteData.ThemeId = await CreateTheme();
                }
                ImportAssets();
                await ImportContent();
                await ImportData();

                await _uow.CompleteAsync();
                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private void ImportAssets()
        {
            // Copy theme's assets
            string srcAssets = $"{MixFolders.ThemePackage}/{MixThemePackageConstants.AssetFolder}";
            //string destAssets = $"{MixFolders.WebRootPath}/{MixFolders.SiteContentAssetsFolder}/{_siteData.ThemeSystemName}";
            string destAssets = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{_siteData.ThemeSystemName}";
            MixFileHelper.CopyFolder(srcAssets, destAssets);

            // Copy theme's upload files
            string srcUpload = $"{MixFolders.ThemePackage}/{MixThemePackageConstants.UploadFolder}";
            //string destUpload = $"{MixFolders.WebRootPath}/{MixFolders.UploadsFolder}";
            string destUpload = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/uploads";
            MixFileHelper.CopyFolder(srcUpload, destUpload);
        }

        private async Task<int> CreateTheme()
        {
            var table = _context.MixModuleContent.AsNoTracking();
            _siteData.ThemeId = table.Any() ? table.Max(m => m.Id) + 1 : 1;
            var theme = new MixTheme()
            {
                MixTenantId = CurrentTenant.Id,
                DisplayName = _siteData.ThemeName,
                SystemName = _siteData.ThemeSystemName,
                AssetFolder = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{_siteData.ThemeSystemName}",
                TemplateFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{_siteData.ThemeSystemName}",
                CreatedBy = _siteData.CreatedBy,
                CreatedDateTime = DateTime.UtcNow,
                Status = MixContentStatus.Published
            };
            _context.MixTheme.Add(theme);
            await _context.SaveChangesAsync(_cts.Token);
            return theme.Id;
        }
        #region Import Contents


        private async Task ImportContent()
        {
            await ImportTemplates();
            await ImportModules();
            await ImportPages();
            await ImportConfigurations();
            await ImportLanguages();
            await ImportPosts();
            await ImportMixDatabases();
            await ImportMixDbDataAsync();
        }

        private async Task ImportLanguages()
        {
            await ImportEntitiesAsync(_siteData.Languages, _dicLanguageIds);
            await ImportContentDataAsync(_siteData.LanguageContents, _dicLanguageContentIds, _dicLanguageIds);
        }

        private async Task ImportConfigurations()
        {
            await ImportEntitiesAsync(_siteData.Configurations, _dicConfigurationIds);
            await ImportContentDataAsync(_siteData.ConfigurationContents, _dicConfigurationContentIds, _dicConfigurationIds);
        }

        private async Task ImportMixDatabases()
        {
            await ImportDatabaseContextsAsync();
            await ImportDatabasesAsync();
            await ImportDatabaseRelationshipsAsync();
            await MigrateMixDatabaseAsync();
            await ImportAssociationDataAsync(_siteData.DatabaseContextDatabaseAssociations, _dicMixDatabaseContextIds, _dicMixDatabaseIds);
        }

        private async Task MigrateMixDatabaseAsync()
        {
            foreach (var item in _siteData.MixDatabases)
            {
                if (_dicMixDatabaseNames.ContainsKey(item.SystemName))
                {
                    await _mixDbService.MigrateDatabase(_dicMixDatabaseNames[item.SystemName]);
                }
            }
        }

        private async Task ImportPosts()
        {
            await ImportEntitiesAsync(_siteData.Posts, _dicPostIds);
            await ImportSEOContentDataAsync(_siteData.PostContents, _dicPostContentIds, _dicPostIds);
        }

        private async Task ImportModules()
        {
            await ImportEntitiesAsync(_siteData.Modules, _dicModuleIds);
            await ImportModuleContentsAsync();
            await ImportContentDataAsync(_siteData.ModuleDatas, _dicModuleDataIds, _dicModuleIds);
        }

        private async Task ImportPages()
        {
            await ImportEntitiesAsync(_siteData.Pages, _dicPageIds);
            await ImportPageContentsAsync();

        }
        private async Task ImportTemplates()
        {
            if (_siteData.Templates.Any())
            {
                _siteData.Templates.ForEach(x =>
                {
                    x.MixTenantId = CurrentTenant.Id;
                    x.CreatedBy = _siteData.CreatedBy;
                    x.CreatedDateTime = DateTime.UtcNow;
                    x.MixThemeId = _siteData.ThemeId;
                    x.MixThemeName = _siteData.ThemeSystemName;
                    x.Content = ReplaceContent(x.Content, _siteData.ThemeSystemName);
                    x.FileFolder = ReplaceContent(x.FileFolder, _siteData.ThemeSystemName);
                    MixFileHelper.SaveFile(new FileModel()
                    {
                        Content = x.Content,
                        Extension = x.Extension,
                        FileFolder = $@"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{x.MixThemeName}/{x.FolderType}", //x.FileFolder,
                        Filename = x.FileName
                    }); ;
                });
                await ImportEntitiesAsync(_siteData.Templates, _dicTemplateIds);
            }
        }

        private SiteDataViewModel ParseSiteData(SiteDataViewModel siteData)
        {
            string strContent = JObject.FromObject(siteData).ToString();
            var obj = JObject.Parse(ReplaceContent(strContent, siteData.ThemeSystemName));
            _currentCulture = _context.MixCulture.First(m =>
                m.MixTenantId == CurrentTenant.Id
                && (!string.IsNullOrEmpty(siteData.Specificulture) || m.Specificulture == siteData.Specificulture));
            return obj.ToObject<SiteDataViewModel>();
        }

        private string ReplaceContent(string content, string themeName)
        {
            var tmp = content
                .Replace("[THEME_NAME]", themeName)
                .Replace("[TENANT_NAME]", CurrentTenant.SystemName);
            return tmp;
        }

        #endregion

        #region Import Datas

        private async Task ImportData()
        {
            await ImportAssociationDataAsync(_siteData.PageModules, _dicPageIds, _dicModuleIds);
            await ImportAssociationDataAsync(_siteData.PagePosts, _dicPageIds, _dicPostIds);
            await ImportAssociationDataAsync(_siteData.ModulePosts, _dicModuleIds, _dicPostIds);
            await ImportEntitiesAsync(_siteData.MixUrlAliases, _dicAliasIds);
        }

        #region Import Page Data

        private async Task ImportPageContentsAsync()
        {
            foreach (var item in _siteData.PageContents)
            {
                var oldId = item.Id;
                while (_context.MixPageContent.Any(m => m.SeoName == item.SeoName))
                {
                    item.SeoName = $"{item.SeoName}-1";
                }
                item.Id = 0;
                item.CreatedBy = _siteData.CreatedBy;
                item.CreatedDateTime = DateTime.UtcNow;
                item.MixCultureId = _currentCulture.Id;
                if (item.TemplateId.HasValue)
                {
                    item.TemplateId = _dicTemplateIds[item.TemplateId.Value];
                }
                if (item.LayoutId.HasValue)
                {
                    item.LayoutId = _dicTemplateIds[item.LayoutId.Value];
                }
                item.Specificulture = _currentCulture.Specificulture;
                item.MixTenantId = CurrentTenant.Id;
                item.ParentId = _dicPageIds[item.ParentId];
                item.Specificulture = _siteData.Specificulture;
                _context.Entry(item).State = EntityState.Added;
                await _context.SaveChangesAsync(_cts.Token);
                _dicPageContentIds.Add(oldId, item.Id);
            }

        }
        #endregion Import Page

        #region Import Module Data

        private async Task ImportModuleContentsAsync()
        {
            foreach (var item in _siteData.ModuleContents)
            {
                if (!_context.MixModuleContent.Any(m => m.SystemName == item.SystemName))
                {
                    var oldId = item.Id;
                    item.Id = 0;
                    item.CreatedBy = _siteData.CreatedBy;
                    item.MixTenantId = CurrentTenant.Id;
                    if (item.TemplateId.HasValue)
                    {
                        item.TemplateId = _dicTemplateIds[item.TemplateId.Value];
                    }
                    item.ParentId = _dicModuleIds[item.ParentId];
                    item.Specificulture = _siteData.Specificulture;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    _dicModuleContentIds.Add(oldId, item.Id);
                }
            }
        }

        #endregion Import Module

        #region Import Database Data
        private async Task ImportDatabaseContextsAsync()
        {
            foreach (var item in _siteData.MixDatabaseContexts)
            {
                var oldId = item.Id;

                while (_context.MixDatabaseContext.Any(m => m.SystemName == item.SystemName))
                {
                    item.SystemName = $"{item.SystemName}_1";
                }
                item.MixTenantId = CurrentTenant.Id;
                item.Id = 0;
                item.CreatedBy = _siteData.CreatedBy;
                item.CreatedDateTime = DateTime.UtcNow;
                item.DatabaseProvider = _databaseService.DatabaseProvider;
                item.ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                _context.Entry(item).State = EntityState.Added;
                await _context.SaveChangesAsync(_cts.Token);
                _dicMixDatabaseContextIds.Add(oldId, item.Id);
            }
        }

        private async Task ImportDatabasesAsync()
        {
            foreach (var item in _siteData.MixDatabases)
            {
                var oldId = item.Id;
                var oldName = item.SystemName;
                var currentDb = _context.MixDatabase.SingleOrDefault(m => m.SystemName == item.SystemName);
                if (currentDb == null)
                {
                    item.Id = 0;
                    item.CreatedBy = _siteData.CreatedBy;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    _dicMixDatabaseIds.Add(oldId, item.Id);
                    _dicMixDatabaseNames.Add(oldName, item.SystemName);
                    await ImportDatabaseColumnsAsync(item, _siteData.MixDatabaseColumns.Where(m => m.MixDatabaseId == oldId));
                }
                else
                {
                    _dicMixDatabaseIds.Add(oldId, currentDb.Id);
                    _dicMixDatabaseNames.Add(oldName, currentDb.SystemName);
                }
            }
        }

        private async Task ImportDatabaseColumnsAsync(MixDatabase database, IEnumerable<MixDatabaseColumn> cols)
        {
            foreach (var item in cols)
            {
                var table = _context.MixDatabaseColumn.AsNoTracking();
                if (table.Any(m => m.MixDatabaseId == _dicMixDatabaseIds[item.MixDatabaseId] && m.SystemName == item.SystemName))
                {
                    continue;
                }
                var oldId = item.Id;
                item.Id = 0;
                item.CreatedBy = _siteData.CreatedBy;
                item.CreatedDateTime = DateTime.UtcNow;
                item.MixDatabaseId = database.Id;
                item.MixDatabaseName = database.SystemName;
                _context.MixDatabaseColumn.Add(item);
                await _context.SaveChangesAsync(_cts.Token);
                _dicColumnIds.Add(oldId, item.Id);
            }
        }

        private async Task ImportDatabaseRelationshipsAsync()
        {
            foreach (var item in _siteData.MixDatabaseRelationships)
            {
                if (_dicMixDatabaseIds.ContainsKey(item.ParentId) && _dicMixDatabaseIds.ContainsKey(item.ChildId))
                {
                    item.Id = 0;
                    item.ParentId = _dicMixDatabaseIds[item.ParentId];
                    item.ChildId = _dicMixDatabaseIds[item.ChildId];
                    item.CreatedBy = _siteData.CreatedBy;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                }
            }
        }

        public async Task ImportMixDbDataAsync()
        {
            foreach (var database in _siteData.MixDbModels)
            {
                try
                {
                    if (database.Data != null && database.Data.Count > 0)
                    {
                        var sql = GetInsertQuery(database);
                        await _repository.ExecuteCommand(sql);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private string GetInsertQuery(MixDbModel database)
        {
            List<string> columns = new List<string> { "Id", "CreatedDateTime", "LastModified", "MixTenantId", "CreatedBy", "ModifiedBy", "Priority", "Status", "IsDeleted" };
            columns.AddRange(_siteData.MixDatabaseColumns.Where(c => c.MixDatabaseName == database.DatabaseName).Select(c => c.SystemName.ToTitleCase()).ToList());
            List<string> sqls = new();
            foreach (JObject item in database.Data)
            {
                List<string> values = new();
                item["MixTenantId"] = CurrentTenant.Id;
                foreach (var col in columns)
                {
                    values.Add($"'{item.Value<string>(col)}'");
                }
                sqls.Add($"Insert into {database.DatabaseName} ({string.Join(',', columns)}) Values ({string.Join(',', values)})");
            }
            return string.Join(';', sqls);
        }

        #endregion Import Module

        #endregion

        #region Generic

        private async Task ImportGuidDatasAsync<T>(List<T> data)
            where T : EntityBase<Guid>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    ReflectionHelper.SetPropertyValue(item, new EntityPropertyModel()
                    {
                        PropertyName = MixRequestQueryKeywords.TenantId,
                        PropertyValue = CurrentTenant.Id
                    });
                    ReflectionHelper.SetPropertyValue(item, new EntityPropertyModel()
                    {
                        PropertyName = MixRequestQueryKeywords.Specificulture,
                        PropertyValue = _siteData.Specificulture
                    });
                    item.CreatedBy = _siteData.CreatedBy;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync(_cts.Token);
            }
        }

        private async Task ImportEntitiesAsync<T>(List<T> data, Dictionary<int, int> dic)
            where T : EntityBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var oldId = item.Id;

                    if (ReflectionHelper.HasProperty(typeof(T), MixRequestQueryKeywords.TenantId))
                    {
                        ReflectionHelper.SetPropertyValue(item, new EntityPropertyModel()
                        {
                            PropertyName = MixRequestQueryKeywords.TenantId,
                            PropertyValue = CurrentTenant.Id
                        });
                    }

                    item.Id = 0;
                    item.CreatedBy = _siteData.CreatedBy;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dic.Add(oldId, item.Id);
                }
            }
        }

        private async Task ImportContentDataAsync<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
            where T : MultilingualContentBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var oldId = item.Id;
                    item.Id = 0;
                    item.CreatedBy = _siteData.CreatedBy;
                    item.MixTenantId = CurrentTenant.Id;
                    item.ParentId = parentDic[item.ParentId];
                    item.MixCultureId = _currentCulture.Id;
                    item.Specificulture = _currentCulture.Specificulture;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dic.Add(oldId, item.Id);
                }
            }
        }

        private async Task ImportSEOContentDataAsync<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
           where T : MultilingualSEOContentBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var oldId = item.Id;
                    item.Id = 0;
                    if (item.TemplateId.HasValue)
                    {
                        item.TemplateId = _dicTemplateIds[item.TemplateId.Value];
                    }
                    if (item.LayoutId.HasValue)
                    {
                        item.LayoutId = _dicTemplateIds[item.LayoutId.Value];
                    }
                    item.CreatedBy = _siteData.CreatedBy;
                    item.MixTenantId = CurrentTenant.Id;
                    item.ParentId = parentDic[item.ParentId];
                    item.MixCultureId = _currentCulture.Id;
                    item.Specificulture = _currentCulture.Specificulture;
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dic.Add(oldId, item.Id);
                }
            }
        }

        private async Task ImportAssociationDataAsync<T>(
            List<T> data,
            Dictionary<int, int> leftDic,
            Dictionary<int, int> rightDic)
            where T : AssociationBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = 0;
                    item.CreatedBy = _siteData.CreatedBy;
                    item.MixTenantId = CurrentTenant.Id;
                    item.ParentId = leftDic[item.ParentId];
                    item.ChildId = rightDic[item.ChildId];
                    item.CreatedDateTime = DateTime.UtcNow;
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync(_cts.Token);
            }
        }
        #endregion

        #endregion Import
    }
}
