using Mix.Mixdb.ViewModels;

namespace Mix.Mixdb.Interfaces
{
    /// <summary>
    /// Interface defining methods for retrieving MixDb information
    /// </summary>
    public interface IMixDbInfoService
    {
        /// <summary>
        /// Gets MixDb configuration for a specific table
        /// </summary>
        Task<MixDbTableViewModel?> GetMixDb(string tableName);

        /// <summary>
        /// Loads MixDb configuration for a specific table
        /// </summary>
        Task LoadMixDb(string tableName);
    }
}