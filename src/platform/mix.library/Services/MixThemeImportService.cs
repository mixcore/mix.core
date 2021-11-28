using Mix.Lib.ViewModels;
using Mix.Heart.Exceptions;
using Mix.Heart.Entities;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Mix.Heart.Enums;

namespace Mix.Lib.Services
{
    public class MixThemeImportService
    {
        private UnitOfWorkInfo _uow;
        private CancellationTokenSource _cts;
        private readonly MixCmsContext _context;
        private SiteDataViewModel _siteData;

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

        public MixThemeImportService(MixCmsContext context)
        {
            _context = context;
            _uow = new UnitOfWorkInfo(_context);
            _cts = new CancellationTokenSource();
        }

        #region Import


        public async Task<SiteDataViewModel> LoadTheme(IFormFile themeFile)
        {
            //Save file to temporary folder
            return LoadThemeFiles(themeFile);
        }

        private SiteDataViewModel LoadThemeFiles(IFormFile themeFile)
        {
            if (themeFile != null)
            {
                string importFolder = $"{MixFolders.TempFolder}/{MixFolders.ThemePackage}/{MixThemePackageConstants.TemplateFolder}";
                MixFileService.Instance.EmptyFolder(importFolder);
                var templateAsset = MixHelper.GetFileModel(themeFile, importFolder);
                MixFileService.Instance.SaveFile(themeFile, importFolder);
                MixFileService.Instance.UnZipFile(templateAsset.FullPath, importFolder);
                var strSchema = MixFileService.Instance.GetFile(MixThemePackageConstants.SchemaFilename, MixFileExtensions.Json, $"{templateAsset.FileFolder}/{MixThemePackageConstants.SchemaFolder}");
                var siteStructures = JObject.Parse(strSchema.Content).ToObject<SiteDataViewModel>();

                return siteStructures;
            }
            return null;
        }

        private Dictionary<int, int> dicColumnIds = new Dictionary<int, int>();

        #endregion


        public async Task<SiteDataViewModel> ImportSelectedItemsAsync(SiteDataViewModel siteData)
        {
            try
            {
                _uow.Begin();
                _siteData = siteData;
                if (_siteData.ThemeId == 0)
                {
                    await CreateTheme();
                }
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

        private async Task CreateTheme()
        {
            var table = _context.MixModuleContent.AsNoTracking();
            _siteData.ThemeId = table.Any() ? table.Max(m => m.Id) + 1 : 1;
            var theme = new MixTheme()
            {
                Id = _siteData.ThemeId,
                MixTenantId = 1,
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
                    x.MixThemeId = _siteData.ThemeId;
                    x.MixThemeName = _siteData.ThemeSystemName;
                    x.Content = ReplaceContent(x.Content);
                    x.FileFolder = ReplaceContent(x.FileFolder);
                });
                await ImportEntitiesAsync(_siteData.Templates, dicTemplateIds);
            }
        }

        private string ReplaceContent(string content)
        {
            string accessFolder = string.Empty;
            return content.Replace("[ACCESS_FOLDER]", accessFolder)
                        .Replace("[THEME_NAME]", _siteData.ThemeName);
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
            var table = _context.MixPageContent.AsNoTracking();
            var startId = table.Any() ? table.Max(m => m.Id) : 0;
            foreach (var item in _siteData.PageContents)
            {
                startId++;
                dicPageContentIds.Add(item.Id, startId);

                while (_context.MixPageContent.Any(m => m.SeoName == item.SeoName))
                {
                    item.SeoName = $"{item.SeoName}-1";
                }

                item.Id = startId;
                item.ParentId = dicPageIds[item.ParentId];
                _context.MixPageContent.Add(item);
            }
            await _context.SaveChangesAsync(_cts.Token);
        }
        #endregion Import Page

        #region Import Module Data

        private async Task ImportModuleContentsAsync()
        {
            var table = _context.MixModuleContent.AsNoTracking();
            var startId = table.Any() ? table.Max(m => m.Id) : 0;
            foreach (var item in _siteData.ModuleContents)
            {
                if (!_context.MixModuleContent.Any(m => m.SystemName == item.SystemName))
                {
                    startId++;
                    dicModuleContentIds.Add(item.Id, startId);
                    item.Id = startId;
                    item.ParentId = dicModuleIds[item.ParentId];
                    _context.MixModuleContent.Add(item);
                }
            }
            await _context.SaveChangesAsync(_cts.Token);
        }


        #endregion Import Module

        #region Import Database Data

        private async Task ImportDatabasesAsync()
        {
            var table = _context.MixDatabase.AsNoTracking();
            var startId = table.Any() ? table.Max(m => m.Id) : 0;
            foreach (var item in _siteData.MixDatabases)
            {
                startId++;
                dicMixDatabaseIds.Add(item.Id, startId);

                while (_context.MixDatabase.Any(m => m.SystemName == item.SystemName))
                {
                    item.SystemName = $"{item.SystemName}_1";
                }

                item.Id = startId;
                _context.MixDatabase.Add(item);
            }
            await _context.SaveChangesAsync(_cts.Token);
        }

        private async Task ImportDatabaseColumnsAsync()
        {
            var table = _context.MixDatabaseColumn.AsNoTracking();
            var startId = table.Any() ? table.Max(m => m.Id) : 0;
            foreach (var item in _siteData.MixDatabaseColumns)
            {
                if (table.Any(m => m.MixDatabaseId == item.MixDatabaseId && m.SystemName == item.SystemName))
                {
                    continue;
                }
                startId++;
                dicColumnIds.Add(item.Id, startId);
                item.Id = startId;
                item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                item.MixDatabaseName = _siteData.MixDatabases.First(m => m.Id == item.MixDatabaseId).SystemName;
                _context.MixDatabaseColumn.Add(item);
            }
            await _context.SaveChangesAsync(_cts.Token);
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
                var table = _context.Set<T>().AsNoTracking();
                var startId = table.Any() ? table.Max(m => m.Id) : 0;
                foreach (var item in data)
                {
                    startId += 1;
                    dic.Add(item.Id, startId);
                    item.Id = startId;
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync(_cts.Token);
            }
        }

        private async Task ImportContentDataAsync<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
            where T : MultilanguageContentBase<int>
        {
            if (data.Count > 0)
            {
                var table = _context.Set<T>().AsNoTracking();
                var startId = table.Any() ? table.Max(m => m.Id) : 0;
                foreach (var item in data)
                {
                    startId += 1;
                    dic.Add(item.Id, startId);
                    item.Id = startId;
                    item.ParentId = parentDic[item.ParentId];
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync(_cts.Token);
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
                var table = _context.Set<T>().AsNoTracking();
                var startId = table.Any() ? table.Max(m => m.Id) : 0;
                foreach (var item in data)
                {
                    startId += 1;
                    item.Id = startId;
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
