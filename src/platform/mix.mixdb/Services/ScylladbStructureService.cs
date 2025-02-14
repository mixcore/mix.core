using Cassandra;
using Microsoft.AspNetCore.Http;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Scylladb.Repositories;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using System.Diagnostics.Metrics;
using System.Dynamic;

namespace Mix.Mixdb.Services
{
    // Nuget: https://github.com/datastax/csharp-driver/?tab=readme-ov-file
    public class ScylladbStructureService : IMixdbStructureService
    {
        public MixDatabaseProvider DbProvider { get => MixDatabaseProvider.SCYLLADB; }
        public ScylladbRepository _repository;
        private IDatabaseConstants _databaseConstant = new CassandraDatabaseConstants();
        public ScylladbStructureService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            ScylladbRepository repository)
        {
            _repository = repository;
        }
        #region Implements
        public void Init(string connectionString, MixDatabaseProvider dbProvider)
        {
            _repository.Connect(connectionString);
        }
        public async Task<int> ExecuteCommand(string commandText)
        {
            var result = await _repository.ExecuteCommand(new SimpleStatement(commandText));
            return result.Count();
        }

        public async Task Migrate(MixDbDatabaseViewModel database, MixDatabaseProvider databaseProvider, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database is { Columns.Count: 0 })
            {
                throw new InvalidDataException();
            }

            var fieldNameService = new FieldNameService(database.NamingConvention);
            var databaseConstant = MixDbHelper.GetDatabaseConstant(databaseProvider);
            var colsSql = new List<string>();
            var tableName = database.SystemName;
            foreach (var col in database.Columns)
            {
                colsSql.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(tableName, database.Type, colsSql, fieldNameService);
            if (!string.IsNullOrEmpty(commandText))
            {
                await ExecuteCommand(
                    $"DROP TABLE IF EXISTS {databaseConstant.BacktickOpen}{tableName}{databaseConstant.BacktickClose};");
                var result = await ExecuteCommand(commandText);
            }
        }

        public async Task AddColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            var commandText = GenerateAddColumnSql(col);
            await ExecuteCommand(commandText);
        }

        public async Task AlterColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }

            var alterCommandText = isDrop
                ? $"{GenerateDropColumnSql(col)} {GenerateAddColumnSql(col)}"
                : GenerateAlterColumnSql(col);
            await ExecuteCommand(alterCommandText);
        }

        public async Task DropColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }

            var alterCommandText = GenerateDropColumnSql(col);
            await ExecuteCommand(alterCommandText);
        }


        public Task<bool> BackupDatabase(MixDbDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }



        public Task<bool> RestoreFromLocal(MixDbDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides

        #endregion

        #region Privates

        private string GetInsertQuery(ExpandoObject obj, List<string> selectMembers)
        {
            var result = obj.ToList();
            List<string> values = new();
            foreach (var col in selectMembers)
            {
                if (obj.Any(m => m.Key == col))
                {
                    var val = obj.First(m => m.Key == col).Value;
                    if (val != null)
                    {
                        if (val is string)
                        {
                            values.Add($"'{val.ToString()!.Replace("'", "\\'")}'");
                        }
                        else
                        {
                            values.Add($"'{val}'");
                        }

                        continue;
                    }
                }

                values.Add("NULL");
            }

            return $"({string.Join(',', values)})";
        }

        private string GetMigrateTableSql(string tableName,
            MixDatabaseType dbType,
            List<string> colsSql,
            FieldNameService fieldNameService)
        {
            return $"""
                CREATE TABLE {tableName} (
                {string.Join(",", colsSql.ToArray())}
                ) 
                """;
        }

        private string GetIdSyntax(MixDatabaseProvider databaseProvider, MixDatabaseType dbType)
        {
            if (dbType == MixDatabaseType.GuidService)
            {
                return
                    $"{_databaseConstant.BacktickOpen}{_databaseConstant.Guid}{_databaseConstant.BacktickClose} PRIMARY KEY";
            }

            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => $"{GetColumnType(MixDataType.Integer, _databaseConstant)} IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => $"integer PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL =>
                    $"{GetColumnType(MixDataType.Integer, _databaseConstant)} NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixdbDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, _databaseConstant, col.ColumnConfigurations.MaxLength);
            string primaryKey = string.Equals("id", col.SystemName, StringComparison.OrdinalIgnoreCase) ? "primary key" : string.Empty;
            return
                $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose} {colType} {primaryKey}";
        }

        private string GenerateAddColumnSql(MixdbDatabaseColumnViewModel col)
        {
            return
                $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" ADD {GenerateColumnSql(col)};";
        }

        private string GenerateDropColumnSql(MixdbDatabaseColumnViewModel col)
        {
            return
                $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" DROP {_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose};";
        }

        private string GenerateAlterColumnSql(MixdbDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, _databaseConstant, col.ColumnConfigurations.MaxLength);
            string colName = $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose}";
            string tableName =
                $"{_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}";
            string alterTable = $"ALTER TABLE {tableName} ";
            var result = alterTable +
                         $" ALTER {colName} TYPE {colType}";
            return result;
        }

        public string GetColumnType(MixDataType dataType, IDatabaseConstants databaseConstant, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.Date:
                    return databaseConstant.Date;
                case MixDataType.DateTime:
                case MixDataType.Time:
                    return databaseConstant.DateTime;
                case MixDataType.Double:
                    return "float";
                case MixDataType.Reference:
                case MixDataType.Integer:
                    return databaseConstant.Integer;
                case MixDataType.Long:
                    return databaseConstant.Long;
                case MixDataType.Guid:
                    return databaseConstant.Guid;
                case MixDataType.Html:
                    return databaseConstant.Text;
                case MixDataType.Boolean:
                    return databaseConstant.Boolean;
                case MixDataType.Json:
                case MixDataType.Array:
                case MixDataType.ArrayMedia:
                case MixDataType.ArrayRadio:
                case MixDataType.Text:
                    return databaseConstant.Text;
                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.PhoneNumber:
                case MixDataType.String:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                case MixDataType.BarCode:
                default:
                    return $"{databaseConstant.NString}";
            }
        }

        #endregion
    }
}
