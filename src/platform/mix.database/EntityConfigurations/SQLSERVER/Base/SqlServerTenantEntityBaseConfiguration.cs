using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerTenantEntityBaseConfiguration<T, TPrimaryKey>
        : TenantEntityBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : TenantEntityBase<TPrimaryKey>
    {
    }
}
