using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.ViewModels;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Interfaces
{
    // TODO:
    // Only run after init CMS success
    // TODO: Add version to systemDatabases and update if have newer version

    public interface IMixdbStructure
    {
        public Task MigrateDatabase(string databaseName, CancellationToken cancellationToken = default);
        public Task<bool> BackupDatabase(string databaseName, CancellationToken cancellationToken = default);
        public Task<bool> RestoreFromLocal(string databaseName, CancellationToken cancellationToken = default);
        public Task ExecuteCommand(string commandText, CancellationToken cancellationToken);
        public Task AlterColumn(MixdbDatabaseColumnViewModel colDto, bool isDrop, CancellationToken cancellationToken = default);
        public Task AddColumn(MixdbDatabaseColumnViewModel repoCol, CancellationToken cancellationToken = default);
        public Task DropColumn(MixdbDatabaseColumnViewModel repoCol, CancellationToken cancellationToken = default);
        Task MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default);
        Task MigrateSystemDatabases(CancellationToken cancellationToken = default);
        public void InitDbStructureService(string cnn, MixDatabaseProvider databaseProvider);
    }
}
