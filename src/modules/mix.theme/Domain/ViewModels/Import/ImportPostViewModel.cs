using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mix.Lib.ViewModels.Cms;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportPostViewModel
         : ViewModelBase<MixCmsContext, MixPost, ImportPostViewModel>
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

        public List<MixPagePost> Pages { get; set; }

        public List<MixModulePost> Modules { get; set; } // Parent to Modules

        public List<MixPostMedia> MediaNavs { get; set; }

        public List<MixPostAssociation> PostNavs { get; set; }

        public JArray ListTag { get; set; } = new JArray();

        public ImportMixDatabaseViewModel Attributes { get; set; }
        
        public ImportMixDataAssociationViewModel AttributeData { get; set; }

        public List<ImportMixDataAssociationViewModel> SysCategories { get; set; }

        public List<ImportMixDataAssociationViewModel> SysTags { get; set; }

        public List<UrlAliasViewModel> UrlAliases { get; set; }

        public ImportMixDataAssociationViewModel RelatedData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportPostViewModel() : base()
        {
        }

        public ImportPostViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
            var getRelatedData = ImportMixDataAssociationViewModel.Repository.GetFirstModel(
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