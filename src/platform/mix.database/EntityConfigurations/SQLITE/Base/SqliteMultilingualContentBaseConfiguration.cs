using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultilingualContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualContentBase<TPrimaryKey>
    {
    }
}
