using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.ViewModels;
using RepoDb.Interfaces;

namespace Mix.RepoDb.Services
{
    public class MixDbService
    {
        #region Properties

        public ITrace Trace { get; }

        public ICache Cache { get; }
        private UnitOfWorkInfo<MixCmsContext> _uow;
        private DatabaseService _databaseService;
        #endregion

        public MixDbService(UnitOfWorkInfo<MixCmsContext> uow, DatabaseService databaseService)
        {
            _uow = uow;
            _databaseService = databaseService;
        }

        #region Migrate

        public async Task<bool> MigrateDatabase(int databaseId)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(databaseId);
            if (database!= null && database.Columns.Count > 0)
            {

                List<string> colSqls = new List<string>();
                foreach (var col in database.Columns)
                {
                    colSqls.Add(GenerateColumnSql(col));
                }
                string tableName = $"{MixConstants.CONST_MIXDB_PREFIX}{database.SystemName}";
                string commandText = $"DROP TABLE IF EXISTS {tableName}; " +
                    $"CREATE TABLE {tableName} " +
                    $"(id {GetAutoIncreaseIdSyntax()}, specificulture varchar(50), status varchar(50), createdDateTime {GetColumnType(MixDataType.DateTime)}, " +
                    $" {string.Join(",", colSqls.ToArray())})";
                if (!string.IsNullOrEmpty(commandText))
                {
                        await _uow.ActiveDbContext.Database.ExecuteSqlRawAsync(commandText);
                        return true;
                }
                return false;
            }
            return false;
        }

        private string GetAutoIncreaseIdSyntax()
        {
            return _databaseService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => "int IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => "INTEGER PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL => "int NOT NULL AUTO_INCREMENT",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixDatabaseColumnViewModel col)
        {

            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            return $"{col.SystemName} {colType} {nullable}";
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return _databaseService.DatabaseProvider switch
                    {
                        MixDatabaseProvider.PostgreSQL => "timestamp without time zone",
                        _ => "datetime"
                    };
                case MixDataType.Double:
                    return "float";
                case MixDataType.Integer:
                    return "int";
                case MixDataType.Reference: // JObject - { ref_table_name: "", children: [] }
                case MixDataType.Html:
                    return _databaseService.DatabaseProvider switch
                    {
                        MixDatabaseProvider.SQLSERVER => "ntext",
                        _ => "text"
                    };
                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Boolean:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                default:
                    return $"varchar({maxLength ?? 250})";
            }
        }

        #endregion
        
    }
}
