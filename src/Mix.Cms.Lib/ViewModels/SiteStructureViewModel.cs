using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels
{
    public class SiteStructureViewModel
    {
        [JsonProperty("isIncludeAssets")]
        public bool IsIncludeAssets { get; set; } = true;

        [JsonProperty("isIncludeTemplates")]
        public bool IsIncludeTemplates { get; set; } = true;

        [JsonProperty("isIncludeConfigurations")]
        public bool IsIncludeConfigurations { get; set; } = true;

        [JsonProperty("isIncludePermissions")]
        public bool IsIncludePermissions { get; set; } = true;

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("posts")]
        public List<MixPosts.ImportViewModel> Posts { get; set; } = new List<MixPosts.ImportViewModel>();

        [JsonProperty("pages")]
        public List<MixPages.ImportViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModules.ImportViewModel> Modules { get; set; }

        [JsonProperty("mixDatabases")]
        public List<MixDatabases.ImportViewModel> MixDatabases { get; set; }

        [JsonProperty("mixTemplates")]
        public List<MixTemplates.ImportViewModel> Templates { get; set; } = new List<MixTemplates.ImportViewModel>();

        [JsonProperty("configurations")]
        public List<MixConfigurations.ImportViewModel> Configurations { get; set; } = new List<MixConfigurations.ImportViewModel>();

        [JsonProperty("permissions")]
        public List<MixPortalPages.UpdateViewModel> Permissions { get; set; } = new();

        [JsonProperty("languages")]
        public List<MixLanguages.ImportViewModel> Languages { get; set; } = new List<MixLanguages.ImportViewModel>();

        [JsonProperty("relatedData")]
        public List<MixDatabaseDataAssociations.ImportViewModel> RelatedData { get; set; } = new List<MixDatabaseDataAssociations.ImportViewModel>();

        [JsonProperty("pagePostNavs")]
        public List<MixPagePosts.ImportViewModel> PagePostNavs { get; set; } = new List<MixPagePosts.ImportViewModel>();

        [JsonProperty("pageModuleNavs")]
        public List<MixPageModules.ImportViewModel> PageModuleNavs { get; set; } = new List<MixPageModules.ImportViewModel>();

        [JsonProperty("modulePostNavs")]
        public List<MixModulePosts.ImportViewModel> ModulePostNavs { get; set; } = new List<MixModulePosts.ImportViewModel>();

        [JsonProperty("moduleDatas")]
        public List<MixModuleDatas.ImportViewModel> ModuleDatas { get; set; } = new List<MixModuleDatas.ImportViewModel>();

        [JsonProperty("mixDatabaseDatas")]
        public List<MixDatabaseDatas.ImportViewModel> MixDatabaseDatas { get; set; } = new List<MixDatabaseDatas.ImportViewModel>();

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("themeName")]
        public string ThemeName { get; set; }

        public SiteStructureViewModel()
        {
        }

        public async Task InitAsync(string culture)
        {
            Pages = (await MixPages.ImportViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            Posts = (await MixPosts.ImportViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            Modules = (await MixModules.ImportViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            MixDatabases = (await ViewModels.MixDatabases.ImportViewModel.Repository.GetModelListAsync()).Data;
        }

        #region Export

        public RepositoryResponse<string> ExportSelectedItemsAsync()
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            try
            {
                if (IsIncludeConfigurations)
                {
                    Configurations = MixConfigurations.ImportViewModel.Repository.GetModelListBy(
                        m => m.Specificulture == Specificulture, context, transaction).Data;
                    Languages = MixLanguages.ImportViewModel.Repository.GetModelListBy(
                        m => m.Specificulture == Specificulture, context, transaction).Data;
                }

                if (IsIncludePermissions)
                {
                    Permissions = MixPortalPages.UpdateViewModel.Repository.GetModelListBy(m => m.Level == 0, context, transaction).Data;
                }

                ExportPages(context, transaction);
                ExportModules(context, transaction);
                ExportMixDatabasesAsync(context, transaction);
                ExportDatas(context, transaction);
                if (IsIncludeTemplates)
                {
                    ExportTemplates(context, transaction);
                }
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPages.ImportViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = ex;
                return result;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private void ExportTemplates(MixCmsContext context, IDbContextTransaction transaction)
        {
            Templates = MixTemplates.ImportViewModel.Repository.GetModelList(context, transaction).Data;
        }

        private void ExportDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            ExportMixDatabaseData(context, transaction);
            ExportRelatedDatas(context, transaction);
        }

        private void ExportMixDatabasesAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in MixDatabases)
            {
                item.Columns = MixDatabaseColumns.UpdateViewModel.Repository.GetModelListBy(a => a.MixDatabaseId == item.Id, context, transaction).Data?.OrderBy(a => a.Priority).ToList();
                // Filter list reference field => Add to Export Data if not exist
                var refFields = item.Columns.Where(f => f.DataType == MixDataType.Reference);

                foreach (var field in refFields)
                {
                    var refSet = MixDatabases.FirstOrDefault(m => m.Name == field.MixDatabaseName);
                    if (refSet == null)
                    {
                        var getSet = ViewModels.MixDatabases.ImportViewModel.Repository.GetSingleModel(m => m.Name == field.MixDatabaseName, context, transaction);
                        if (getSet.IsSucceed)
                        {
                            refSet = getSet.Data;
                            refSet.IsExportData = refSet.IsExportData || item.IsExportData;
                            MixDatabases.Add(refSet);
                        }
                    }
                    else
                    {
                        refSet.IsExportData = refSet.IsExportData || item.IsExportData;
                    }
                }
                // Load export data if checked and did not process
                if (item.IsExportData && item.Data != null)
                {
                }
            }
        }

        #region Export Page

        private void ExportPages(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in Pages)
            {
                if (item.IsExportData)
                {
                    ExportPageModuleNav(item, context, transaction);
                    ExportPagePostNav(item, context, transaction);
                    ExportAdditionalData(item.Id.ToString(), MixDatabaseParentType.Page, context, transaction);
                    item.UrlAliases = item.GetAliases(context, transaction);
                }
            }
        }

        private void ExportPageModuleNav(MixPages.ImportViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            PageModuleNavs.AddRange(item.GetModuleNavs(context, transaction)
                .Where(m => !PageModuleNavs.Any(n => n.ModuleId == m.ModuleId && n.PageId == m.PageId)));
            foreach (var nav in PageModuleNavs)
            {
                if (!Modules.Any(m => m.Id == nav.ModuleId && m.Specificulture == Specificulture))
                {
                    Modules.Add(nav.Module);
                }
            }
        }

        private void ExportPagePostNav(MixPages.ImportViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            PagePostNavs.AddRange(item.GetPostNavs(context, transaction)
                .Where(m => !PagePostNavs.Any(n => n.PostId == m.PostId && n.PageId == m.PageId)));
            foreach (var nav in PagePostNavs)
            {
                if (!Posts.Any(m => m.Id == nav.Post.Id && m.Specificulture == Specificulture))
                {
                    Posts.Add(nav.Post);
                }
            }
        }

        #endregion Export Page

        #region Export Modules

        private void ExportModules(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in Modules)
            {
                if (item.IsExportData)
                {
                    ExportModuleDatas(item, context, transaction);
                    ExportModulePostNavs(item, context, transaction);
                }
                ExportAdditionalData(item.Id.ToString(), MixDatabaseParentType.Module, context, transaction);
            }
        }

        private void ExportModuleDatas(MixModules.ImportViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getDataResult = MixModuleDatas.ImportViewModel.Repository
                               .GetModelListBy(m => m.ModuleId == item.Id
                                    && m.Specificulture == item.Specificulture
                               , context, transaction);

            if (getDataResult.IsSucceed)
            {
                ModuleDatas.AddRange(getDataResult.Data);
            }
        }

        private void ExportModulePostNavs(MixModules.ImportViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            ModulePostNavs.AddRange(item.GetPostNavs(context, transaction)
                .Where(m => !ModulePostNavs.Any(n => n.PostId == m.PostId && n.ModuleId == m.ModuleId)));
            foreach (var nav in ModulePostNavs)
            {
                if (!Posts.Any(m => m.Id == nav.Post.Id && m.Specificulture == Specificulture))
                {
                    Posts.Add(nav.Post);
                }
            }
        }

        #endregion Export Modules

        private void ExportAdditionalData(string id, MixDatabaseParentType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!RelatedData.Any(m => m.ParentId == id && m.ParentType == type))
            {
                var getRelatedData = MixDatabaseDataAssociations.ImportViewModel.Repository.GetFirstModel(
                            m => m.Specificulture == Specificulture && m.ParentType == type
                                && m.ParentId == id, context, transaction);
                if (getRelatedData.IsSucceed)
                {
                    RelatedData.Add(getRelatedData.Data);
                }
            }
        }

        private void ExportMixDatabaseData(MixCmsContext context, IDbContextTransaction transaction)
        {
            MixDatabaseDatas = new List<MixDatabaseDatas.ImportViewModel>();
            // Load MixDatabase data
            foreach (var item in MixDatabases)
            {
                if (item.IsExportData)
                {
                    var getData = ViewModels.MixDatabaseDatas.ImportViewModel.Repository.GetModelListBy(
                        a => a.Specificulture == Specificulture && a.MixDatabaseId == item.Id, context, transaction)
                        .Data?.OrderBy(a => a.Priority).ToList();
                    if (getData != null)
                    {
                        MixDatabaseDatas.AddRange(getData);
                    }
                }
            }
            // Load Related Data
            RelatedData.AddRange(Posts.Where(p => p.RelatedData != null).Select(p => p.RelatedData));
            RelatedData.AddRange(Pages.Where(p => p.RelatedData != null).Select(p => p.RelatedData));
            RelatedData.AddRange(Modules.Where(p => p.RelatedData != null).Select(p => p.RelatedData));
            foreach (var item in RelatedData)
            {
                if (!MixDatabaseDatas.Any(m => m.Id == item.Id))
                {
                    var getData = ViewModels.MixDatabaseDatas.ImportViewModel.Repository.GetSingleModel(
                        m => m.Id == item.Id, context, transaction);
                    if (getData.IsSucceed)
                    {
                        MixDatabaseDatas.Add(getData.Data);
                    }
                }
            }
        }

        private void ExportRelatedDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            var postIds = Posts.Select(p => p.Id.ToString()).ToList();
            var pageIds = Pages.Select(p => p.Id.ToString()).ToList();
            var moduleIds = Modules.Select(p => p.Id.ToString()).ToList();
            var relatedIds = RelatedData.Select(p => p.Id).ToList();
            var dataIds = MixDatabaseDatas.Select(p => p.Id).ToList();
            Expression<Func<MixDatabaseDataAssociation, bool>> predicate = m =>
                !relatedIds.Any(r => r == m.Id)
                && m.Specificulture == Specificulture
                && (
                    dataIds.Any(d => d == m.ParentId)
                    || (m.ParentType == MixDatabaseParentType.Page && pageIds.Any(p => p == m.ParentId))
                    || (m.ParentType == MixDatabaseParentType.Post && postIds.Any(p => p == m.ParentId))
                    || (m.ParentType == MixDatabaseParentType.Module && moduleIds.Any(p => p == m.ParentId))
                    );

            var getRelatedData = MixDatabaseDataAssociations.ImportViewModel.Repository
                               .GetModelListBy(predicate, context, transaction);

            if (getRelatedData.IsSucceed && getRelatedData.Data.Count > 0)
            {
                RelatedData.AddRange(getRelatedData.Data);
            }
        }

        #endregion Export

        #region Import

        private Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPermissionIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicFieldIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicMixDatabaseIds = new Dictionary<int, int>();

        public async Task<RepositoryResponse<bool>> ImportAsync(
            int themeId,
            string themeName,
            string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                if (result.IsSucceed && Templates.Count > 0)
                {
                    result = await ImportTemplates(themeId, themeName, context, transaction);
                }

                if (Configurations != null && Configurations.Count > 0)
                {
                    result = await ImportConfigurationsAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && Languages != null && Languages.Count > 0)
                {
                    result = await ImportLanguagesAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && Permissions != null && Permissions.Count > 0)
                {
                    result = await ImportPermissionsAsync(context, transaction);
                }
                if (result.IsSucceed && Pages != null && Pages.Count > 0)
                {
                    result = await ImportPagesAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && Modules != null && Modules.Count > 0)
                {
                    result = await ImportModulesAsync(destCulture, context, transaction);
                }

                if (result.IsSucceed && ModuleDatas.Count > 0)
                {
                    result = await ImportModuleDatas(destCulture, context, transaction);
                }
                if (result.IsSucceed && Posts != null && Posts.Count > 0)
                {
                    result = await ImportPostsAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && MixDatabases != null && MixDatabases.Count > 0)
                {
                    result = await ImportMixDatabasesAsync(context, transaction);
                }
                if (result.IsSucceed && MixDatabaseDatas.Count > 0)
                {
                    result = await ImportMixDatabaseDatas(destCulture, context, transaction);
                }
                if (result.IsSucceed && RelatedData.Count > 0)
                {
                    result = await ImportRelatedDatas(destCulture, context, transaction);
                }


                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPages.ImportViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportTemplates(int themeId, string themeName, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Templates)
            {
                if (result.IsSucceed)
                {
                    item.ThemeId = themeId;
                    item.ThemeName = themeName;
                    var saveResult = await item.SaveModelAsync(true, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportModulesAsync(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var startId = context.MixModule.Any() ? context.MixModule.Max(m => m.Id) : 0;
            foreach (var module in Modules)
            {
                var oldId = module.Id;
                module.CreatedBy = CreatedBy;

                if (result.IsSucceed)
                {
                    if (!context.MixModule.Any(m => m.Name == module.Name && m.Specificulture == destCulture))
                    {
                        startId++;
                        module.Id = startId;
                        module.Specificulture = destCulture;
                        module.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await module.SaveModelAsync(false, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    if (!dicModuleIds.Any(m => m.Key == oldId))
                    {
                        // update new id to related attribute data
                        dicModuleIds.Add(oldId, module.Id);
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportPostsAsync(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            var startId = context.MixPost.Any() ? context.MixPost.Max(m => m.Id) : 0;
            foreach (var post in Posts)
            {
                var oldId = post.Id;
                post.CreatedBy = CreatedBy;

                if (result.IsSucceed)
                {
                    if (!context.MixPost.Any(m => m.SeoName == post.SeoName && m.Specificulture == destCulture))
                    {
                        startId++;
                        post.Id = startId;
                        post.Specificulture = destCulture;
                        post.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await post.SaveModelAsync(true, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    // update new id to related attribute data
                    if (!dicPostIds.Any(m => m.Key == oldId))
                    {
                        dicPostIds.Add(oldId, post.Id);
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportMixDatabasesAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (MixDatabases != null)
            {
                var startId = ViewModels.MixDatabases.ImportViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var startFieldId = MixDatabaseColumns.UpdateViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var mixDatabaseColumns = new List<MixDatabaseColumns.UpdateViewModel>();
                foreach (var set in MixDatabases)
                {
                    set.CreatedBy = CreatedBy;
                    if (result.IsSucceed)
                    {
                        // Import database if not exist in current system (unique by name)
                        if (!context.MixDatabase.Any(m => m.Name == set.Name))
                        {
                            startId++;
                            dicMixDatabaseIds.Add(set.Id, startId);
                            set.Id = startId;
                            set.CreatedDateTime = DateTime.UtcNow;
                            mixDatabaseColumns.AddRange(set.Columns
                                    .Where(m => !mixDatabaseColumns.Any(n => n.Id == m.Id))
                                    .ToList());
                            var saveResult = await set.SaveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        else
                        {
                            dicMixDatabaseIds.Add(set.Id, set.Id);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                // save fields
                if (result.IsSucceed)
                {
                    foreach (var field in mixDatabaseColumns)
                    {
                        if (result.IsSucceed)
                        {
                            field.CreatedBy = CreatedBy;
                            field.MixDatabaseId = dicMixDatabaseIds[field.MixDatabaseId];
                            if (field.ReferenceId.HasValue)
                            {
                                field.ReferenceId = dicMixDatabaseIds[field.ReferenceId.Value];
                            }
                            if (dicFieldIds.ContainsKey(field.Id))
                            {
                                field.Id = dicFieldIds[field.Id];
                                field.CreatedDateTime = DateTime.UtcNow;
                            }
                            else
                            {
                                startFieldId++;
                                dicFieldIds.Add(field.Id, startFieldId);
                                field.Id = startFieldId;
                                field.CreatedDateTime = DateTime.UtcNow;
                            }
                            var saveResult = await field.SaveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        else
                        {
                            result.Errors.Add($"Cannot Import {field.Name} - {field.Id}");
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportConfigurationsAsync(string destCulture,
          MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int startId = MixConfigurations.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                foreach (var item in Configurations)
                {
                    var oldId = item.Id;
                    item.CreatedBy = CreatedBy;
                    if (!context.MixConfiguration.Any(p => p.Keyword == item.Keyword))
                    {
                        startId++;
                        item.Id = startId;

                        item.CreatedDateTime = DateTime.UtcNow;

                        item.Specificulture = destCulture;
                        var saveResult = await item.SaveModelAsync(false, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors = saveResult.Errors;
                            break;
                        }
                    }
                    if (!dicConfigurationIds.Any(m => m.Key == item.Id))
                    {
                        dicConfigurationIds.Add(oldId, startId);
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPages.ImportViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportLanguagesAsync(string destCulture,
          MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int startId = MixLanguages.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                foreach (var item in Languages)
                {
                    var oldId = item.Id;
                    item.CreatedBy = CreatedBy;
                    if (!context.MixLanguage.Any(p => p.Keyword == item.Keyword))
                    {
                        startId++;
                        item.Id = startId;

                        item.CreatedDateTime = DateTime.UtcNow;

                        item.Specificulture = destCulture;
                        var saveResult = await item.SaveModelAsync(false, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors = saveResult.Errors;
                            break;
                        }
                    }
                    if (!dicLanguageIds.Any(m => m.Key == item.Id))
                    {
                        dicLanguageIds.Add(oldId, startId);
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPages.ImportViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportPermissionsAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int startId = MixPortalPages.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                foreach (var item in Permissions)
                {
                    var oldId = item.Id;
                    item.CreatedBy = CreatedBy;
                    startId++;
                    item.Id = startId;

                    item.CreatedDateTime = DateTime.UtcNow;

                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    if (!saveResult.IsSucceed)
                    {
                        result.IsSucceed = false;
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }

                    if (!dicPermissionIds.Any(m => m.Key == item.Id))
                    {
                        dicPermissionIds.Add(oldId, startId);
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPortalPages.UpdateViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportPagesAsync(string destCulture,
          MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int startId = MixPages.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                int startModuleId = MixModules.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                //var pages = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_PAGES, MixFolders.JsonDataFolder, true, "{}");
                //var obj = JObject.Parse(pages.Content);
                //var initPages = obj["data"].ToObject<JArray>();
                foreach (var item in Pages)
                {
                    // store old id => update to related data if save success
                    var oldId = item.Id;
                    item.CreatedBy = CreatedBy;
                    // TODO: Id > 7 => not system init page
                    if (!context.MixPage.Any(p => p.SeoName == item.SeoName))
                    {
                        startId++;
                        item.Id = startId;

                        item.CreatedDateTime = DateTime.UtcNow;
                        item.ThemeName = ThemeName;

                        item.Specificulture = destCulture;
                        var saveResult = await item.SaveModelAsync(false, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors = saveResult.Errors;
                            break;
                        }
                    }
                    if (!dicPageIds.Any(m => m.Key == item.Id))
                    {
                        dicPageIds.Add(oldId, startId);
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPages.ImportViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportMixDatabaseDatas(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in MixDatabaseDatas)
            {
                item.CreatedBy = CreatedBy;
                if (result.IsSucceed)
                {
                    item.CreatedBy = CreatedBy;
                    if (!context.MixDatabaseData.Any(m => m.Id == item.Id && m.Specificulture == item.Specificulture))
                    {
                        item.Specificulture = destCulture;
                        item.CreatedDateTime = DateTime.UtcNow;
                        // update new Id if not system attribute
                        if (dicMixDatabaseIds.ContainsKey(item.MixDatabaseId))
                        {
                            item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                        }
                        item.Columns = item.Columns ?? MixDatabases.FirstOrDefault(m => m.Name == item.MixDatabaseName).Columns;
                        foreach (var field in item.Columns)
                        {
                            field.Specificulture = destCulture;
                            var newSet = MixDatabases.FirstOrDefault(m => m.Name == field.MixDatabaseName);
                            var newField = newSet?.Columns.FirstOrDefault(m => m.Name == field.Name);
                            if (newField != null)
                            {
                                field.Id = newField.Id;
                                field.MixDatabaseId = newSet.Id;
                                field.MixDatabaseName = newSet.Name;
                                field.CreatedDateTime = DateTime.UtcNow;
                            }
                        }
                        var saveResult = await item.SaveModelAsync(true, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportRelatedDatas(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = await ImportRelatedMixDatabaseDatas(desCulture, context, transaction);
            if (result.IsSucceed)
            {
                result = await ImportPagePostNavs(desCulture, context, transaction);
            }

            if (result.IsSucceed)
            {
                result = await ImportPageModuleNavs(desCulture, context, transaction);
            }
            if (result.IsSucceed)
            {
                result = await ImportModulePostNavs(desCulture, context, transaction);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportPagePostNavs(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in PagePostNavs)
            {
                if (result.IsSucceed)
                {
                    item.CreatedBy = CreatedBy;
                    item.Specificulture = desCulture;
                    item.PageId = dicPageIds[item.PageId];
                    item.PostId = dicPostIds[item.PostId];
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportPageModuleNavs(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in PageModuleNavs)
            {
                if (result.IsSucceed)
                {
                    item.CreatedBy = CreatedBy;
                    item.Specificulture = desCulture;
                    item.PageId = dicPageIds[item.PageId];
                    item.ModuleId = dicModuleIds[item.ModuleId];
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportModulePostNavs(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in ModulePostNavs)
            {
                if (result.IsSucceed)
                {
                    item.CreatedBy = CreatedBy;
                    item.Specificulture = desCulture;
                    item.ModuleId = dicModuleIds[item.ModuleId];
                    item.PostId = dicPostIds[item.PostId];
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportModuleDatas(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in ModuleDatas)
            {
                item.CreatedBy = CreatedBy;
                if (result.IsSucceed)
                {
                    item.Specificulture = desCulture;
                    item.ModuleId = dicModuleIds[item.ModuleId];
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportRelatedMixDatabaseDatas(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RelatedData)
            {
                item.CreatedBy = CreatedBy;
                item.Id = Guid.NewGuid().ToString();
                item.Specificulture = desCulture;
                switch (item.ParentType)
                {
                    case MixDatabaseParentType.Set:
                        item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                        break;

                    case MixDatabaseParentType.Post:
                        break;

                    case MixDatabaseParentType.Page:
                        if (dicPageIds.TryGetValue(int.Parse(item.ParentId), out int pageId))
                        {
                            item.ParentId = pageId.ToString();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case MixDatabaseParentType.Module:
                        if (dicModuleIds.TryGetValue(int.Parse(item.ParentId), out int moduleId))
                        {
                            item.ParentId = moduleId.ToString();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    default:
                        break;
                }
                if (result.IsSucceed)
                {
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        #endregion Import
    }
}