using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class ReadListItemViewModel
       : ViewModelBase<MixCmsContext, MixPage, ReadListItemViewModel>
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

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

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
        public MixEnums.PageStatus Status { get; set; }

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

        [JsonProperty("childs")]
        public List<MixPagePages.ReadViewModel> Childs { get; set; }

        [JsonProperty("totalPost")]
        public int TotalPost { get; set; }

        [JsonProperty("totalProduct")]
        public int TotalProduct { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadListItemViewModel() : base()
        {
        }

        public ReadListItemViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getChilds = MixPagePages.ReadViewModel.Repository.GetModelListBy(
                p => p.ParentId == Id && p.Specificulture == Specificulture, _context, _transaction);
            if (getChilds.IsSucceed)
            {
                Childs = getChilds.Data.OrderBy(p => p.Priority).ToList();
            }
            var countPost = MixPagePosts.ReadViewModel.Repository.Count(c => c.PageId == Id && c.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction);

            if (countPost.IsSucceed)
            {
                TotalPost = countPost.Data;
            }
        }

        #endregion Overrides
    }
}