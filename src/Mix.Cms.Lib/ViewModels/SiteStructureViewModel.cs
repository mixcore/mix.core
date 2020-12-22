using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels
{
    public class SiteStructureViewModel
    {
        [JsonProperty("pages")]
        public List<MixPages.ImportViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModules.ImportViewModel> Modules { get; set; }

        [JsonProperty("attributeSets")]
        public List<MixAttributeSets.ImportViewModel> AttributeSets { get; set; }

        [JsonProperty("configurations")]
        public List<MixConfigurations.ImportViewModel> Configurations { get; set; }

        [JsonProperty("languages")]
        public List<MixLanguages.ImportViewModel> Languages { get; set; }

        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.ImportViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.ImportViewModel>();

        [JsonProperty("pagePostNavs")]
        public List<MixPagePosts.ImportViewModel> PagePostNavs { get; set; } = new List<MixPagePosts.ImportViewModel>();

        [JsonProperty("pageModuleNavs")]
        public List<MixPageModules.ImportViewModel> PageModuleNavs { get; set; } = new List<MixPageModules.ImportViewModel>();

        [JsonProperty("modulePostNavs")]
        public List<MixModulePosts.ImportViewModel> ModulePostNavs { get; set; } = new List<MixModulePosts.ImportViewModel>();

        [JsonProperty("posts")]
        public List<MixPosts.ImportViewModel> Posts { get; set; } = new List<MixPosts.ImportViewModel>();

        [JsonProperty("moduleDatas")]
        public List<MixModuleDatas.ImportViewModel> ModuleDatas { get; set; } = new List<MixModuleDatas.ImportViewModel>();

        [JsonProperty("attributeSetDatas")]
        public List<MixAttributeSetDatas.ImportViewModel> AttributeSetDatas { get; set; } = new List<MixAttributeSetDatas.ImportViewModel>();

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
            Modules = (await MixModules.ImportViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
            AttributeSets = (await MixAttributeSets.ImportViewModel.Repository.GetModelListAsync()).Data;
        }

        #region Export

        public RepositoryResponse<string> ExportSelectedItemsAsync()
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            try
            {
                Configurations = MixConfigurations.ImportViewModel.Repository.GetModelListBy(
                    m => !m.Keyword.Contains("sys_") && m.Specificulture == Specificulture, context, transaction).Data;
                Languages = MixLanguages.ImportViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture, context, transaction).Data;

                ExportPages(context, transaction);
                ExportModules(context, transaction);
                ExportAttributeSetsAsync(context, transaction);
                ExportDatas(context, transaction);
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

        private void ExportDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            ExportAttributeSetData(context, transaction);
            ExportRelatedDatas(context, transaction);
        }

        private void ExportAttributeSetsAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in AttributeSets)
            {
                item.Fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(a => a.AttributeSetId == item.Id, context, transaction).Data?.OrderBy(a => a.Priority).ToList();
                // Filter list reference field => Add to Export Data if not exist
                var refFields = item.Fields.Where(f => f.DataType == MixEnums.MixDataType.Reference);

                foreach (var field in refFields)
                {
                    var refSet = AttributeSets.FirstOrDefault(m => m.Name == field.AttributeSetName);
                    if (refSet == null)
                    {
                        var getSet = MixAttributeSets.ImportViewModel.Repository.GetSingleModel(m => m.Name == field.AttributeSetName, context, transaction);
                        if (getSet.IsSucceed)
                        {
                            refSet = getSet.Data;
                            refSet.IsExportData = refSet.IsExportData || item.IsExportData;
                            AttributeSets.Add(refSet);
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
                    ExportAddictionalData(item.Id.ToString(), MixEnums.MixAttributeSetDataType.Page, context, transaction);
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

        #endregion

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
                ExportAddictionalData(item.Id.ToString(), MixEnums.MixAttributeSetDataType.Module, context, transaction);
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
        #endregion


        private void ExportAddictionalData(string id, MixEnums.MixAttributeSetDataType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!RelatedData.Any(m => m.ParentId == id && m.ParentType == type))
            {
                var getRelatedData = MixRelatedAttributeDatas.ImportViewModel.Repository.GetSingleModel(
                            m => m.Specificulture == Specificulture && m.ParentType == type.ToString()
                                && m.ParentId == id, context, transaction);
                if (getRelatedData.IsSucceed)
                {
                    RelatedData.Add(getRelatedData.Data);
                }
            }
        }

        private void ExportAttributeSetData(MixCmsContext context, IDbContextTransaction transaction)
        {
            AttributeSetDatas = new List<MixAttributeSetDatas.ImportViewModel>();
            // Load AttributeSet data
            foreach (var item in AttributeSets)
            {
                if (item.IsExportData)
                {
                    var getData = MixAttributeSetDatas.ImportViewModel.Repository.GetModelListBy(
                        a => a.Specificulture == Specificulture && a.AttributeSetId == item.Id, context, transaction)
                        .Data?.OrderBy(a => a.Priority).ToList();
                    if (getData != null)
                    {
                        AttributeSetDatas.AddRange(getData);
                    }
                }
            }
            // Load Related Data
            RelatedData.AddRange(Posts.Where(p => p.RelatedData != null).Select(p => p.RelatedData));
            foreach (var item in RelatedData)
            {
                if (!AttributeSetDatas.Any(m => m.Id == item.Id))
                {
                    var getData = MixAttributeSetDatas.ImportViewModel.Repository.GetSingleModel(
                        m => m.Id == item.Id, context, transaction);
                    if (getData.IsSucceed)
                    {
                        AttributeSetDatas.Add(getData.Data);
                    }
                }
            }
        }

        private void ExportRelatedDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in AttributeSetDatas)
            {
                var getDataResult = MixRelatedAttributeDatas.ImportViewModel.Repository
                                   .GetModelListBy(m => m.ParentId == item.Id && m.Specificulture == item.Specificulture
                                   , context, transaction);

                if (getDataResult.IsSucceed && getDataResult.Data.Count > 0)
                {
                    var data = getDataResult.Data.Where(m =>
                        AttributeSetDatas.Any(d => d.Id == m.DataId)
                        && !RelatedData.Any(r => r.ParentId == item.Id && r.DataId == m.DataId))
                        .ToList();
                    RelatedData.AddRange(data);
                }
            }
        }
        #endregion Export

        #region Import

        Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        Dictionary<int, int> dicFieldIds = new Dictionary<int, int>();
        Dictionary<int, int> dicAttributeSetIds = new Dictionary<int, int>();

        public async Task<RepositoryResponse<bool>> ImportAsync(string destCulture,
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
                if (result.IsSucceed && AttributeSets != null && AttributeSets.Count > 0)
                {
                    result = await ImportAttributeSetsAsync(context, transaction);
                }
                if (result.IsSucceed && AttributeSetDatas.Count > 0)
                {
                    result = await ImportAttributeSetDatas(destCulture, context, transaction);
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

        private async Task<RepositoryResponse<bool>> ImportModulesAsync(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var startId = context.MixModule.Any() ? context.MixModule.Max(m => m.Id) : 0;
            foreach (var module in Modules)
            {

                var oldId = module.Id;

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

        private async Task<RepositoryResponse<bool>> ImportAttributeSetsAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (AttributeSets != null)
            {
                var startId = MixAttributeSets.ImportViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var startFieldId = MixAttributeFields.UpdateViewModel.Repository.Max(m => m.Id, context, transaction).Data;
                var attributeFields = new List<MixAttributeFields.UpdateViewModel>();
                foreach (var set in AttributeSets)
                {
                    if (result.IsSucceed)
                    {
                        if (!context.MixAttributeSet.Any(m => m.Name == set.Name))
                        {
                            startId++;
                            set.Id = startId;
                            set.CreatedDateTime = DateTime.UtcNow;
                            attributeFields.AddRange(set.Fields
                                    .Where(m => !attributeFields.Any(n => n.Id == m.Id))
                                    .ToList());
                            var saveResult = await set.SaveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        if (!dicAttributeSetIds.Any(m => m.Key == set.Id))
                        {
                            dicAttributeSetIds.Add(set.Id, startId);
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
                    foreach (var field in attributeFields)
                    {
                        if (result.IsSucceed)
                        {
                            var setId = dicAttributeSetIds.FirstOrDefault(m => m.Key == field.AttributeSetId);
                            field.AttributeSetId = setId.Value;
                            if (field.ReferenceId != null)
                            {
                                var refId = dicAttributeSetIds.FirstOrDefault(m => m.Key == field.ReferenceId);
                                field.ReferenceId = refId.Value;

                            }
                            if (!dicFieldIds.Any(m => m.Key == field.Id))
                            {
                                startFieldId++;
                                dicFieldIds.Add(field.Id, startFieldId);
                                field.Id = startFieldId;
                                field.CreatedDateTime = DateTime.UtcNow;
                            }
                            else
                            {
                                var current = dicFieldIds.FirstOrDefault(m => m.Key == field.Id);
                                field.Id = current.Value;
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

        private async Task<RepositoryResponse<bool>> ImportPagesAsync(string destCulture,
          MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int startId = MixPages.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                int startModuleId = MixModules.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data;
                //var pages = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_PAGES, "data", true, "{}");
                //var obj = JObject.Parse(pages.Content);
                //var initPages = obj["data"].ToObject<JArray>();
                foreach (var item in Pages)
                {
                    // store old id => update to related data if save success
                    var oldId = item.Id;
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

        private async Task<RepositoryResponse<bool>> ImportAttributeSetDatas(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in AttributeSetDatas)
            {
                if (result.IsSucceed)
                {
                    if (!context.MixAttributeSetData.Any(m => m.Id == item.Id && m.Specificulture == item.Specificulture))
                    {
                        item.Specificulture = destCulture;
                        item.CreatedDateTime = DateTime.UtcNow;
                        // update new Id if not system attribute
                        if (item.AttributeSetName.IndexOf("sys_") != 0 && dicAttributeSetIds.ContainsKey(item.AttributeSetId))
                        {
                            item.AttributeSetId = dicAttributeSetIds[item.AttributeSetId];
                        }
                        item.Fields = item.Fields ?? MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(
                            m => m.AttributeSetId == item.AttributeSetId, context, transaction).Data;
                        foreach (var field in item.Fields)
                        {
                            field.Specificulture = destCulture;
                            var newSet = AttributeSets.FirstOrDefault(m => m.Name == field.AttributeSetName);
                            var newField = newSet?.Fields.FirstOrDefault(m => m.Name == field.Name);
                            if (newField != null)
                            {
                                field.Id = newField.Id;
                                field.AttributeSetId = newSet.Id;
                                field.AttributeSetName = newSet.Name;
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

            var result = await ImportRelatedAttributeSetDatas(desCulture, context, transaction);
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

        private async Task<RepositoryResponse<bool>> ImportRelatedAttributeSetDatas(string desCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RelatedData)
            {
                item.Id = Guid.NewGuid().ToString();
                item.Specificulture = desCulture;
                switch (item.ParentType)
                {
                    case MixEnums.MixAttributeSetDataType.System:
                        break;
                    case MixEnums.MixAttributeSetDataType.Set:
                        item.AttributeSetId = dicAttributeSetIds[item.AttributeSetId];
                        break;
                    case MixEnums.MixAttributeSetDataType.Post:
                        break;
                    case MixEnums.MixAttributeSetDataType.Page:
                        if (dicPageIds.TryGetValue(int.Parse(item.ParentId), out int pageId))
                        {
                            item.ParentId = pageId.ToString();
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case MixEnums.MixAttributeSetDataType.Module:
                        if (dicModuleIds.TryGetValue(int.Parse(item.ParentId), out int moduleId))
                        {
                            item.ParentId = moduleId.ToString();
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case MixEnums.MixAttributeSetDataType.Service:
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