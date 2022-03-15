using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageSEOContentBase<TPrimaryKey>
    {
    }
}
