using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
{
    public abstract class MySqlMultilanguageSEOContentBaseConfiguration<T, TPrimaryKey> 
        : MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
    }
}
