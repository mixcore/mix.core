using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPost
    {
        public MixPost()
        {
            MixArticleAttributeData = new HashSet<MixPostAttributeData>();
            MixArticleAttributeSet = new HashSet<MixPostAttributeSet>();
            MixArticleMedia = new HashSet<MixPostMedia>();
            MixArticleModule = new HashSet<MixPostModule>();
            MixComment = new HashSet<MixComment>();
            MixModuleArticle = new HashSet<MixModulePost>();
            MixModuleData = new HashSet<MixModuleData>();
            MixPageArticle = new HashSet<MixPagePost>();
            MixRelatedArticleMixArticle = new HashSet<MixRelatedArticle>();
            MixRelatedArticleS = new HashSet<MixRelatedArticle>();
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
        public virtual ICollection<MixPostAttributeData> MixArticleAttributeData { get; set; }
        public virtual ICollection<MixPostAttributeSet> MixArticleAttributeSet { get; set; }
        public virtual ICollection<MixPostMedia> MixArticleMedia { get; set; }
        public virtual ICollection<MixPostModule> MixArticleModule { get; set; }
        public virtual ICollection<MixComment> MixComment { get; set; }
        public virtual ICollection<MixModulePost> MixModuleArticle { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixPagePost> MixPageArticle { get; set; }
        public virtual ICollection<MixRelatedArticle> MixRelatedArticleMixArticle { get; set; }
        public virtual ICollection<MixRelatedArticle> MixRelatedArticleS { get; set; }
        public virtual ICollection<MixOrderItem> MixOrderItem { get; set; }
    }
}
