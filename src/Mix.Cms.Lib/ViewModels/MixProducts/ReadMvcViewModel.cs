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

namespace Mix.Cms.Lib.ViewModels.MixProducts
{
    public class ReadMvcViewModel
        : ViewModelBase<MixCmsContext, MixProduct, ReadMvcViewModel>
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

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("priceUnit")]
        public string PriceUnit { get; set; }

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

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("totalSaled")]
        public int TotalSaled { get; set; }

        [JsonProperty("dealPrice")]
        public double? DealPrice { get; set; }

        [JsonProperty("discount")]
        public double Discount { get; set; }

        [JsonProperty("importPrice")]
        public double ImportPrice { get; set; }

        [JsonProperty("material")]
        public string Material { get; set; }

        [JsonProperty("normalPrice")]
        public double NormalPrice { get; set; }

        [JsonProperty("packageCount")]
        public int PackageCount { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("view")]
        public MixTemplates.ReadViewModel View { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain") ?? "/"; } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (Image != null && (Image.IndexOf("http") == -1 && Image[0] != '/'))
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
                if (!string.IsNullOrEmpty(Thumbnail))
                {
                    if (Thumbnail.IndexOf("http") == -1)
                    {
                        return CommonHelper.GetFullPath(new string[] {
                            Domain,  Thumbnail

                        });
                    }
                    else
                    {
                        return Thumbnail;
                    }

                }
                else
                {
                    return ImageUrl;
                }
            }
        }

        public string TemplatePath
        {
            get
            {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture) ?? "Default"
                    , Template
                });
            }
        }

        [JsonProperty("properties")]
        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("mediaNavs")]
        public List<MixProductMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("productNavs")]
        public List<MixProductProducts.ReadViewModel> ProductNavs { get; set; }


        [JsonProperty("strPrice")]
        public string StrPrice
        {
            get
            {
                return MixCmsHelper.FormatPrice(Price);
            }
        }

        [JsonProperty("strNormalPrice")]
        public string StrNormalPrice
        {
            get
            {
                return MixCmsHelper.FormatPrice(NormalPrice);
            }
        }

        [JsonProperty("strDealPrice")]
        public string StrDealPrice
        {
            get
            {
                return MixCmsHelper.FormatPrice(DealPrice);
            }
        }

        [JsonProperty("strImportPrice")]
        public string StrImportPrice
        {
            get
            {
                return MixCmsHelper.FormatPrice(ImportPrice);
            }
        }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixProduct model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.View = MixTemplates.ReadViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
           
            Properties = new List<ExtraProperty>();
            if (!string.IsNullOrEmpty(ExtraProperties))
            {
                JArray arr = JArray.Parse(ExtraProperties);
                foreach (JToken item in arr)
                {
                    Properties.Add(item.ToObject<ExtraProperty>());
                }
            }
            
            var getProductMedia = MixProductMedias.ReadViewModel.Repository.GetModelListBy(n => n.ProductId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getProductMedia.IsSucceed)
            {
                MediaNavs = getProductMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => n.IsActived = true);
            }

            var getRelatedProduct = MixProductProducts.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getRelatedProduct.IsSucceed)
            {
                ProductNavs = getRelatedProduct.Data.OrderBy(p => p.Priority).ToList();
                ProductNavs.ForEach(n => n.IsActived = true);
            }
        }

        #endregion Overrides

        #region Expands

        private void GenerateSEO()
        {
            if (string.IsNullOrEmpty(this.SeoName))
            {
                this.SeoName = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoTitle))
            {
                this.SeoTitle = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoDescription))
            {
                this.SeoDescription = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoKeywords))
            {
                this.SeoKeywords = SeoHelper.GetSEOString(this.Title);
            }
        }
        public string GetProperty(string name)
        {
            return Properties.Find(p => p.Name == name)?.Value;
        }
        #endregion Expands
    }
}
