using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPageModule
    {
        public MixPageModule()
        {
            MixModuleAttributeSet = new HashSet<MixModuleAttributeSet>();
            MixModuleData = new HashSet<MixModuleData>();
        }

        public int ModuleId { get; set; }
        public int CategoryId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public MixModule MixModule { get; set; }
        public MixPage MixPage { get; set; }
        public ICollection<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public ICollection<MixModuleData> MixModuleData { get; set; }
    }
}
