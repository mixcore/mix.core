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
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class ReadListItemViewModel
        : ViewModelBase<MixCmsContext, MixPost, ReadListItemViewModel>
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
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("view")]
        public ReadViewModel View { get; set; }

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

        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("listTag")]
        public JArray ListTag { get => JArray.Parse(Tags ?? "[]"); }

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
            Properties = new List<ExtraProperty>();

            if (!string.IsNullOrEmpty(ExtraProperties))
            {
                JArray arr = JArray.Parse(ExtraProperties);
                foreach (JToken item in arr)
                {
                    Properties.Add(item.ToObject<ExtraProperty>());
                }
            }
        }

        #endregion Overrides

        #region Expands

        //Get Property by name
        public string Property(string name)
        {
            var prop = Properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            return prop?.Value;
        }

        #endregion Expands
    }
}