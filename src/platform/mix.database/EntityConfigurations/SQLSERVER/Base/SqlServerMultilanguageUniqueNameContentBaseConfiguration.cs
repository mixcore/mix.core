using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SqlServerMultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, SqlServerDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
