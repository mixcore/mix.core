using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPageModule
    {
        public MixPageModule()
        {
            MixModuleData = new HashSet<MixModuleData>();
        }

        public int ModuleId { get; set; }
        public int PageId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual MixPage MixPage { get; set; }
        public virtual ICollection<MixModuleData> MixModuleData { get; set; }
    }
}