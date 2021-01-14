﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Interfaces;
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
        : ViewModelBase<MixCmsContext, MixPost, ReadMvcViewModel>, MvcViewModel
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

        [JsonIgnore]
        [JsonProperty("extraFields")]
        public string ExtraFields { get; set; } = "[]";

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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get => Id > 0 ? $"/post/{Specificulture}/{Id}/{SeoName}" : null; }

        [JsonProperty("view")]
        public MixTemplates.ReadListItemViewModel View { get; set; }

        [JsonProperty("modules")]
        public List<ViewModels.MixModules.ReadMvcViewModel> Modules { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
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
        public string ThumbnailUrl
        {
            get
            {
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

        [JsonProperty("templatePath")]
        public string TemplatePath
        {
            get
            {
                return $"/{ MixConstants.Folder.TemplatesFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, Specificulture) ?? "Default"}/{Template}";
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
        public List<MixAttributeSets.ReadViewModel> AttributeSets { get; set; } = new List<MixAttributeSets.ReadViewModel>();

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.ReadMvcViewModel AttributeData { get; set; }

        [JsonProperty("sysTags")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysTags { get; set; } = new List<MixRelatedAttributeDatas.FormViewModel>();

        [JsonProperty("sysCategories")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysCategories { get; set; } = new List<MixRelatedAttributeDatas.FormViewModel>();

        [JsonProperty("listTag")]
        public List<string> ListTag { get => SysTags.Select(t => t.AttributeData.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("listCategory")]
        public List<string> ListCategory { get => SysCategories.Select(t => t.AttributeData.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("pages")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("Layout")]
        public string Layout { get; set; }
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
            LoadAttributes(_context, _transaction);
            LoadTags(_context, _transaction);
            LoadCategories(_context, _transaction);
            //Load Template + Style +  Scripts for views
            this.View = MixTemplates.ReadListItemViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            if (Pages == null)
            {

                LoadPages(_context, _transaction);
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

            }
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixRelatedAttributeDatas.FormViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture && m.Status == MixEnums.MixContentStatus.Published
                   && m.ParentId == Id.ToString() && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
                   && m.AttributeSetName == MixConstants.AttributeSetName.SYSTEM_TAG, context, transaction);
            if (getTags.IsSucceed)
            {
                SysTags = getTags.Data;
            }
        }

        private void LoadCategories(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = MixRelatedAttributeDatas.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                   && m.ParentId == Id.ToString() && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
                   && m.AttributeSetName == MixConstants.AttributeSetName.SYSTEM_CATEGORY, context, transaction);
            if (getData.IsSucceed)
            {
                SysCategories = getData.Data;
            }
        }

        #endregion Overrides

        #region Expands

        private void LoadPages(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            this.Pages = MixPagePosts.Helper.GetActivedNavAsync<MixPagePosts.ReadViewModel>(Id, null, Specificulture, _context, _transaction).Data;
            this.Pages.ForEach(p => p.LoadPage(_context, _transaction));
        }

        /// <summary>Loads the attributes.</summary>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            Type = string.IsNullOrEmpty(Type) ? MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST : Type;
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == Type, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AttributeData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.AttributeSetId == getAttrs.Data.Id
                    , _context, _transaction).Data ?? new MixRelatedAttributeDatas.ReadMvcViewModel();
            }
        }

        public bool HasValue(string fieldName)
        {
            return AttributeData != null && AttributeData.Data.Obj.GetValue(fieldName) != null;
        }

        /// <summary>Get Post's Property by type and name</summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>T</returns>
        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AttributeData?.Data?.Obj, fieldName);
        }

        /// <summary>Gets the module.</summary>
        /// <param name="name">The name.</param>
        /// <returns>MixModules.ReadMvcViewModel</returns>
        public MixModules.ReadMvcViewModel GetModule(string name)
        {
            return ModuleNavs.FirstOrDefault(m => m.Module.Name == name)?.Module;
        }

        /// <summary>Gets the attribute set.</summary>
        /// <param name="name">The name.</param>
        /// <returns>MixAttributeSets.ReadMvcViewModel</returns>
        public MixAttributeSets.ReadViewModel GetAttributeSet(string name)
        {
            return AttributeSets.FirstOrDefault(m => m.Name == name);
        }

        #endregion Expands
    }
}