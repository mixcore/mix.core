using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.WellKnownTypes;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mix.Auth.Constants;
using Mix.Database.Base;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.RepoDb.Helpers;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Signalr.Hub.Models;

namespace Mix.Lib.Services
{
    public class MixThemeImportService : IMixThemeImportService
    {
        private MixRepoDbRepository _repository { get; set; }
        private FieldNameService _fieldNameService { get; set; }
        private MixCacheService _cacheService { get; set; }
        private readonly CancellationTokenSource _cts;
        private readonly DatabaseService _databaseService;
        private readonly IDatabaseConstants _databaseConstant;
        private UnitOfWorkInfo<MixCmsContext> _uow { get; set; }
        private MixCmsContext _context { get; set; }
        private SiteDataViewModel _siteData;
        private readonly ISession _session;
        private readonly IServiceProvider _serviceProvider;
        public MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session?.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant) ?? new()
                    {
                        Id = 1
                    };
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
        private readonly Dictionary<int, int> _dicModuleDataIds = new();
        private readonly Dictionary<int, int> _dicPostIds = new();
        private readonly Dictionary<int, int> _dicPostContentIds = new();
        private readonly Dictionary<int, int> _dicPageIds = new();
        private readonly Dictionary<int, int> _dicTemplateIds = new();
        private readonly Dictionary<int, int> _dicMixDatabaseIds = new();
        private readonly Dictionary<string, string> _dicMixDatabaseNames = new();
        private readonly Dictionary<int, int> _dicMixDatabaseContextIds = new();

        public MixThemeImportService(
            IHttpContextAccessor httpContext,
            DatabaseService databaseService,
            IServiceProvider serviceProvider,
            MixCacheService cacheService)
        {
            _cts = new CancellationTokenSource();
            _session = httpContext.HttpContext?.Session;
            _cacheService = cacheService;
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
            _databaseConstant = _databaseService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
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


        public Task<SiteDataViewModel> LoadSchema()
        {
            return LoadSchema($"{MixFolders.ThemePackage}/{MixThemePackageConstants.SchemaFolder}");
        }


        public async Task<SiteDataViewModel> LoadSchema(string folder)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                _uow = serviceScope.ServiceProvider.GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
                _context = _uow.DbContext;
                var strSchema = MixFileHelper.GetFile(MixThemePackageConstants.SchemaFilename, MixFileExtensions.Json, folder);
                var siteStructures = JObject.Parse(strSchema.Content).ToObject<SiteDataViewModel>();
                await ValidateSiteData(siteStructures);
                serviceScope.Dispose();
                return siteStructures;
            }
        }

        public void ExtractTheme(IFormFile themeFile)
        {
            MixFileHelper.EmptyFolder(MixFolders.ThemePackage);
            if (themeFile != null)
            {
                using (var fileStream = themeFile.OpenReadStream())
                {
                    var formFile = new FileModel(themeFile.FileName, fileStream, MixFolders.ThemePackage);
                    var templateAsset = MixFileHelper.SaveFile(formFile);
                    MixFileHelper.UnZipFile(formFile.FullPath, MixFolders.ThemePackage);
                    fileStream.Dispose();
                }
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
                _siteData = ParseSiteData(siteData);

                await ImportMixDb();
                await ImportSiteData();

                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task<SiteDataViewModel> ImportSiteData()
        {
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    _uow = serviceScope.ServiceProvider.GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
                    var mixdbUow = serviceScope.ServiceProvider.GetRequiredService<UnitOfWorkInfo<MixDbDbContext>>();
                    _repository = serviceScope.ServiceProvider.GetRequiredService<MixRepoDbRepository>();
                    _repository.SetDbConnection(mixdbUow);
                    _context = _uow.DbContext;
                    _uow.Begin();

                    _currentCulture = _context.MixCulture.First(m =>
                m.MixTenantId == CurrentTenant.Id
                && (string.IsNullOrEmpty(_siteData.Specificulture) || m.Specificulture == _siteData.Specificulture));

                    if (_siteData.ThemeId == 0)
                    {
                        _siteData.ThemeId = await CreateTheme();
                    }
                    ImportAssets();
                    await ImportContent();
                    await ImportData();

                    await _uow.CompleteAsync();
                    await mixdbUow.CompleteAsync();
                    serviceScope.Dispose();
                    return _siteData;
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }
        private async Task ImportMixDb()
        {
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var uow = serviceScope.ServiceProvider.GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
                    uow.Begin();
                    var mixDbService = serviceScope.ServiceProvider.GetRequiredService<IMixDbService>();

                    await ImportMixDatabases(uow, _cacheService, mixDbService);
                    await uow.CompleteAsync();
                    serviceScope.Dispose();
                }
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
            await ImportMixDbDataAsync();
        }

        private async Task ImportLanguages()
        {
            await ImportEntitiesAsync(_context, _siteData.Languages, _dicLanguageIds);
            await ImportContentDataAsync(_siteData.LanguageContents, _dicLanguageContentIds, _dicLanguageIds);
        }

        private async Task ImportConfigurations()
        {
            await ImportEntitiesAsync(_context, _siteData.Configurations, _dicConfigurationIds);
            await ImportContentDataAsync(_siteData.ConfigurationContents, _dicConfigurationContentIds, _dicConfigurationIds);
        }

        private async Task ImportMixDatabases(UnitOfWorkInfo<MixCmsContext> uow, MixCacheService cacheService, IMixDbService mixDbService, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await ImportDatabaseContextsAsync(uow.DbContext);
            await ImportDatabasesAsync(uow.DbContext);
            await ImportDatabaseRelationshipsAsync(uow.DbContext);
            await MigrateMixDatabaseAsync(uow, cacheService, mixDbService);
            await MigrateSystemMixDatabaseAsync(mixDbService, cancellationToken);
        }

        private async Task MigrateMixDatabaseAsync(UnitOfWorkInfo<MixCmsContext> uow, MixCacheService cacheService, IMixDbService mixDbService)
        {
            foreach (var item in _siteData.MixDatabases)
            {
                if (_dicMixDatabaseNames.ContainsKey(item.SystemName))
                {

                    RepoDbMixDatabaseViewModel database = await RepoDbMixDatabaseViewModel
                        .GetRepository(uow, cacheService).GetSingleAsync(m => m.SystemName == _dicMixDatabaseNames[item.SystemName]);

                    await mixDbService.MigrateDatabase(database);
                }
            }
        }

        private async Task MigrateSystemMixDatabaseAsync(IMixDbService mixDbService, CancellationToken cancellationToken = default)
        {
            await mixDbService.MigrateSystemDatabases(cancellationToken);
        }

        private async Task ImportPosts()
        {
            await ImportEntitiesAsync(_context, _siteData.Posts, _dicPostIds);
            await ImportSEOContentDataAsync(_siteData.PostContents, _dicPostContentIds, _dicPostIds);
        }

        private async Task ImportModules()
        {
            await ImportEntitiesAsync(_context, _siteData.Modules, _dicModuleIds);
            await ImportModuleContentsAsync();
            await ImportContentDataAsync(_siteData.ModuleDatas, _dicModuleDataIds, _dicModuleIds);
        }

        private async Task ImportPages()
        {
            await ImportEntitiesAsync(_context, _siteData.Pages, _dicPageIds);
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
                    });
                });
                await ImportEntitiesAsync(_context, _siteData.Templates, _dicTemplateIds);
            }
        }

        private SiteDataViewModel ParseSiteData(SiteDataViewModel siteData)
        {
            string strContent = JObject.FromObject(siteData).ToString();
            var obj = JObject.Parse(ReplaceContent(strContent, siteData.ThemeSystemName));
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
            try
            {
                await ImportAssociationDataAsync(_context, _siteData.PageModules, _dicPageIds, _dicModuleIds);
                await ImportAssociationDataAsync(_context, _siteData.PagePosts, _dicPageIds, _dicPostIds);
                await ImportAssociationDataAsync(_context, _siteData.ModulePosts, _dicModuleIds, _dicPostIds);
                await ImportEntitiesAsync(_context, _siteData.MixUrlAliases, _dicAliasIds);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, nameof(ImportData), ex.Message);
            }

        }

        #region Import Page Data

        private async Task ImportPageContentsAsync()
        {
            foreach (var item in _siteData.PageContents)
            {
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
                }
            }
        }

        #endregion Import Module

        #region Import Database Data

        // TODO: should not import db context, it must be predefined before import database
        private async Task ImportDatabaseContextsAsync(MixCmsContext context)
        {

            foreach (var item in _siteData.MixDatabaseContexts)
            {
                try
                {
                    if (!context.MixDatabaseContext.Any(m => m.SystemName == item.SystemName))
                    {
                        var oldId = item.Id;

                        while (context.MixDatabaseContext.Any(m => m.SystemName == item.SystemName))
                        {
                            item.SystemName = $"{item.SystemName}_1";
                        }
                        item.MixTenantId = CurrentTenant.Id;
                        item.Id = 0;
                        item.CreatedBy = _siteData.CreatedBy;
                        item.CreatedDateTime = DateTime.UtcNow;
                        item.DatabaseProvider = _databaseService.DatabaseProvider;
                        item.ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                        context.Entry(item).State = EntityState.Added;
                        await context.SaveChangesAsync(_cts.Token);
                        _dicMixDatabaseContextIds.Add(oldId, item.Id);
                    }
                }
                catch (MixException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.Badrequest, nameof(ImportDatabaseContextsAsync), item.DisplayName, ex.Message);
                }
            }

        }

        private async Task ImportDatabasesAsync(MixCmsContext context)
        {

            foreach (var item in _siteData.MixDatabases)
            {
                try
                {
                    var oldDbContext = _siteData.MixDatabaseContexts.Find(m => m.Id == item.MixDatabaseContextId);
                    var oldId = item.Id;
                    var oldName = item.SystemName;
                    var currentDbContext = oldDbContext != null ? context.MixDatabaseContext.SingleOrDefault(m => m.SystemName == oldDbContext.SystemName) : default;
                    var currentDb = context.MixDatabase.SingleOrDefault(m => m.SystemName == item.SystemName);


                    // Skip install database if there is no predefined db context
                    if (item.MixDatabaseContextId.HasValue && currentDbContext is null)
                    {
                        continue;
                    }

                    if (currentDb == null)
                    {
                        item.Id = 0;
                        item.MixDatabaseContextId = currentDbContext?.Id;
                        item.CreatedBy = _siteData.CreatedBy;
                        item.CreatedDateTime = DateTime.UtcNow;
                        context.Entry(item).State = EntityState.Added;
                        await context.SaveChangesAsync(_cts.Token);
                        _dicMixDatabaseIds.Add(oldId, item.Id);
                        _dicMixDatabaseNames.Add(oldName, item.SystemName);
                        await ImportDatabaseColumnsAsync(context, item, _siteData.MixDatabaseColumns.Where(m => m.MixDatabaseId == oldId));
                    }
                    else
                    {
                        _dicMixDatabaseIds.Add(oldId, currentDb.Id);
                        _dicMixDatabaseNames.Add(oldName, currentDb.SystemName);
                    }
                }
                catch (MixException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.Badrequest, nameof(ImportDatabasesAsync), item.DisplayName, ex.Message);
                }
            }
        }

        private async Task ImportDatabaseColumnsAsync(MixCmsContext context, MixDatabase database, IEnumerable<MixDatabaseColumn> cols)
        {
            foreach (var item in cols)
            {
                try
                {
                    var table = context.MixDatabaseColumn.AsNoTracking();
                    if (table.Any(m => m.MixDatabaseId == _dicMixDatabaseIds[item.MixDatabaseId] && m.SystemName == item.SystemName))
                    {
                        continue;
                    }
                    var obj = ReflectionHelper.CloneObject(item);
                    obj.Id = 0;
                    obj.CreatedBy = _siteData.CreatedBy;
                    obj.CreatedDateTime = DateTime.UtcNow;
                    obj.MixDatabaseId = database.Id;
                    obj.MixDatabaseName = database.SystemName;
                    context.MixDatabaseColumn.Add(obj);
                    await context.SaveChangesAsync(_cts.Token);
                }
                catch (MixException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.Badrequest, nameof(ImportDatabaseColumnsAsync), database.DisplayName, ex.Message);
                }
            }
        }

        private async Task ImportDatabaseRelationshipsAsync(MixCmsContext dbContext)
        {
            foreach (var item in _siteData.MixDatabaseRelationships)
            {
                try
                {
                    if (_dicMixDatabaseIds.ContainsKey(item.ParentId) && _dicMixDatabaseIds.ContainsKey(item.ChildId))
                    {
                        item.Id = 0;
                        item.ParentId = _dicMixDatabaseIds[item.ParentId];
                        item.ChildId = _dicMixDatabaseIds[item.ChildId];
                        item.CreatedBy = _siteData.CreatedBy;
                        item.CreatedDateTime = DateTime.UtcNow;

                        if (!dbContext.MixDatabaseRelationship.Any(
                                m => m.ParentId == item.ParentId
                                    && m.ChildId == item.ChildId
                                    && m.DisplayName == item.DisplayName))
                        {
                            dbContext.Entry(item).State = EntityState.Added;
                            await dbContext.SaveChangesAsync(_cts.Token);
                        }
                    }
                }
                catch (MixException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.Badrequest, nameof(ImportDatabaseRelationshipsAsync), item.DisplayName, ex.Message);
                }
            }
        }

        public async Task ImportMixDbDataAsync()
        {
            var groupData = _siteData.MixDbModels.GroupBy(m => m.DatabaseName).ToList();
            foreach (var mixGroupData in groupData)
            {
                var oldDbContext = _siteData.MixDatabaseContexts.Find(m => m.Id == _siteData.MixDatabases.First(m => m.SystemName == mixGroupData.Key).MixDatabaseContextId);
                var dbContext = oldDbContext != null ? _context.MixDatabaseContext.SingleOrDefault(m => m.SystemName == oldDbContext.SystemName) : default;
                if (dbContext == null)
                {
                    return;
                }
                var mixDb = await RepoDbMixDatabaseViewModel.GetRepository(_uow, _cacheService).GetSingleAsync(m => m.SystemName == mixGroupData.Key);
                if (mixDb.MixDatabaseContextId.HasValue)
                {

                    var cnn = dbContext.ConnectionString.IsBase64()
                        ? AesEncryptionHelper.DecryptString(dbContext.ConnectionString, GlobalConfigService.Instance.AppSettings.ApiEncryptKey)
                        : dbContext.ConnectionString;
                    _repository.Init(mixDb.SystemName, dbContext.DatabaseProvider, cnn);
                    _fieldNameService = new FieldNameService(dbContext.NamingConvention);
                }
                else
                {
                    _repository.InitTableName(mixDb.SystemName);
                    _fieldNameService = new FieldNameService(MixDatabaseNamingConvention.TitleCase);

                }

                foreach (var mixData in mixGroupData)
                {
                    // Not import user data from other site
                    if (mixData.DatabaseName == MixDatabaseNames.SYSTEM_USER_DATA)
                    {
                        continue;
                    }

                    try
                    {

                        
                        if (mixDb != null && mixData.Data != null && mixData.Data.Count > 0)
                        {
                            List<JObject> lstDto = new();
                            foreach (var jToken in mixData.Data)
                            {
                                lstDto.Add(await MixDbHelper.ParseDtoToEntityAsync((JObject)jToken, mixDb.Type, mixDb.Columns, _fieldNameService, username: _siteData.CreatedBy));
                            }
                            await _repository.InsertManyAsync(lstDto, mixDb);
                            _repository.CompleteTransaction();
                        }
                    }
                    catch (MixException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, nameof(ImportMixDbDataAsync), mixData.DatabaseName, ex.Message);
                    }
                }
            }

        }
        #endregion Import Module

        #endregion

        #region Generic

        private async Task ImportGuidDataAsync<T>(List<T> data)
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

        private async Task ImportEntitiesAsync<T>(MixCmsContext dbContext, List<T> data, Dictionary<int, int> dic)
            where T : EntityBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    try
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
                        dbContext.Entry(item).State = EntityState.Added;
                        await dbContext.SaveChangesAsync(_cts.Token);
                        dic.Add(oldId, item.Id);
                    }
                    catch (MixException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, nameof(ImportEntitiesAsync), item.GetType(), item.Id, ex.Message);
                    }
                }
            }
        }

        private async Task ImportContentDataAsync<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
            where T : MultilingualContentBase<int>
        {
            if (data != null && data.Count > 0)
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
            MixCmsContext dbContext,
            List<T> data,
            Dictionary<int, int> leftDic,
            Dictionary<int, int> rightDic)
            where T : AssociationBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    try
                    {
                        if (leftDic.ContainsKey(item.ParentId) && rightDic.ContainsKey(item.ChildId))
                        {
                            item.Id = 0;
                            item.CreatedBy = _siteData.CreatedBy;
                            item.MixTenantId = CurrentTenant.Id;
                            item.ParentId = leftDic[item.ParentId];
                            item.ChildId = rightDic[item.ChildId];
                            item.CreatedDateTime = DateTime.UtcNow;
                            dbContext.Entry(item).State = EntityState.Added;
                        }
                    }
                    catch (MixException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, nameof(ImportAssociationDataAsync), item.GetType(), item.Id, ex.Message);
                    }
                }
                await dbContext.SaveChangesAsync(_cts.Token);
            }
        }
        #endregion

        private async Task ValidateSiteData(SiteDataViewModel siteData)
        {
            var dbNames = siteData.MixDatabases.Select(m => m.SystemName).ToList();
            var existedDbNameErrors = await _uow.DbContext.MixDatabase
                .Where(m => dbNames.Contains(m.SystemName))
                .Select(m => m.SystemName)
                .ToListAsync();

            siteData.InvalidDatabaseNames.AddRange(existedDbNameErrors);
            siteData.IsValid = !siteData.Errors.Any();
        }

        #endregion Import
    }
}
