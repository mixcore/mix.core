using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
