using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib;
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
using System.Linq;
using MixLibViewModels = Mix.Cms.Lib.ViewModels;

namespace Mix.Rest.Api.Client.ViewModels
{
    public class PostViewModel
        : ViewModelBase<MixCmsContext, MixPost, PostViewModel>
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
        public string DetailsUrl { get => Id > 0 ? $"/{Specificulture}/post/{Id}/{SeoName}" : null; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
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
                return $"/{ MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/{Template}";
            }
        }

        [JsonProperty("postNavs")]
        public List<MixLibViewModels.MixPosts.ReadListItemViewModel> PostNavs { get; set; }

        [JsonProperty("additionalData")]
        public JObject AdditionalData { get; set; }

        [JsonProperty("tags")]
        public List<JObject> Tags { get; set; } = new List<JObject>();

        [JsonProperty("categories")]
        public List<JObject> Categories { get; set; } = new List<JObject>();

        [JsonProperty("pages")]
        public List<PageViewModel> Pages { get; set; }

        [JsonProperty("Layout")]
        public string Layout { get; set; }

        [JsonProperty("author")]
        public JObject Author { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public PostViewModel() : base()
        {
        }

        public PostViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
            if (Pages == null)
            {
                LoadPages(_context, _transaction);
                // Related Posts
                PostNavs = MixLibViewModels.MixPostPosts.ReadViewModel.Repository.GetModelListBy(
                    n => n.SourceId == Id && n.Specificulture == Specificulture, _context, _transaction)
                    .Data.Select(m=>m.RelatedPost).ToList();
            }
        }

        private void LoadAuthor(MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!string.IsNullOrEmpty(CreatedBy))
            {
                var getAuthor = MixLibViewModels.MixDatabaseDatas.Helper.LoadAdditionalData(MixDatabaseParentType.User, CreatedBy, MixDatabaseNames.SYSTEM_USER_DATA
                    , Specificulture, context, transaction);
                if (getAuthor.IsSucceed)
                {
                    Author = getAuthor.Data.Obj;
                }
            }
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixLibViewModels.MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture && m.Status == MixContentStatus.Published
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_TAG, context, transaction);
            if (getTags.IsSucceed)
            {
                Tags = getTags.Data.Select(m => m.AttributeData.Obj).ToList();
            }
        }

        private void LoadCategories(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = MixLibViewModels.MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(
                m => m.Specificulture == Specificulture
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_CATEGORY, context, transaction);
            if (getData.IsSucceed)
            {
                Categories = getData.Data.Select(m => m.AttributeData.Obj).ToList();
            }
        }

        #endregion Overrides

        #region Expands

        private void LoadPages(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var pageIds = _context.MixPagePost.Where(m => m.Specificulture == Specificulture
                && m.PostId == Id).Select(m => m.PageId);
            Pages = PageViewModel.Repository.GetModelListBy(m =>
                m.Specificulture == Specificulture 
                && pageIds.Any(n => n == m.Id)
                , _context, _transaction
                ).Data;
        }

        /// <summary>Loads the attributes.</summary>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            Type = string.IsNullOrEmpty(Type) ? MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST : Type;
            var dataId = _context.MixDatabaseDataAssociation.Where(
                a => a.ParentId == Id.ToString()
                    && a.Specificulture == Specificulture
                    && a.MixDatabaseName == Type)
                .Select(m => m.DataId)
                .FirstOrDefault();

            AdditionalData = MixLibViewModels.MixDatabaseDatas.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.Id == dataId && a.Specificulture == Specificulture
                    , _context, _transaction).Data?.Obj;
        }

        #endregion Expands
    }
}