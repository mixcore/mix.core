using Microsoft.AspNetCore.Components.Web;
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
    public interface IMixdbStructureService
    {
        public MixDatabaseProvider DbProvider { get; }
        void Init(string connectionString, MixDatabaseProvider dbProvider);
        public Task<int> ExecuteCommand(string commandText);
        public Task AlterColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default);
        public Task AddColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default);
        public Task DropColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default);
        public Task Migrate(MixDbDatabaseViewModel database, MixDatabaseProvider databaseProvider, CancellationToken cancellationToken = default);
    }
}
