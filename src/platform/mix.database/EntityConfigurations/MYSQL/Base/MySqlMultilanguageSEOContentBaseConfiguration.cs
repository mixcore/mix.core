using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultiLanguageSEOContentBase<TPrimaryKey>
    {
    }
}
