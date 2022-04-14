using Mix.Database.Entities.Base;

using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabase : TenantEntityUniqueNameBase<int>
    {
        public MixDatabaseType Type { get; set; }

        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
    }
}
