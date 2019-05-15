using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixArticle
    {
        public MixArticle()
        {
            MixArticleAttributeData = new HashSet<MixArticleAttributeData>();
            MixArticleMedia = new HashSet<MixArticleMedia>();
            MixArticleModule = new HashSet<MixArticleModule>();
            MixComment = new HashSet<MixComment>();
            MixModuleArticle = new HashSet<MixModuleArticle>();
            MixPageArticle = new HashSet<MixPageArticle>();
            MixRelatedArticleMixArticle = new HashSet<MixRelatedArticle>();
            MixRelatedArticleS = new HashSet<MixRelatedArticle>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int? SetAttributeId { get; set; }
        public string SetAttributeData { get; set; }
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

        public virtual MixAttributeSet SetAttribute { get; set; }
        public virtual MixSetAttribute SetAttributeNavigation { get; set; }
        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixArticleAttributeData> MixArticleAttributeData { get; set; }
        public virtual ICollection<MixArticleMedia> MixArticleMedia { get; set; }
        public virtual ICollection<MixArticleModule> MixArticleModule { get; set; }
        public virtual ICollection<MixComment> MixComment { get; set; }
        public virtual ICollection<MixModuleArticle> MixModuleArticle { get; set; }
        public virtual ICollection<MixPageArticle> MixPageArticle { get; set; }
        public virtual ICollection<MixRelatedArticle> MixRelatedArticleMixArticle { get; set; }
        public virtual ICollection<MixRelatedArticle> MixRelatedArticleS { get; set; }
    }
}
