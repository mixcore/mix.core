using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
{
    public abstract class MySqlSiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey> 
        : SiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : SiteEntityUniqueNameBase<TPrimaryKey>
    {
    }
}
