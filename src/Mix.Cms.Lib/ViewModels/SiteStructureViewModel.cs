using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
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

        public async Task<RepositoryResponse<bool>> ImportAsync(string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                if (Pages != null)
                {
                    result = await ImportPagesAsync(Pages, destCulture, context, transaction);
                }
                if (result.IsSucceed && Modules != null)
                {
                    result = await ImportModulesAsync(Modules, destCulture, context, transaction);
                }
                if (result.IsSucceed && AttributeSets!=null)
                {
                    result = await ImportAttributeSetsAsync(AttributeSets, context, transaction);
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

        private async Task<RepositoryResponse<bool>> ImportModulesAsync(List<MixModules.ImportViewModel> modules, string destCulture, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var module in modules)
            {
                if (result.IsSucceed)
                {
                    if (!context.MixModule.Any(m => m.Name == module.Name && m.Specificulture == destCulture))
                    {
                        module.Id = context.MixModule.Max(m => m.Id) + 1;
                        module.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await module.SaveModelAsync(true, context, transaction);
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


        private async Task<RepositoryResponse<bool>> ImportAttributeSetsAsync(List<MixAttributeSets.ImportViewModel> attributeSets, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (attributeSets != null)
            {
                var startId = MixAttributeSets.ImportViewModel.Repository.Max(m => m.Id).Data;
                foreach (var set in attributeSets)
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

        private async Task<RepositoryResponse<bool>> ImportPagesAsync(List<MixPages.ImportViewModel> arrPage, string destCulture,
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
                foreach (var item in arrPage)
                {
                    item.Id = startId;
                    item.CreatedDateTime = DateTime.UtcNow;
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

    }
}
