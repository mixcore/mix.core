using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixConfiguration : TenantEntityUniqueNameBase<int>
    {
        public virtual ICollection<MixConfigurationContent> MixConfigurationContents { get; set; }
    }
}
