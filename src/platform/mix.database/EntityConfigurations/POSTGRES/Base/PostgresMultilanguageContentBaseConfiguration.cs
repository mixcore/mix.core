using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.POSTGRES.Base
{
    public abstract class PostgresMultilanguageContentBaseConfiguration<T, TPrimaryKey>
        : MultilanguageContentBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageContentBase<TPrimaryKey>
    {
    }
}
