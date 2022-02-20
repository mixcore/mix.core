using Mix.Database.EntityConfigurations.Base;
using System;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerAssociationBaseConfiguration<T, TPrimaryKey>
        : EntityBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
