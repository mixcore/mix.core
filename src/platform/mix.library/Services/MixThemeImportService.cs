using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;

namespace Mix.Lib.Services
{
    public class MixThemeImportService
    {
        private UnitOfWorkInfo _uow;
        private CancellationTokenSource _cts;
        private readonly MixCmsContext _context;
        private SiteDataViewModel _siteData;
        public readonly int tenantId;
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

        public MixThemeImportService(MixCmsContext cmsContext, IHttpContextAccessor httpContext)
        {
            _context = cmsContext;
            _uow = new UnitOfWorkInfo(_context);
            _cts = new CancellationTokenSource();
            if (httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                tenantId = httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
        }

        #region Import

        public async Task DownloadThemeAsync(
            JObject theme, IProgress<int> progress, HttpService httpService)
        {
            string name = theme.Value<string>("name");
            MixFileHelper.EmptyFolder(MixFolders.ThemePackage);
            var cancellationToken = new CancellationToken();
            string filePath = $"{MixFolders.ThemePackage}/{name}{MixFileExtensions.Zip}";
            await httpService.DownloadAsync(
                theme.Value<string>("source"),
                MixFolders.ThemePackage,
                name, MixFileExtensions.Zip,
                progress, cancellationToken);
            MixFileHelper.UnZipFile(filePath, MixFolders.ThemePackage);
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
                    await CreateTheme();
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
            string srcAssets = $"{MixFolders.ThemePackage}/{MixThemePackageConstants.AssetFolder}";
            string destAssets = $"{MixFolders.WebRootPath}/{MixFolders.SiteContentAssetsFolder}/{_siteData.ThemeSystemName}";
            string srcUpload = $"{MixFolders.ThemePackage}/{MixThemePackageConstants.UploadFolder}";
            string destUpload = $"{MixFolders.WebRootPath}/{MixFolders.UploadsFolder}";
            MixFileHelper.CopyFolder(srcAssets, destAssets);
            MixFileHelper.CopyFolder(srcUpload, destUpload);
        }

        private async Task CreateTheme()
        {
            var table = _context.MixModuleContent.AsNoTracking();
            _siteData.ThemeId = table.Any() ? table.Max(m => m.Id) + 1 : 1;
            var theme = new MixTheme()
            {
                Id = _siteData.ThemeId,
                MixTenantId = tenantId,
                DisplayName = _siteData.ThemeName,
                SystemName = _siteData.ThemeSystemName,
                CreatedBy = _siteData.CreatedBy,
                Status = MixContentStatus.Published
            };
            _context.MixTheme.Add(theme);
            await _context.SaveChangesAsync(_cts.Token);
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
            await ImportDatabasesAsync();
            await ImportDatabaseColumnsAsync();
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
                    x.MixTenantId = tenantId;
                    x.MixThemeId = _siteData.ThemeId;
                    x.MixThemeName = _siteData.ThemeSystemName;
                    x.Content = ReplaceContent(x.Content, _siteData.ThemeSystemName);
                    x.FileFolder = ReplaceContent(x.FileFolder, _siteData.ThemeSystemName);
                    MixFileHelper.SaveFile(new FileModel()
                    {
                        Content = x.Content,
                        Extension = x.Extension,
                        FileFolder = x.FileFolder,
                        Filename = x.FileName
                    });
                });
                await ImportEntitiesAsync(_siteData.Templates, dicTemplateIds);
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
            return content.Replace("[THEME_NAME]", themeName);
        }

        #endregion

        #region Import Datas

        private async Task ImportData()
        {
            await ImportAssociationDataAsync(_siteData.PageModules, dicPageIds, dicModuleIds);
            await ImportAssociationDataAsync(_siteData.PagePosts, dicPageIds, dicPostIds);
            await ImportAssociationDataAsync(_siteData.ModulePosts, dicModuleIds, dicPostIds);

            await ImportDatabaseDataAsync();

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
                item.MixTenantId = tenantId;
                item.ParentId = dicPageIds[item.ParentId];
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
                    item.MixTenantId = tenantId;
                    item.ParentId = dicModuleIds[item.ParentId];
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dicModuleContentIds.Add(oldId, item.Id);
                }
            }
        }


        #endregion Import Module

        #region Import Database Data

        private async Task ImportDatabasesAsync()
        {
            foreach (var item in _siteData.MixDatabases)
            {
                var oldId = item.Id;

                while (_context.MixDatabase.Any(m => m.SystemName == item.SystemName))
                {
                    item.SystemName = $"{item.SystemName}_1";
                }
                item.MixTenantId = tenantId;
                item.Id = 0;
                _context.Entry(item).State = EntityState.Added;
                await _context.SaveChangesAsync(_cts.Token);
                dicMixDatabaseIds.Add(oldId, item.Id);
            }
        }

        private async Task ImportDatabaseColumnsAsync()
        {
            foreach (var item in _siteData.MixDatabaseColumns)
            {
                var table = _context.MixDatabaseColumn.AsNoTracking();
                if (table.Any(m => m.MixDatabaseId == item.MixDatabaseId && m.SystemName == item.SystemName))
                {
                    continue;
                }
                var oldId = item.Id;
                item.Id = 0;
                item.MixTenantId = tenantId;
                item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                item.MixDatabaseName = _siteData.MixDatabases.First(m => m.Id == item.MixDatabaseId).SystemName;
                _context.MixDatabaseColumn.Add(item);
                await _context.SaveChangesAsync(_cts.Token);
                dicColumnIds.Add(oldId, item.Id);
            }
        }

        private async Task ImportDatabaseDataAsync()
        {
            await ImportGuidDatasAsync(_siteData.Datas);
            await ImportGuidDatasAsync(_siteData.DataContents);
            await ImportGuidDatasAsync(_siteData.DataContentValues);
            await ImportGuidDatasAsync(_siteData.DataContentAssociations);
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
                    if (ReflectionHelper.HasProperty(typeof(T), MixRequestQueryKeywords.MixTenantId))
                    {
                        ReflectionHelper.SetPropertyValue(item, new EntityPropertyModel()
                        {
                            PropertyName = MixRequestQueryKeywords.MixTenantId,
                            PropertyValue = tenantId
                        });
                    }
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

                    if (ReflectionHelper.HasProperty(typeof(T), MixRequestQueryKeywords.MixTenantId))
                    {
                        ReflectionHelper.SetPropertyValue(item, new EntityPropertyModel()
                        {
                            PropertyName = MixRequestQueryKeywords.MixTenantId,
                            PropertyValue = tenantId
                        });
                    }

                    item.Id = 0;
                    _context.Entry(item).State = EntityState.Added;
                    await _context.SaveChangesAsync(_cts.Token);
                    dic.Add(oldId, item.Id);
                }
            }
        }

        private async Task ImportContentDataAsync<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
            where T : MultiLanguageContentBase<int>
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var oldId = item.Id;
                    item.Id = 0;
                    item.MixTenantId = tenantId;
                    item.ParentId = parentDic[item.ParentId];
                    item.Specificulture ??= _siteData.Specificulture;
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
                    item.MixTenantId = tenantId;
                    item.LeftId = leftDic[item.LeftId];
                    item.RightId = rightDic[item.RightId];
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync(_cts.Token);
            }
        }
        #endregion

        #endregion Import
    }
}
