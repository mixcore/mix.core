using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultiLanguageContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageContentBase<TPrimaryKey>
    {
    }
}
