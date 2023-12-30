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

        public Task<bool> MigrateDatabase(string name);

        public Task<bool> RestoreFromLocal(string name);

        public Task<bool> BackupDatabase(string databaseName, CancellationToken cancellationToken = default);
        Task<JObject?> GetByParentIdAsync(string tableName, MixContentType parentType, int parentId, bool loadNestedData);
        Task<JObject> ParseDataAsync(string tableName, dynamic obj);
        Task<bool> MigrateSystemDatabases(CancellationToken cancellationToken = default);
        Task<bool> MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default);
    }
}
