using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class SqlServerSiteEntityBaseConfiguration<T, TPrimaryKey> 
        : SiteEntityBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : SiteEntityBase<TPrimaryKey>
    {
    }
}
