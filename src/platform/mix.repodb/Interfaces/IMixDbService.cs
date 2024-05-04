using Mix.Database.Entities.Cms;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.ViewModels;

namespace Mix.RepoDb.Interfaces
{
    public interface IMixDbService
    {
        public Task<bool> MigrateDatabase(RepoDbMixDatabaseViewModel database);
        public Task<bool> BackupDatabase(RepoDbMixDatabaseViewModel database, CancellationToken cancellationToken = default);
        public Task<bool> RestoreFromLocal(RepoDbMixDatabaseViewModel database);
        Task<bool> MigrateSystemDatabases(CancellationToken cancellationToken = default);
        Task<bool> MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default);
        Task<bool> AlterColumn(AlterColumnDto col);
        Task<bool> DropColumn(RepoDbMixDatabaseColumnViewModel col);
        Task<bool> AddColumn(RepoDbMixDatabaseColumnViewModel col);
    }
}
