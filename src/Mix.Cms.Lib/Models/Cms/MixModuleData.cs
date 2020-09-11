using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixModuleData
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int ModuleId { get; set; }
        public int? PageId { get; set; }
        public int? PostId { get; set; }
        public string Fields { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual MixPage MixPage { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}
