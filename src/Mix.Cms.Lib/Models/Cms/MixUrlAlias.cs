using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixUrlAlias
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string SourceId { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
    }
}
