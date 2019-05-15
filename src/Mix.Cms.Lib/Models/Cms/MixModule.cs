using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixModule
    {
        public MixModule()
        {
            MixArticleModule = new HashSet<MixArticleModule>();
            MixModuleArticle = new HashSet<MixModuleArticle>();
            MixModuleAttributeData = new HashSet<MixModuleAttributeData>();
            MixModuleAttributeSet = new HashSet<MixModuleAttributeSet>();
            MixModuleData = new HashSet<MixModuleData>();
            MixModuleProduct = new HashSet<MixModuleProduct>();
            MixPageModule = new HashSet<MixPageModule>();
            MixProductModule = new HashSet<MixProductModule>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int? SetAttributeId { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Template { get; set; }
        public string FormTemplate { get; set; }
        public string EdmTemplate { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int? PageSize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual MixAttributeSet SetAttribute { get; set; }
        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixArticleModule> MixArticleModule { get; set; }
        public virtual ICollection<MixModuleArticle> MixModuleArticle { get; set; }
        public virtual ICollection<MixModuleAttributeData> MixModuleAttributeData { get; set; }
        public virtual ICollection<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixModuleProduct> MixModuleProduct { get; set; }
        public virtual ICollection<MixPageModule> MixPageModule { get; set; }
        public virtual ICollection<MixProductModule> MixProductModule { get; set; }
    }
}
