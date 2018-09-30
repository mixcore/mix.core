using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixProduct
    {
        public MixProduct()
        {
            MixComment = new HashSet<MixComment>();
            MixModuleProduct = new HashSet<MixModuleProduct>();
            MixOrderItem = new HashSet<MixOrderItem>();
            MixPageProduct = new HashSet<MixPageProduct>();
            MixProductMedia = new HashSet<MixProductMedia>();
            MixProductModule = new HashSet<MixProductModule>();
            MixRelatedProductMixProduct = new HashSet<MixRelatedProduct>();
            MixRelatedProductS = new HashSet<MixRelatedProduct>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int? SetAttributeId { get; set; }
        public string SetAttributeData { get; set; }
        public string Content { get; set; }
        public string Unit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Excerpt { get; set; }
        public string ExtraProperties { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public double Price { get; set; }
        public string PrivacyId { get; set; }
        public int Priority { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public string Source { get; set; }
        public int Status { get; set; }
        public string Tags { get; set; }
        public string Template { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int? Views { get; set; }
        public string Code { get; set; }
        public double? DealPrice { get; set; }
        public double Discount { get; set; }
        public double ImportPrice { get; set; }
        public string Material { get; set; }
        public double NormalPrice { get; set; }
        public int PackageCount { get; set; }
        public int TotalSaled { get; set; }

        public MixSetAttribute SetAttribute { get; set; }
        public MixCulture SpecificultureNavigation { get; set; }
        public ICollection<MixComment> MixComment { get; set; }
        public ICollection<MixModuleProduct> MixModuleProduct { get; set; }
        public ICollection<MixOrderItem> MixOrderItem { get; set; }
        public ICollection<MixPageProduct> MixPageProduct { get; set; }
        public ICollection<MixProductMedia> MixProductMedia { get; set; }
        public ICollection<MixProductModule> MixProductModule { get; set; }
        public ICollection<MixRelatedProduct> MixRelatedProductMixProduct { get; set; }
        public ICollection<MixRelatedProduct> MixRelatedProductS { get; set; }
    }
}
