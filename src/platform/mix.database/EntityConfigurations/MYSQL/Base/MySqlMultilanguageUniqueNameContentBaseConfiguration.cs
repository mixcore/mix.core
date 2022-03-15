using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageUniqueNameContentBase<TPrimaryKey>
    {
    }
}
