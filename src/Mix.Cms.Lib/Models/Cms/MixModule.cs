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
            MixModuleAttributeSet = new HashSet<MixModuleAttributeSet>();
            MixModuleData = new HashSet<MixModuleData>();
            MixModuleProduct = new HashSet<MixModuleProduct>();
            MixPageModule = new HashSet<MixPageModule>();
            MixProductModule = new HashSet<MixProductModule>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
        public string Image { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Template { get; set; }
        public string FormTemplate { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int? PageSize { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public MixCulture SpecificultureNavigation { get; set; }
        public ICollection<MixArticleModule> MixArticleModule { get; set; }
        public ICollection<MixModuleArticle> MixModuleArticle { get; set; }
        public ICollection<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public ICollection<MixModuleData> MixModuleData { get; set; }
        public ICollection<MixModuleProduct> MixModuleProduct { get; set; }
        public ICollection<MixPageModule> MixPageModule { get; set; }
        public ICollection<MixProductModule> MixProductModule { get; set; }
    }
}
