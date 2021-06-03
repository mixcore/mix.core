using Microsoft.EntityFrameworkCore.Storage;
using Mix.Shared.Enums;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Theme.Domain.ViewModels.Import;
using Mix.Lib.Entities.Cms;
using Mix.Lib.ViewModels.Cms;

namespace Mix.Theme.Domain.ViewModels
{
    public class SiteStructureViewModel
    {
        public string CreatedBy { get; set; }

        public List<ImportPostViewModel> Posts { get; set; } = new List<ImportPostViewModel>();
        
        public List<ImportPageViewModel> Pages { get; set; }

        public List<ImportModuleViewModel> Modules { get; set; }

        public List<ImportMixDatabaseViewModel> MixDatabases { get; set; }

        public List<MixConfigurationViewModel> Configurations { get; set; }

        public List<MixLanguageViewModel> Languages { get; set; }

        public List<ImportMixDataAssociationViewModel> RelatedData { get; set; } = new List<ImportMixDataAssociationViewModel>();

        public List<MixPagePostViewModel> PagePostNavs { get; set; } = new List<MixPagePostViewModel>();

        public List<MixPageModuleViewModel> PageModuleNavs { get; set; } = new List<MixPageModuleViewModel>();

        public List<MixModulePostViewModel> ModulePostNavs { get; set; } = new List<MixModulePostViewModel>();

        public List<MixModuleDataViewModel> ModuleDatas { get; set; } = new List<MixModuleDataViewModel>();

        public List<ImportMixDataViewModel> MixDatabaseDatas { get; set; } = new List<ImportMixDataViewModel>();

        public string Specificulture { get; set; }

        public string ThemeName { get; set; }

        public SiteStructureViewModel()
        {
        }

        public async Task InitAsync(string culture)
        {
            Pages = (await ImportPageViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            Posts = (await ImportPostViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            Modules = (await ImportModuleViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            MixDatabases = (await ImportMixDatabaseViewModel.Repository.GetModelListAsync()).Data;
        }

        #region Export

        public RepositoryResponse<string> ExportSelectedItemsAsync()
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            try
            {
                Configurations = MixConfigurationViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture, context, transaction).Data;
                Languages = MixLanguageViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture, context, transaction).Data;

                ExportPages(context, transaction);
                ExportModules(context, transaction);
                ExportMixDatabasesAsync(context, transaction);
                ExportDatas(context, transaction);
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ImportPageViewModel>(ex, isRoot, transaction);
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

        private void ExportDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            ExportMixDatabaseData(context, transaction);
            ExportRelatedDatas(context, transaction);
        }

        private void ExportMixDatabasesAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in MixDatabases)
            {
                item.Fields = ImportaMixDatabaseColumnViewModel.Repository.GetModelListBy(a => a.MixDatabaseId == item.Id, context, transaction).Data?.OrderBy(a => a.Priority).ToList();
                // Filter list reference field => Add to Export Data if not exist
                var refFields = item.Fields.Where(f => f.DataType == MixDataType.Reference);

                foreach (var field in refFields)
                {
                    var refSet = MixDatabases.FirstOrDefault(m => m.Name == field.MixDatabaseName);
                    if (refSet == null)
                    {
                        var getSet = ImportMixDatabaseViewModel.Repository.GetSingleModel(m => m.Name == field.MixDatabaseName, context, transaction);
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

        private void ExportPageModuleNav(ImportPageViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            PageModuleNavs.AddRange(item.GetModuleNavs(context, transaction)
                .Where(m => !PageModuleNavs.Any(n => n.ModuleId == m.ModuleId && n.PageId == m.PageId)));
            foreach (var nav in PageModuleNavs)
            {
                if (!Modules.Any(m => m.Id == nav.ModuleId && m.Specificulture == Specificulture))
                {
                    var getModule = ImportModuleViewModel.Repository.GetSingleModel(
                        m => m.Id == nav.ModuleId && m.Specificulture == nav.Specificulture
                        , context, transaction);
                    if (getModule.IsSucceed)
                    {
                        Modules.Add(getModule.Data);
                    }
                }
            }
        }

        private void ExportPagePostNav(ImportPageViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            PagePostNavs.AddRange(item.GetPostNavs(context, transaction)
                .Where(m => !PagePostNavs.Any(n => n.PostId == m.PostId && n.PageId == m.PageId)));
            foreach (var nav in PagePostNavs)
            {
                if (!Posts.Any(m => m.Id == nav.PostId && m.Specificulture == Specificulture))
                {
                    var getPost = ImportPostViewModel.Repository.GetSingleModel(
                        m => m.Id == nav.PostId && m.Specificulture == nav.Specificulture
                        , context, transaction);
                    if (getPost.IsSucceed)
                    {
                        Posts.Add(getPost.Data);
                    }
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

        private void ExportModuleDatas(ImportModuleViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getDataResult = MixModuleDataViewModel.Repository
                               .GetModelListBy(m => m.ModuleId == item.Id
                                    && m.Specificulture == item.Specificulture
                               , context, transaction);

            if (getDataResult.IsSucceed)
            {
                ModuleDatas.AddRange(getDataResult.Data);
            }
        }

        private void ExportModulePostNavs(ImportModuleViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            ModulePostNavs.AddRange(item.GetPostNavs(context, transaction)
                .Where(m => !ModulePostNavs.Any(n => n.PostId == m.PostId && n.ModuleId == m.ModuleId)));
            foreach (var nav in ModulePostNavs)
            {
                if (!Posts.Any(m => m.Id == nav.PostId && m.Specificulture == Specificulture))
                {
                    var getPost = ImportPostViewModel.Repository.GetSingleModel(
                        m => m.Id == nav.PostId && m.Specificulture == nav.Specificulture
                        , context, transaction);
                    if (getPost.IsSucceed)
                    {
                        Posts.Add(getPost.Data);
                    }
                }
            }
        }

        #endregion Export Modules

        private void ExportAdditionalData(string id, MixDatabaseParentType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!RelatedData.Any(m => m.ParentId == id && m.ParentType == type))
            {
                var getRelatedData = ImportMixDataAssociationViewModel.Repository.GetSingleModel(
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
            MixDatabaseDatas = new List<ImportMixDataViewModel>();
            // Load MixDatabase data
            foreach (var item in MixDatabases)
            {
                if (item.IsExportData)
                {
                    var getData = ImportMixDataViewModel.Repository.GetModelListBy(
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
                    var getData = ImportMixDataViewModel.Repository.GetSingleModel(
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
            foreach (var item in MixDatabaseDatas)
            {
                var getDataResult = ImportMixDataAssociationViewModel.Repository
                                   .GetModelListBy(m => m.ParentId == item.Id && m.Specificulture == item.Specificulture
                                   , context, transaction);

                if (getDataResult.IsSucceed && getDataResult.Data.Count > 0)
                {
                    var data = getDataResult.Data.Where(m =>
                        MixDatabaseDatas.Any(d => d.Id == m.DataId)
                        && !RelatedData.Any(r => r.ParentId == item.Id && r.DataId == m.DataId))
                        .ToList();
                    RelatedData.AddRange(data);
                }
            }
        }

        #endregion Export

        #region Import

        private Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicFieldIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicMixDatabaseIds = new Dictionary<int, int>();

        public async Task<RepositoryResponse<bool>> ImportAsync(
            string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                if (Configurations != null && Configurations.Count > 0)
                {
                    result = await ImportConfigurationsAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && Languages != null && Languages.Count > 0)
                {
                    result = await ImportLanguagesAsync(destCulture, context, transaction);
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
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ImportPageViewModel>(ex, isRoot, transaction);
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
                var startId = ImportMixDatabaseViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var startFieldId = ImportaMixDatabaseColumnViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var mixDatabaseColumns = new List<ImportaMixDatabaseColumnViewModel>();
                foreach (var set in MixDatabases)
                {
                    set.CreatedBy = CreatedBy;
                    if (result.IsSucceed)
                    {                        
                        if (!context.MixDatabase.Any(m => m.Name == set.Name))
                        {
                            startId++;
                            if (!dicMixDatabaseIds.Any(m => m.Key == set.Id))
                            {
                                dicMixDatabaseIds.Add(set.Id, startId);
                            }
                            set.Id = startId;
                            set.CreatedDateTime = DateTime.UtcNow;
                            mixDatabaseColumns.AddRange(set.Fields
                                    .Where(m => !mixDatabaseColumns.Any(n => n.Id == m.Id))
                                    .ToList());
                            var saveResult = await set.SaveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
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
                            var setId = dicMixDatabaseIds.FirstOrDefault(m => m.Key == field.MixDatabaseId);
                            field.MixDatabaseId = setId.Value;
                            if (field.ReferenceId != null)
                            {
                                var refId = dicMixDatabaseIds.FirstOrDefault(m => m.Key == field.ReferenceId);
                                field.ReferenceId = refId.Value;
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
                int startId = MixConfigurationViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
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
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ImportPageViewModel>(ex, isRoot, transaction);
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
                int startId = MixLanguageViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
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
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ImportPageViewModel>(ex, isRoot, transaction);
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
                int startId = MixPageViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                int startModuleId = MixModuleViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
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
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ImportPageViewModel>(ex, isRoot, transaction);
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
                        if (item.MixDatabaseName.IndexOf("sys_") != 0 && dicMixDatabaseIds.ContainsKey(item.MixDatabaseId))
                        {
                            item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                        }
                        item.Columns = item.Columns ?? ImportaMixDatabaseColumnViewModel.Repository.GetModelListBy(
                            m => m.MixDatabaseId == item.MixDatabaseId, context, transaction).Data;
                        foreach (var field in item.Columns)
                        {
                            field.Specificulture = destCulture;
                            var newSet = MixDatabases.FirstOrDefault(m => m.Name == field.MixDatabaseName);
                            var newField = newSet?.Fields.FirstOrDefault(m => m.Name == field.Name);
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