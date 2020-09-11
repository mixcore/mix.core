using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Data.ViewModels;
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
        [JsonProperty("relatedData")]
        public MixRelatedAttributeDatas.ImportViewModel RelatedData { get; set; }
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
            GetAdditionalData(Id.ToString(), MixEnums.MixAttributeSetDataType.Post, _context, _transaction);
        }
        private void GetAdditionalData(string id, MixEnums.MixAttributeSetDataType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getRelatedData = MixRelatedAttributeDatas.ImportViewModel.Repository.GetSingleModel(
                        m => m.Specificulture == Specificulture && m.ParentType == type.ToString()
                            && m.ParentId == id, context, transaction);
            if (getRelatedData.IsSucceed)
            {
                RelatedData = (getRelatedData.Data);
            }
        }
        #region Async Methods




        #endregion Async Methods

        #endregion Overrides

    }
}