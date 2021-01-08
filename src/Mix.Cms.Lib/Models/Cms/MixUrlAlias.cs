
using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixUrlAlias
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string SourceId { get; set; }
        public MixUrlAliasType Type { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
    }
}
