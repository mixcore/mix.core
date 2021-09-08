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
        public List<SupportedCulture> Cultures { get; set; }

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
        public string Type { get; set; }

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
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain => MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain);

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
        public MixDatabases.ImportViewModel Attributes { get; set; }

        [JsonProperty("additionalData")]
        public MixDatabaseDataAssociations.UpdateViewModel AdditionalData { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysTags { get; set; }

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("relatedData")]
        public MixDatabaseDataAssociations.ImportViewModel RelatedData { get; set; }

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

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            GetAdditionalData(Id.ToString(), MixDatabaseParentType.Post, _context, _transaction);
        }

        private void GetAdditionalData(string id, MixDatabaseParentType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getRelatedData = MixDatabaseDataAssociations.ImportViewModel.Repository.GetFirstModel(
                        m => m.Specificulture == Specificulture && m.ParentType == type
                            && m.ParentId == id, context, transaction);
            if (getRelatedData.IsSucceed)
            {
                RelatedData = (getRelatedData.Data);
            }
        }

        #endregion Overrides
    }
}