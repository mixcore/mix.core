using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixModule
    {
        public MixModule()
        {
            MixModuleAttributeData = new HashSet<MixModuleAttributeData>();
            MixModuleAttributeSet = new HashSet<MixModuleAttributeSet>();
            MixModuleData = new HashSet<MixModuleData>();
            MixModulePost = new HashSet<MixModulePost>();
            MixPageModule = new HashSet<MixPageModule>();
            MixPostModule = new HashSet<MixPostModule>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
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

        public virtual MixCulture SpecificultureNavigation { get; set; }
        public virtual ICollection<MixModuleAttributeData> MixModuleAttributeData { get; set; }
        public virtual ICollection<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
        public virtual ICollection<MixModulePost> MixModulePost { get; set; }
        public virtual ICollection<MixPageModule> MixPageModule { get; set; }
        public virtual ICollection<MixPostModule> MixPostModule { get; set; }
    }
}