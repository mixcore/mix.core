using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixModule : SiteEntityBase<int>
    {
        public virtual ICollection<MixModuleContent> MixModuleContents { get; set; }
    }
}
