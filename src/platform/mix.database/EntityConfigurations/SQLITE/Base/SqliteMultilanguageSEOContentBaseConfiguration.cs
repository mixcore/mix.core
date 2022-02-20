using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.SQLITE.Base
{
    public abstract class SqliteMultilanguageSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
    }
}
