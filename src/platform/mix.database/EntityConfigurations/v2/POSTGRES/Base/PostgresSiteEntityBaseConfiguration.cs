using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES.Base
{
    public abstract class PostgresSiteEntityBaseConfiguration<T, TPrimaryKey> 
        : SiteEntityBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : SiteEntityBase<TPrimaryKey>
    {
    }
}
