using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixConfiguration : SiteEntityUniqueNameBase<int>
    {
        public virtual ICollection<MixConfigurationContent> MixConfigurationContents { get; set; }
    }
}
