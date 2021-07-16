using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
{
    public abstract class MySqlMultilanguageContentBaseConfiguration<T, TPrimaryKey> 
        : MultilanguageContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageContentBase<TPrimaryKey>
    {
    }
}
