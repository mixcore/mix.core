using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
