using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE.Base
{
    public abstract class SqliteSiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey> 
        : SiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : SiteEntityUniqueNameBase<TPrimaryKey>
    {
    }
}
