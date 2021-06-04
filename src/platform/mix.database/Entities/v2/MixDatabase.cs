using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDatabase : SiteEntityBase<int>
    {
        public MixDatabaseType Type { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
    }
}
