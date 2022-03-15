using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultiLanguageContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageContentBase<TPrimaryKey>
    {
    }
}
