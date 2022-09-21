using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Lib.Extensions;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;

namespace Mix.Lib.Services
{
    public class MixThemeImportService
    {
        private UnitOfWorkInfo _uow;
        private CancellationTokenSource _cts;
        private readonly RuntimeDbContextService _runtimeDbContextService;
        private DatabaseService _databaseService;
        private MixDbService _mixDbService;
        private readonly MixCmsContext _context;
        private readonly DbContext _runtimeDbContext;
        private SiteDataViewModel _siteData;
        private ISession _session;
        public MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        private MixTenantSystemViewModel _currentTenant;
        private MixCulture _currentCulture;
        #region Dictionaries

        private Dictionary<int, int> dicAliasIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicConfigurationContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleDataIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicTemplateIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicMixDatabaseIds = new Dictionary<int, int>();
        private Dictionary<string, string> dicMixDatabaseNames = new Dictionary<string, string>();
        private Dictionary<int, int> dicMixDatabaseContextIds = new Dictionary<int, int>();

        public MixThemeImportService(UnitOfWorkInfo<MixCmsContext> uow, IHttpContextAccessor httpContext, DatabaseService databaseService, MixDbService mixDbService, RuntimeDbContextService runtimeDbContextService)
        {
            _uow = uow;
            _context = uow.DbContext;
            _cts = new CancellationTokenSource();
            _session = httpContext.HttpContext.Session;
            _databaseService = databaseService;
            _mixDbService = mixDbService;
            _runtimeDbContextService = runtimeDbContextService;
            _runtimeDbContext = _runtimeDbContextService.GetMixDatabaseDbContext();
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
                MixFileHelper.SaveFile(themeFile, MixFolders.ThemePackage);
                MixFileHelper.UnZipFile(templateAsset.FullPath, MixFolders.ThemePackage);
            }
            else
            {
                MixFileHelper.UnZipFile(MixThemePackageConstants.DefaultThemeFilePath, MixFolders.ThemePackage);
            }
        }

        private Dictionary<int, int> dicColumnIds = new Dictionary<int, int>();

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
                _runtimeDbContextService.Reload();
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
            string destAssets = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{_siteData.ThemeSystemName}/assets";
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
            await ImportEntitiesAsync(_siteData.Languages, dicLanguageIds);
            await ImportContentDataAsync(_siteData.LanguageContents, dicLanguageContentIds, dicLanguageIds);
        }

        private async Task ImportConfigurations()
        {
            await ImportEntitiesAsync(_siteData.Configurations, dicConfigurationIds);
            await ImportContentDataAsync(_siteData.ConfigurationContents, dicConfigurationContentIds, dicConfigurationIds);
        }

        private async Task ImportMixDatabases()
        {
            await ImportDatabaseContextsAsync();
            await ImportDatabasesAsync();
            await ImportDatabaseRelationshipsAsync();
            await MigrateMixDatabaseAsync();
            await ImportAssociationDataAsync(_siteData.DatabaseContextDatabaseAssociations, dicMixDatabaseContextIds, dicMixDatabaseIds);
        }

        private async Task MigrateMixDatabaseAsync()
        {
            foreach (var item in _siteData.MixDatabases)
            {
                if (dicMixDatabaseNames.ContainsKey(item.SystemName))
                {
                    await _mixDbService.MigrateDatabase(dicMixDatabaseNames[item.SystemName]);
                }
            }
            _runtimeDbContextService.Reload();
        }

        private async Task ImportPosts()
        {
            await ImportEntitiesAsync(_siteData.Posts, dicPostIds);
            await ImportContentDataAsync(_siteData.PostContents, dicPostContentIds, dicPostIds);
        }

        private async Task ImportModules()
        {
            await ImportEntitiesAsync(_siteData.Modules, dicModuleIds);
            await ImportModuleContentsAsync();
            await ImportContentDataAsync(_siteData.ModuleDatas, dicModuleDataIds, dicModuleIds);
        }

        private async Task ImportPages()
        {
            await ImportEntitiesAsync(_siteData.Pages, dicPageIds);
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
                        FileFolder = String.Format(@"{0}/{1}/{2}/{3}", MixFolders.TemplatesFolder, CurrentTenant.SystemName, x.MixThemeName, x.FolderType), //x.FileFolder,
                        Filename = x.FileName
                    }); ;
                });
                await ImportEntitiesAsync(_siteData.Templates, dicTemplateIds);
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
            await ImportAssociationDataAsync(_siteData.PageModules, dicPageIds, dicModuleIds);
            await ImportAssociationDataAsync(_siteData.PagePosts, dicPageIds, dicPostIds);
            await ImportAssociationDataAsync(_siteData.ModulePosts, dicModuleIds, dicPostIds);

            //await ImportDatabaseDataAsync();

            await ImportEntitiesAsync(_siteData.MixUrlAliases, dicAliasIds);
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
                item.Specificulture = _currentCulture.Specificulture;
                item.MixTenantId = CurrentTenant.Id;
                item.ParentId = dicPageIds[item.ParentId];
                item.Specificulture = _siteData.Specificulture;
                _context.Entry(item).State = EntityState.Added;
                await _context.SaveChangesAsync(_cts.Token);
                dicPageContentIds.Add(oldId, item.Id);
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
                    item.ParentId = dicModuleIds[item.ParentId];
                    item.Specificulture = _siteData.Specificulture;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dicModuleContentIds.Add(oldId, item.Id);
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
                dicMixDatabaseContextIds.Add(oldId, item.Id);
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
                    dicMixDatabaseIds.Add(oldId, item.Id);
                    dicMixDatabaseNames.Add(oldName, item.SystemName);
                    await ImportDatabaseColumnsAsync(item, _siteData.MixDatabaseColumns.Where(m => m.MixDatabaseId == oldId));
                }
                else
                {
                    dicMixDatabaseIds.Add(oldId, currentDb.Id);
                    dicMixDatabaseNames.Add(oldName, currentDb.SystemName);
                }
            }
        }

        private async Task ImportDatabaseColumnsAsync(MixDatabase database, IEnumerable<MixDatabaseColumn> cols)
        {
            foreach (var item in cols)
            {
                var table = _context.MixDatabaseColumn.AsNoTracking();
                if (table.Any(m => m.MixDatabaseId == dicMixDatabaseIds[item.MixDatabaseId] && m.SystemName == item.SystemName))
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
                dicColumnIds.Add(oldId, item.Id);
            }
        }

        private async Task ImportDatabaseRelationshipsAsync()
        {
            foreach (var item in _siteData.MixDatabaseRelationships)
            {
                if (dicMixDatabaseIds.ContainsKey(item.ParentId) && dicMixDatabaseIds.ContainsKey(item.ChildId))
                {
                    item.Id = 0;
                    item.ParentId = dicMixDatabaseIds[item.ParentId];
                    item.ChildId = dicMixDatabaseIds[item.ChildId];
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
                        await _runtimeDbContext.Database.ExecuteSqlRawAsync(sql);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private string GetInsertQuery(MixDbModel database)
        {
            List<string> columns = new List<string> { "id", "createdDateTime", "tenantId" };
            columns.AddRange(_siteData.MixDatabaseColumns.Where(c => c.MixDatabaseName == database.DatabaseName).Select(c => c.SystemName).ToList());
            List<string> sqls = new();
            foreach (JObject item in database.Data)
            {
                List<string> values = new();
                item["tenantId"] = CurrentTenant.Id;
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
