using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPost : AuditedEntity
    {
        public MixPost()
        {
            MixModuleData = new HashSet<MixModuleData>();
            MixModulePost = new HashSet<MixModulePost>();
            MixPagePost = new HashSet<MixPagePost>();
            MixPostMedia = new HashSet<MixPostMedia>();
            MixPostModule = new HashSet<MixPostModule>();
            MixRelatedPostMixPost = new HashSet<MixPostAssociation>();
            MixRelatedPostS = new HashSet<MixPostAssociation>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Content { get; set; }
        public string EditorValue { get; set; }
        public MixEditorType? EditorType { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string Excerpt { get; set; }
        public string ExtraProperties { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public string Template { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int? Views { get; set; }
        public string ExtraFields { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixModulePost> MixModulePost { get; set; }
        public virtual ICollection<MixPagePost> MixPagePost { get; set; }
        public virtual ICollection<MixPostMedia> MixPostMedia { get; set; }
        public virtual ICollection<MixPostModule> MixPostModule { get; set; }
        public virtual ICollection<MixPostAssociation> MixRelatedPostMixPost { get; set; }
        public virtual ICollection<MixPostAssociation> MixRelatedPostS { get; set; }
    }
}