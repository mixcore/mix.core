using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostModule
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int ModuleId { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}
