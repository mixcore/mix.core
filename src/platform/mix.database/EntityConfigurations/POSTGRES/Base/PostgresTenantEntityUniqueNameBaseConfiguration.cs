using Mix.Database.Base;
using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES.Base
{
    public abstract class PostgresTenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey>
        : TenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : TenantEntityUniqueNameBase<TPrimaryKey>
    {
    }
}
