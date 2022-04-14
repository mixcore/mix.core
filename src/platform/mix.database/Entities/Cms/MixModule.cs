using Mix.Database.Entities.Base;

using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixModule : TenantEntityUniqueNameBase<int>
    {
        public virtual MixModuleType Type { get; set; }
        public virtual ICollection<MixModuleContent> MixModuleContents { get; set; }
    }
}
