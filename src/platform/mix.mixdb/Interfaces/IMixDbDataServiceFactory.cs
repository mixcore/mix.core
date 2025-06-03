using Mix.Constant.Enums;
using Mix.Heart.Enums;

namespace Mix.Mixdb.Interfaces
{
    /// <summary>
    /// Factory interface for creating IMixDbDataService instances
    /// </summary>
    public interface IMixDbDataServiceFactory
    {
        /// <summary>
        /// Creates an instance of IMixDbDataService based on the database provider
        /// </summary>
        IMixDbDataService Create(MixDatabaseProvider provider, string connectionString);
    }
}