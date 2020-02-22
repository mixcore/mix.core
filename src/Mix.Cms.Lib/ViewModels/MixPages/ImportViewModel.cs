using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class ImportViewModel
       : ViewModelBase<MixCmsContext, MixPage, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("seoTitle")]
        public string SeoTitle { get; set; }

        [JsonProperty("seoDescription")]
        public string SeoDescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SeoKeywords { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("views")]
        public int? Views { get; set; }

        [JsonProperty("type")]
        public MixPageType Type { get; set; }

        [JsonProperty("status")]
        public MixEnums.PageStatus Status { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("updatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("moduleNavs")]
        public List<MixPageModules.ImportViewModel> ModuleNavs { get; set; } // Parent to Modules

        //[JsonProperty("parentNavs")]
        //public List<MixPagePages.ReadViewModel> ParentNavs { get; set; } // Parent to  Parent

        //[JsonProperty("childNavs")]
        //public List<MixPagePages.ReadViewModel> ChildNavs { get; set; } // Parent to  Parent

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("isExportData")]
        public bool IsExportData { get; set; }

        [JsonProperty("themeName")]
        public string ThemeName { get; set; } = "default";

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //Cultures = Helper.LoadCultures(Id, Specificulture, _context, _transaction);
            //this.ModuleNavs = GetModuleNavs(_context, _transaction);
            ////this.ParentNavs = GetParentNavs(_context, _transaction);
            ////this.ChildNavs = GetChildNavs(_context, _transaction);
            //this.UrlAliases = GetAliases(_context, _transaction);
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPage parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            // Save Alias
            //foreach (var item in UrlAliases)
            //{
            //    item.SourceId = parent.Id.ToString();
            //    item.Type = UrlAliasType.Page;
            //    item.Specificulture = parent.Specificulture;
            //    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
            //    ViewModelHelper.HandleResult(saveResult, ref result);
            //    if (!result.IsSucceed)
            //    {
            //        break;
            //    }
            //}
            // End Save Alias

            //Save Module Navigations
            if (result.IsSucceed && ModuleNavs != null)
            {
                foreach (var item in ModuleNavs)
                {
                    if (!MixModules.ImportViewModel.Repository.CheckIsExists(m => m.Name == item.Module.Name && m.Specificulture == parent.Specificulture,
                            _context, _transaction))
                    {
                        //  Force to create new module
                        item.Module.Id = 0;
                        item.Module.Specificulture = parent.Specificulture;
                        if (!string.IsNullOrEmpty(item.Image))
                        {
                            item.Image = item.Image.Replace($"content/templates/{ThemeName}", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", parent.Specificulture)}");
                        }
                        if (!string.IsNullOrEmpty(item.Module.Image))
                        {
                            item.Module.Image = item.Module.Image.Replace($"content/templates/{ThemeName}", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", parent.Specificulture)}");
                        }
                        if (!string.IsNullOrEmpty(item.Module.Thumbnail))
                        {
                            item.Module.Thumbnail = item.Module.Thumbnail.Replace("content/templates/default", $"content/templates/{MixService.GetConfig<string>("ThemeFolder", parent.Specificulture)}");
                        }
                        var saveModule = await item.Module.SaveModelAsync(true, _context, _transaction);
                        ViewModelHelper.HandleResult(saveModule, ref result);
                        if (!result.IsSucceed)
                        {
                            break;
                        }
                        else // Save Module Success
                        {
                            item.PageId = parent.Id;
                            item.ModuleId = saveModule.Data.Id;
                            item.Specificulture = parent.Specificulture;
                            item.Description = saveModule.Data.Title;
                            var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                            if (!result.IsSucceed)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            // End Save Module Navigations
            /*
            // Save Parents Pages
            if (result.IsSucceed)
            {
                foreach (var item in ParentNavs)
                {
                    item.Id = parent.Id;

                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            // End Save Parents Pages

            // Save Children Pages
            if (result.IsSucceed)
            {
                foreach (var item in ChildNavs)
                {
                    item.ParentId = parent.Id;
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            // End Save Children Pages*/
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expands

        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixEnums.UrlAliasType.Page, context, transaction);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliases.UpdateViewModel>();
            }
        }

        public List<MixPageModules.ImportViewModel> GetModuleNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixPageModules.ImportViewModel.Repository.GetModelListBy(
                module => module.Specificulture == Specificulture && module.PageId == Id,
                context, transaction).Data;
        }

        public List<MixPagePages.ReadViewModel> GetParentNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixPagePages.ReadViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture && p.Id == Id).Data;
        }

        public List<MixPagePages.ReadViewModel> GetChildNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixPagePages.ReadViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture && p.ParentId == Id).Data;
        }

        #endregion Expands
    }
}