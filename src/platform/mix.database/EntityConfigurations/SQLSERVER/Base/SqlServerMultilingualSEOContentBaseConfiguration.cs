using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerMultilingualSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualSEOContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualSEOContentBase<TPrimaryKey>
    {
    }
}
