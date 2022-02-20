using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
