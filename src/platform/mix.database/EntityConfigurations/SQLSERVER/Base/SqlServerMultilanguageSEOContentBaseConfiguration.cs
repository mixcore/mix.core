using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerMultilanguageSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageSEOContentBase<TPrimaryKey>
    {
    }
}
