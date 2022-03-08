using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.POSTGRES.Base
{
    public abstract class PostgresMultilanguageSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageSEOContentBase<TPrimaryKey>
    {
    }
}
