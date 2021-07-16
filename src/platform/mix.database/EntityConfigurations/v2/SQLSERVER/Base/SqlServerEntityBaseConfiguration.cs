using Mix.Database.EntityConfigurations.v2.Base;
using Mix.Heart.Entities;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class SqlServerEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
