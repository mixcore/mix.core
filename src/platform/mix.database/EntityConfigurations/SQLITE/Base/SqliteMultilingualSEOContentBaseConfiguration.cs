using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultilingualSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualSEOContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualSEOContentBase<TPrimaryKey>
    {
    }
}
