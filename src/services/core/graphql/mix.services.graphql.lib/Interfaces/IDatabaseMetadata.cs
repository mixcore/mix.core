using Mix.Services.Graphql.Lib.Models;

namespace Mix.Services.Graphql.Lib.Interfaces
{
    public interface IDatabaseMetadata
    {
        void ReloadMetadata();
        IEnumerable<TableMetadata> GetTableMetadatas();
    }
}
