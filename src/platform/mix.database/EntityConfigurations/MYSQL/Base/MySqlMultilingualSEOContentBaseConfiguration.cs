using Mix.Database.Entities.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlMultilingualSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualSEOContentBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : MultilingualSEOContentBase<TPrimaryKey>
    {
    }
}
