using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixModules;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public List<MixConfigurations.ReadViewModel> Configurations { get; set; }

        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.ReadViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.ReadViewModel>();

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

        public RepositoryResponse<string> ProcessSelectedExportDataAsync()
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            try
            {
                //Configurations = MixConfigurations.ReadViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture, context, transaction).Data;
                ProcessPages(context, transaction);
                ProcessModules(context, transaction);
                ProcessAttributeSetsAsync(context, transaction);
                ProcessAttributeSetData(context, transaction);
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
                    context?.Dispose();
                }
            }
        }

        private void ProcessAttributeSetsAsync(MixCmsContext context, IDbContextTransaction transaction)
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

        private void ProcessModules(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in Modules)
            {
                if (item.IsExportData)
                {
                    ProcessModuleData(item, context, transaction);
                }
            }
        }

        private void ProcessPages(MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var item in Pages)
            {
                if (item.IsExportData)
                {
                    item.ModuleNavs = item.GetModuleNavs(context, transaction);
                    foreach (var nav in item.ModuleNavs)
                    {
                        var dupModule = Modules.FirstOrDefault(m => m.Id == nav.ModuleId && m.Specificulture == Specificulture);
                        if (dupModule != null)
                        {
                            Modules.Remove(dupModule);
                        }
                        else
                        {
                            nav.Module.IsExportData = true;
                        }
                        ProcessModuleData(nav.Module, context, transaction);
                    }
                    item.UrlAliases = item.GetAliases(context, transaction);
                    GetAdditionalData(item.Id.ToString(), MixEnums.MixAttributeSetDataType.Page, context, transaction);
                    //this.ParentNavs = GetParentNavs(_context, _transaction);
                    //this.ChildNavs = GetChildNavs(_context, _transaction);
                }
            }
        }

        private void ProcessModuleData(ImportViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getDataResult = MixModuleDatas.ReadViewModel.Repository
                               .GetModelListBy(m => m.ModuleId == item.Id && m.Specificulture == item.Specificulture
                               , "Priority", 0, null, null
                               , context, transaction);

            if (getDataResult.IsSucceed)
            {
                getDataResult.Data.JsonItems = new List<JObject>();
                getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                item.Data = getDataResult.Data;
            }

            GetAdditionalData(item.Id.ToString(), MixEnums.MixAttributeSetDataType.Module, context, transaction);
        }

        private void GetAdditionalData(string id, MixEnums.MixAttributeSetDataType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!RelatedData.Any(m => m.ParentId == id && m.ParentType == (int)type))
            {
                var getRelatedData = MixRelatedAttributeDatas.ReadViewModel.Repository.GetSingleModel(
                            m => m.Specificulture == Specificulture && m.ParentType == (int)type
                                && m.ParentId == id, context, transaction);
                if (getRelatedData.IsSucceed)
                {
                    RelatedData.Add(getRelatedData.Data);
                }
            }
        }

        private void ProcessAttributeSetData(MixCmsContext context, IDbContextTransaction transaction)
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

        #endregion Export

        #region Import

        public async Task<RepositoryResponse<bool>> ImportAsync(string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                if (Pages != null)
                {
                    result = await ImportPagesAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && Modules != null)
                {
                    result = await ImportModulesAsync(destCulture, context, transaction);
                }
                if (result.IsSucceed && AttributeSets != null)
                {
                    result = await ImportAttributeSetsAsync(context, transaction);
                }
                if (result.IsSucceed && AttributeSetDatas.Count > 0)
                {
                    result = await ImportAttributeSetDatas(context, transaction);
                }
                if (result.IsSucceed && RelatedData.Count > 0)
                {
                    result = await ImportRelatedDatas(context, transaction);
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
                    context?.Dispose();
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportModulesAsync(string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var module in Modules)
            {
                var oldId = module.Id;
                if (result.IsSucceed)
                {
                    if (!context.MixModule.Any(m => m.Name == module.Name && m.Specificulture == destCulture))
                    {
                        module.Id = context.MixModule.Max(m => m.Id) + 1;
                        if (!string.IsNullOrEmpty(module.Image))
                        {
                            module.Image = module.Image.Replace($"content/templates/{ThemeName}", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", destCulture)}");
                        }
                        module.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await module.SaveModelAsync(true, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    // update new id to related attribute data
                    var related = RelatedData.Where(

                    m => m.ParentType == (int)MixEnums.MixAttributeSetDataType.Module && m.ParentId == oldId.ToString());
                    foreach (var r in related)
                    {
                        r.ParentId = module.Id.ToString();
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
                var startId = MixAttributeSets.ImportViewModel.Repository.Max(m => m.Id).Data;
                foreach (var set in AttributeSets)
                {
                    if (result.IsSucceed)
                    {
                        startId++;
                        if (!context.MixAttributeSet.Any(m => m.Name == set.Name))
                        {
                            set.Id = startId;
                            set.CreatedDateTime = DateTime.UtcNow;
                            var saveResult = await set.SaveModelAsync(true, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                    }
                    else
                    {
                        break;
                    }
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
                int startId = MixPages.UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data + 1;
                //var pages = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_PAGES, "data", true, "{}");
                //var obj = JObject.Parse(pages.Content);
                //var initPages = obj["data"].ToObject<JArray>();
                foreach (var item in Pages)
                {
                    // store old id => update to related data if save success
                    var oldId = item.Id;

                    item.Id = startId;
                    item.CreatedDateTime = DateTime.UtcNow;
                    item.ThemeName = ThemeName;

                    //if (_context.MixPage.Any(m=>m.Id == startId)) //(item.Id > initPages.Count)
                    //{
                    //    item.Id = _context.MixPage.Max(m => m.Id) + 1;
                    //    item.CreatedDateTime = DateTime.UtcNow;
                    //}
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        item.Image = item.Image.Replace($"content/templates/{ThemeName}", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", destCulture)}");
                    }
                    if (!string.IsNullOrEmpty(item.Thumbnail))
                    {
                        item.Thumbnail = item.Thumbnail.Replace($"content/templates/{ThemeName}", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", destCulture)}");
                    }
                    item.Specificulture = destCulture;
                    var saveResult = await item.SaveModelAsync(true, context, transaction);
                    if (!saveResult.IsSucceed)
                    {
                        result.IsSucceed = false;
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }
                    else
                    {
                        // update new id to related attribute data
                        var related = RelatedData.Where(
                        m => m.ParentType == (int)MixEnums.MixAttributeSetDataType.Page && m.ParentId == oldId.ToString());
                        foreach (var r in related)
                        {
                            r.ParentId = item.Id.ToString();
                        }
                        startId++;
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
                    context?.Dispose();
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportAttributeSetDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in AttributeSetDatas)
            {
                if (result.IsSucceed)
                {
                    if (!context.MixAttributeSetData.Any(m => m.Id == item.Id && m.Specificulture == item.Specificulture))
                    {
                        item.Fields = item.Fields ?? MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(
                            m => m.AttributeSetId == item.AttributeSetId, context, transaction).Data;
                        foreach (var field in item.Fields)
                        {
                            var newSet = AttributeSets.FirstOrDefault(m => m.Name == field.AttributeSetName);
                            var newField = newSet?.Fields.FirstOrDefault(m => m.Name == field.Name);
                            if (newField != null)
                            {
                                field.Id = newField.Id;
                                field.AttributeSetId = newSet.Id;
                                field.AttributeSetName = newSet.Name;
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

        private async Task<RepositoryResponse<bool>> ImportRelatedDatas(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RelatedData)
            {
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