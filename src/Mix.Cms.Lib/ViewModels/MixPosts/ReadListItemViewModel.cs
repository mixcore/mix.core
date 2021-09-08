using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Heart.NetCore.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    [GeneratedController("api/v1/rest/{culture}/mix-post/list-item")]
    public class ReadListItemViewModel
        : ViewModelBase<MixCmsContext, MixPost, ReadListItemViewModel>
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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

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

        [JsonProperty("pages")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("author")]
        public JObject Author { get; set; }

        [JsonProperty("aliases")]
        public List<MixUrlAliases.UpdateViewModel> Aliases { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadListItemViewModel() : base()
        {
        }

        public ReadListItemViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            LoadAttributes(_context, _transaction);
            LoadPages(_context, _transaction);
            LoadTags(_context, _transaction);
            LoadCategories(_context, _transaction);
            LoadAuthor(_context, _transaction);
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

        private void LoadPages(MixCmsContext context, IDbContextTransaction transaction)
        {
            this.Pages = MixPagePosts.Helper.GetActivedNavAsync<MixPagePosts.ReadViewModel>(Id, null, Specificulture, context, transaction).Data;
            this.Pages.ForEach(p => p.LoadPage(context, transaction));
        }

        #endregion Overrides

        #region Expands

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

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            string type = Type ?? MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST;
            var getAttrs = MixDatabases.UpdateViewModel.Repository.GetSingleModel(
                m => m.Name == type, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AdditionalData = MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture
                    && a.MixDatabaseId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
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

        //Get Property by name
        public T Property<T>(string fieldName)
        {
            if (AdditionalData != null)
            {
                var field = AdditionalData.Data.Obj?.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        #endregion Expands
    }
}