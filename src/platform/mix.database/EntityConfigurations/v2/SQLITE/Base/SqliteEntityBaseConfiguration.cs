using Mix.Database.EntityConfigurations.v2.Base;
using Mix.Heart.Entities;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE.Base
{
    public abstract class SqliteEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey, SqliteDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
