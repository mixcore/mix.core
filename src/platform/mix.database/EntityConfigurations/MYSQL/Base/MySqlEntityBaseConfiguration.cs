using Mix.Database.Base;
using Mix.Database.EntityConfigurations.Base;

namespace Mix.Database.EntityConfigurations.MYSQL.Base
{
    public abstract class MySqlEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey, MySqlDatabaseConstants>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
    }
}
