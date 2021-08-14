using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixModule : SiteEntityUniqueNameBase<int>
    {
        public virtual MixModuleType Type{ get; set; }
        public virtual ICollection<MixModuleContent> MixModuleContents { get; set; }
    }
}
