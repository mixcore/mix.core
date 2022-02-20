using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlAssociationBaseConfiguration<T, TPrimaryKey>
        : EntityBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
