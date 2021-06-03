using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixConfiguration : EntityBase<int>
    {
        public string SystemName { get; set; }
        public ICollection<MixConfigurationContent> MixConfigurationContents { get; set; }
    }
}
