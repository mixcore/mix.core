using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerMultilingualContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualContentBase<TPrimaryKey>
    {
    }
}
