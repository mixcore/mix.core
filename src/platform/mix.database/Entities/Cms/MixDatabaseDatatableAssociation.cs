
using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabaseContextDatabaseAssociation : AssociationBase<int>
    {
        public virtual ICollection<MixDatabase> MixDatabases { get; set; }
        public virtual ICollection<MixDatabaseContext> MixDatabaseContext { get; set; }
    }
}
