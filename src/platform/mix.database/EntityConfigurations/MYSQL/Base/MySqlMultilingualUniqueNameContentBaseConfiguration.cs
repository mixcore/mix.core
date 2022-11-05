using Mix.Database.Base;
using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultilingualUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualUniqueNameContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualUniqueNameContentBase<TPrimaryKey>
    {
    }
}
