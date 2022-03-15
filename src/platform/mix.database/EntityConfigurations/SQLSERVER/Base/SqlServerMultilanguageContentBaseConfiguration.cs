using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerMultilanguageContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageContentBase<TPrimaryKey>
    {
    }
}
