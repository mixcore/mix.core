using Mix.Database.Entities.Base;

using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDatatable : TenantEntityUniqueNameBase<int>
    {
        public virtual ICollection<MixDatabase> MixDatabases { get; set; }
        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
    }
}
