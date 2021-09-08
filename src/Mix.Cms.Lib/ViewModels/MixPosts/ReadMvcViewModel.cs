using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Interfaces;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class ReadMvcViewModel
        : ViewModelBase<MixCmsContext, MixPost, ReadMvcViewModel>, IMvcViewModel
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
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("view")]
        public MixTemplates.ReadListItemViewModel View { get; set; }

        [JsonProperty("modules")]
        public List<ViewModels.MixModules.ReadMvcViewModel> Modules { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1))
                {
                    return $"{Domain.TrimEnd('/')}/{Image.TrimStart('/')}";
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
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1)
                {
                    return $"{Domain.TrimEnd('/')}/{Thumbnail.TrimStart('/')}";
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
                return $"/{ MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/{Template}";
            }
        }

        [JsonProperty("mediaNavs")]
        public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPostModules.ReadViewModel> ModuleNavs { get; set; }

        [JsonProperty("postNavs")]
        public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        [JsonProperty("Databases")]
        public List<MixDatabases.ReadViewModel> Databases { get; set; } = new List<MixDatabases.ReadViewModel>();

        [JsonProperty("additionalData")]
        public MixDatabaseDataAssociations.ReadMvcViewModel AdditionalData { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysTags { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysCategories { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        [JsonProperty("listTag")]
        public List<string> ListTag { get => SysTags.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("listCategory")]
        public List<string> ListCategory { get => SysCategories.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("pages")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("Layout")]
        public string Layout { get; set; }

        [JsonProperty("author")]
        public JObject Author { get; set; }

        [JsonProperty("bodyClass")]
        public string BodyClass => Property<string>("body_class");

        [JsonProperty("aliases")]
        public List<MixUrlAliases.UpdateViewModel> Aliases { get; set; }
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
            LoadAuthor(_context, _transaction);
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

            LoadAliased(_context, _transaction);
            DetailsUrl = Aliases.Count > 0
               ? MixCmsHelper.GetDetailsUrl(Specificulture, $"/{Aliases[0].Alias}")
               : Id > 0
                   ? MixCmsHelper.GetDetailsUrl(Specificulture, $"/{MixService.GetConfig("PostController", Specificulture, "post")}/{Id}/{SeoName}")
                   : null;
        }

        private void LoadAliased(MixCmsContext context, IDbContextTransaction transaction)
        {
            Aliases = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(
                m => m.Type == (int)MixUrlAliasType.Post && m.SourceId == Id.ToString(),
                context, transaction).Data;
        }


        private void LoadAuthor(MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!string.IsNullOrEmpty(CreatedBy))
            {
                var getAuthor = MixDatabaseDatas.Helper.LoadAdditionalData(MixDatabaseParentType.User, CreatedBy, MixDatabaseNames.SYSTEM_USER_DATA
                    , Specificulture, context, transaction);
                if (getAuthor.IsSucceed)
                {
                    Author = getAuthor.Data.Obj;
                }
            }
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture && m.Status == MixContentStatus.Published
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_TAG, context, transaction);
            if (getTags.IsSucceed)
            {
                SysTags = getTags.Data;
            }
        }

        private void LoadCategories(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_CATEGORY, context, transaction);
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
            Type = string.IsNullOrEmpty(Type) ? MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST : Type;
            var getAttrs = MixDatabases.UpdateViewModel.Repository.GetSingleModel(m => m.Name == Type, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AdditionalData = MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.MixDatabaseId == getAttrs.Data.Id
                    , _context, _transaction).Data ?? new MixDatabaseDataAssociations.ReadMvcViewModel();
            }
        }

        public bool HasValue(string fieldName)
        {
            return AdditionalData != null && AdditionalData.Data.Obj.GetValue(fieldName) != null;
        }

        /// <summary>Get Post's Property by type and name</summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>T</returns>
        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AdditionalData?.Data?.Obj, fieldName);
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
        /// <returns>Databases.ReadMvcViewModel</returns>
        public MixDatabases.ReadViewModel GetMixDatabase(string name)
        {
            return Databases.FirstOrDefault(m => m.Name == name);
        }

        #endregion Expands
    }
}