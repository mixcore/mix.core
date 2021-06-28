using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabases
{
    public class Helper
    {
        public static async Task<bool> MigrateDatabase(UpdateViewModel database, string culture)
        {
            if (database.Columns.Count > 0)
            {

                List<string> colSqls = new List<string>();
                foreach (var col in database.Columns)
                {
                    colSqls.Add(GenerateColumnSql(col));
                }
                string tableName = $"{MixConstants.CONST_MIXDB_PREFIX}{database.Name}_{culture.Replace("-", "_")}";
                string commandText = $"DROP TABLE IF EXISTS {tableName}; " +
                    $"CREATE TABLE {tableName} " +
                    $"(id varchar(50) NOT NULL Unique, specificulture varchar(50), mix_status varchar(50), createdDateTime {GetColumnType(MixDataType.DateTime)}, " +
                    $" {string.Join(",", colSqls.ToArray())})";
                if (!string.IsNullOrEmpty(commandText))
                {
                    using (var ctx = new MixCmsContext())
                    {
                        await ctx.Database.ExecuteSqlRawAsync(commandText);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private static string GenerateColumnSql(MixDatabaseColumns.UpdateViewModel col)
        {

            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.IsRequire ? "NOT NUll" : "NULL";
            return $"{col.Name} {colType} {nullable}";
        }

        private static string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            var provider = MixService.GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return provider switch
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
                    return provider switch
                    {
                        MixDatabaseProvider.MSSQL => "ntext",
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
    }
}