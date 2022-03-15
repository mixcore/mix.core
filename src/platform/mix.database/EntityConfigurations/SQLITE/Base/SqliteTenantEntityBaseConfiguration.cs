using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteTenantEntityBaseConfiguration<T, TPrimaryKey>
        : TenantEntityBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : TenantEntityBase<TPrimaryKey>
    {
    }
}
