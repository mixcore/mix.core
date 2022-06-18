using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES.Base
{
    public abstract class PostgresMultilingualContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualContentBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualContentBase<TPrimaryKey>
    {
    }
}
