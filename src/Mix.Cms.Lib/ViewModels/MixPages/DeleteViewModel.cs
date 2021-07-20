using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class DeleteViewModel
       : ViewModelBase<MixCmsContext, MixPage, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

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
        public MixContentStatus Status { get; set; }

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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPageModules.ReadMvcViewModel> ModuleNavs { get; set; } // Parent to Modules

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl {
            get {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return $"{Domain}/{Thumbnail}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }

        [JsonProperty("master")]
        public MixTemplates.UpdateViewModel Master { get; set; }

        [JsonProperty("masters")]
        public List<MixTemplates.UpdateViewModel> Masters { get; set; }

        [JsonIgnore]
        public int ActivedTheme {
            get {
                return MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string TemplateFolderType {
            get {
                return MixTemplateFolders.Pages;
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return $"{MixFolders.TemplatesFolder}/" +
                   $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeName, Specificulture)}/" +
                   $"{MixTemplateFolders.Pages}";
            }
        }

        #endregion Template

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("attributes")]
        public MixDatabases.UpdateViewModel Attributes { get; set; }

        [JsonProperty("attributeData")]
        public MixDatabaseDataAssociations.UpdateViewModel AttributeData { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.UpdateViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.UpdateViewModel> SysTags { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #region Async

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };
            var navPosts = _context.MixPagePost.Where(m => m.PageId == Id && m.Specificulture == Specificulture);
            await navPosts.ForEachAsync(m => _context.Entry(m).State = EntityState.Deleted);
            var navModls = _context.MixPageModule.Where(m => m.PageId == Id && m.Specificulture == Specificulture);
            await navModls.ForEachAsync(m => _context.Entry(m).State = EntityState.Deleted);
            await _context.SaveChangesAsync();
            var removeRelatedData = await MixDatabaseDataAssociations.Helper.RemoveRelatedDataAsync(
                    Id.ToString(), MixDatabaseParentType.Page
                    , Specificulture
                    , _context, _transaction);
            ViewModelHelper.HandleResult(removeRelatedData, ref result);
            return result;
        }

        #endregion Async

        #endregion Overrides
    }
}