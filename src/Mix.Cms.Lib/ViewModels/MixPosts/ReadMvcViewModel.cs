using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class ReadMvcViewModel
        : ViewModelBase<MixCmsContext, MixPost, ReadMvcViewModel>
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

        [JsonIgnore]
        [JsonProperty("extraProperties")]
        public string ExtraProperties { get; set; } = "[]";

        [JsonProperty("icon")]
        public string Icon { get; set; }

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
        public int Type { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTime? PublishedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("view")]
        public MixTemplates.ReadListItemViewModel View { get; set; }

        [JsonProperty("modules")]
        public List<ViewModels.MixModules.ReadMvcViewModel> Modules { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
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
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        public string TemplatePath {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, Specificulture) ?? "Default"
                    , Template
                });
            }
        }

        //[JsonProperty("properties")]
        //public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("mediaNavs")]
        public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPostModules.ReadViewModel> ModuleNavs { get; set; }

        [JsonProperty("postNavs")]
        public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        [JsonProperty("attributeSets")]
        public List<MixAttributeSets.ReadMvcPostViewModel> AttributeSets { get; set; } = new List<MixAttributeSets.ReadMvcPostViewModel>();

        [JsonProperty("listTag")]
        public JArray ListTag { get => JArray.Parse(Tags ?? "[]"); }

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.ReadMvcViewModel AttributeData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //Load Template + Style +  Scripts for views
            this.View = MixTemplates.ReadListItemViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;

            //Properties = new List<ExtraProperty>();

            //if (!string.IsNullOrEmpty(ExtraProperties))
            //{
            //    JArray arr = JArray.Parse(ExtraProperties);
            //    foreach (JToken item in arr)
            //    {
            //        Properties.Add(item.ToObject<ExtraProperty>());
            //    }
            //}

            LoadAttributes(_context, _transaction);

            var getPostMedia = MixPostMedias.ReadViewModel.Repository.GetModelListBy(n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostMedia.IsSucceed)
            {
                MediaNavs = getPostMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => n.IsActived = true);
            }

            // Modules
            var getPostModule = MixPostModules.ReadViewModel.Repository.GetModelListBy(
                n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostModule.IsSucceed)
            {
                ModuleNavs = getPostModule.Data.OrderBy(p => p.Priority).ToList();
                foreach (var item in ModuleNavs)
                {
                    item.IsActived = true;
                    item.Module.LoadData(postId: Id, _context: _context, _transaction: _transaction);
                }
            }

            // Related Posts
            PostNavs = MixPostPosts.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, _context, _transaction).Data;

            // Get Attribute Sets
            var navs = MixPostAttributeSets.ReadMvcViewModel.Repository.GetModelListBy(n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction).Data;
            foreach (var item in navs)
            {
                AttributeSets.Add(item.MixAttributeSet);
            }
        }

        #endregion Overrides

        #region Expands

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AttributeData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.AttributeSetId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        public T Property<T>(string fieldName)
        {
            if (AttributeData != null)
            {
                var field = AttributeData.Data.Data.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        public MixModules.ReadMvcViewModel GetModule(string name)
        {
            return ModuleNavs.FirstOrDefault(m => m.Module.Name == name)?.Module;
        }

        public MixAttributeSets.ReadMvcPostViewModel GetAttributeSet(string name)
        {
            return AttributeSets.FirstOrDefault(m => m.Name == name);
        }

        #endregion Expands
    }
}