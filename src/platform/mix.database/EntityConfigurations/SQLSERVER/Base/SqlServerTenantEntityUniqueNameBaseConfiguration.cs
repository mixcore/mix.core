using Mix.Database.Base;
using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerTenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey>
        : TenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : TenantEntityUniqueNameBase<TPrimaryKey>
    {
    }
}
