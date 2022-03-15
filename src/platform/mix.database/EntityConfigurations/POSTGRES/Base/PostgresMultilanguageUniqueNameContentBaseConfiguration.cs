using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES.Base
{
    public abstract class PostgresMultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, PostgresDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
