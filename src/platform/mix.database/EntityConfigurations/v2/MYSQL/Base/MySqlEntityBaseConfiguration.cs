using Mix.Database.EntityConfigurations.v2.Base;
using Mix.Heart.Entities;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
{
    public abstract class MySqlEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
