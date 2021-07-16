using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.v2.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class SqlServerMultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey> 
        : MultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
