using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixCultures;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class ImportViewModel
         : ViewModelBase<MixCmsContext, MixPost, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

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
        public MixEnums.MixContentStatus Type { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTime? PublishedDateTime { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; } = "[]";

        public string CreatedBy { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }
        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain => MixService.GetConfig<string>("Domain");

        [JsonProperty("categories")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModulePosts.ReadViewModel> Modules { get; set; } // Parent to Modules

        [JsonProperty("mediaNavs")]
        public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("postNavs")]
        public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("attributes")]
        public MixAttributeSets.ImportViewModel Attributes { get; set; }

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.UpdateViewModel AttributeData { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysTags { get; set; }

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #region Async Methods

        #region Save Sub Models Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                if (result.IsSucceed)
                {
                    // Save Alias
                    result = await SaveUrlAliasAsync(parent.Id, _context, _transaction);
                }
                
                if (result.IsSucceed && MediaNavs != null)
                {
                    // Save Medias
                    result = await SaveMediasAsync(parent.Id, _context, _transaction);
                }
                
                if (result.IsSucceed)
                {
                    // Save Attributes
                    result = await SaveAttributeAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed && PostNavs != null)
                {
                    // Save related posts
                    result = await SaveRelatedPostAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed && Pages != null)
                {
                    // Save Parent Category
                    result = await SaveParentPagesAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed && Modules != null)
                {
                    // Save Parent Modules
                    result = await SaveParentModulesAsync(parent.Id, _context, _transaction);
                }

                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
        }

        private async Task<RepositoryResponse<bool>> SaveAttributeAsync(int parentId, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            AttributeData.ParentId = parentId.ToString();
            AttributeData.ParentType = MixEnums.MixAttributeSetDataType.Post;
            var saveData = await AttributeData.Data.SaveModelAsync(true, context, transaction);
            ViewModelHelper.HandleResult(saveData, ref result);
            if (result.IsSucceed)
            {
                AttributeData.DataId = saveData.Data.Id;
                var saveRelated = await AttributeData.SaveModelAsync(true, context, transaction);
                ViewModelHelper.HandleResult(saveRelated, ref result);
            }

            foreach (var item in SysCategories)
            {
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixEnums.MixAttributeSetDataType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }

            foreach (var item in SysTags)
            {
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixEnums.MixAttributeSetDataType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.Specificulture = Specificulture;
                item.PostId = id;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentPagesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                item.Specificulture = Specificulture;
                item.PostId = id;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveRelatedPostAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navPost in PostNavs)
            {
                navPost.SourceId = id;
                navPost.Status = MixEnums.MixContentStatus.Published;
                navPost.Specificulture = Specificulture;
                if (navPost.IsActived)
                {
                    var saveResult = await navPost.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await navPost.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        //private async Task<RepositoryResponse<bool>> SaveSubModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        //{
        //    var result = new RepositoryResponse<bool>() { IsSucceed = true };
        //    foreach (var navModule in ModuleNavs)
        //    {
        //        navModule.PostId = id;
        //        navModule.Specificulture = Specificulture;
        //        navModule.Status = MixEnums.MixContentStatus.Published;
        //        if (navModule.IsActived)
        //        {
        //            var saveResult = await navModule.SaveModelAsync(false, _context, _transaction);
        //            ViewModelHelper.HandleResult(saveResult, ref result);
        //        }
        //        else
        //        {
        //            var saveResult = await navModule.RemoveModelAsync(false, _context, _transaction);
        //            ViewModelHelper.HandleResult(saveResult, ref result);
        //        }
        //    }
        //    return result;
        //}

        private async Task<RepositoryResponse<bool>> SaveMediasAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.PostId = id;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    var saveResult = await navMedia.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveUrlAliasAsync(int parentId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in UrlAliases)
            {
                item.SourceId = parentId.ToString();
                item.Type = MixEnums.UrlAliasType.Post;
                item.Specificulture = Specificulture;
                var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion Save Sub Models Async

        
        #endregion Async Methods

        #endregion Overrides

    }
}