using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE.Base
{
    public abstract class SqliteMultilanguageContentBaseConfiguration<T, TPrimaryKey> 
        : MultilanguageContentBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageContentBase<TPrimaryKey>
    {
    }
}
