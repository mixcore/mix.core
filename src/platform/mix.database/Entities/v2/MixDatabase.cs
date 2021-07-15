using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDatabase : SiteEntityUniqueNameBase<int>
    {
        public MixDatabaseType Type { get; set; }

        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
    }
}
