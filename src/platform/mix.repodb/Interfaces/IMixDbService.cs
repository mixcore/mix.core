using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.RepoDb.ViewModels;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;

namespace Mix.RepoDb.Interfaces
{
    public interface IMixDbService
    {
        public Task<PagingResponseModel<JObject>> GetMyData(string tableName, SearchMixDbRequestDto req, string username);

        public Task<JObject?> GetMyDataById(string tableName, string username, int id, bool loadNestedData);

        public Task<JObject?> GetById(string tableName, int id, bool loadNestedData);

        Task<JObject?> GetByParentIdAsync(string tableName, MixContentType parentType, int parentId, bool loadNestedData);
        Task<JObject> ParseDataAsync(string tableName, dynamic obj);
        public Task<bool> MigrateDatabase(RepoDbMixDatabaseViewModel database);

        public Task<bool> BackupDatabase(RepoDbMixDatabaseViewModel database, CancellationToken cancellationToken = default);
        public Task<bool> RestoreFromLocal(RepoDbMixDatabaseViewModel database);
        Task<bool> MigrateSystemDatabases(CancellationToken cancellationToken = default);
        Task<bool> MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default);
    }
}
