using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPost
    {
        public MixPost()
        {
            MixModuleData = new HashSet<MixModuleData>();
            MixModulePost = new HashSet<MixModulePost>();
            MixOrderItem = new HashSet<MixOrderItem>();
            MixPagePost = new HashSet<MixPagePost>();
            MixPostAttributeData = new HashSet<MixPostAttributeData>();
            MixPostAttributeSet = new HashSet<MixPostAttributeSet>();
            MixPostMedia = new HashSet<MixPostMedia>();
            MixPostModule = new HashSet<MixPostModule>();
            MixRelatedPostMixPost = new HashSet<MixRelatedPost>();
            MixRelatedPostS = new HashSet<MixRelatedPost>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Excerpt { get; set; }
        public string ExtraProperties { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
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
        public string ExtraFields { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixModulePost> MixModulePost { get; set; }
        public virtual ICollection<MixOrderItem> MixOrderItem { get; set; }
        public virtual ICollection<MixPagePost> MixPagePost { get; set; }
        public virtual ICollection<MixPostAttributeData> MixPostAttributeData { get; set; }
        public virtual ICollection<MixPostAttributeSet> MixPostAttributeSet { get; set; }
        public virtual ICollection<MixPostMedia> MixPostMedia { get; set; }
        public virtual ICollection<MixPostModule> MixPostModule { get; set; }
        public virtual ICollection<MixRelatedPost> MixRelatedPostMixPost { get; set; }
        public virtual ICollection<MixRelatedPost> MixRelatedPostS { get; set; }
    }
}