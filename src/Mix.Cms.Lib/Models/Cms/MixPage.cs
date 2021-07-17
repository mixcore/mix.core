using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPage: AuditedEntity
    {
        public MixPage()
        {
            MixModuleData = new HashSet<MixModuleData>();
            MixPageModule = new HashSet<MixPageModule>();
            MixPagePost = new HashSet<MixPagePost>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Content { get; set; }
        public string EditorValue { get; set; }
        public MixEditorType? EditorType { get; set; }
        public string CssClass { get; set; }
        public string Excerpt { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string Layout { get; set; }
        public int? Level { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public string StaticUrl { get; set; }
        public string Tags { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public MixPageType Type { get; set; }
        public string PostType { get; set; }
        public int? Views { get; set; }
        public int? PageSize { get; set; }
        public string ExtraFields { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixPageModule> MixPageModule { get; set; }
        public virtual ICollection<MixPagePost> MixPagePost { get; set; }
    }
}