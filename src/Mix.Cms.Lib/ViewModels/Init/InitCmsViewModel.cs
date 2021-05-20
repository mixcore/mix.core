using Mix.Cms.Lib.Enums;
using Mix.Heart.Enums;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixInit
{
    public class InitCmsViewModel
    {
        #region Properties

        [JsonProperty("connectionString")]
        public string ConnectionString {
            get {
                switch (DatabaseProvider)
                {
                    case MixDatabaseProvider.MSSQL:
                        {
                            string dbServer = !string.IsNullOrEmpty(DatabasePort) ? $"{DatabaseServer},{DatabasePort}" : DatabaseServer;
                            return IsUseLocal
                                ? LocalDbConnectionString
                                : $"Server={dbServer};Database={DatabaseName}" +
                                $";UID={DatabaseUser};Pwd={DatabasePassword};MultipleActiveResultSets=true;";
                        }
                    case MixDatabaseProvider.MySQL:
                        return $"Server={DatabaseServer};port={DatabasePort};Database={DatabaseName}" +
                      $";User={DatabaseUser};Password={DatabasePassword};";
                    case MixDatabaseProvider.PostgreSQL:
                        return $"Host={DatabaseServer};Port={DatabasePort};Database={DatabaseName};Username={DatabaseUser};Password={DatabasePassword}";
                    case MixDatabaseProvider.SQLITE:
                        return SqliteDbConnectionString;

                    default:
                        return string.Empty;
                }
            }
        }

        [JsonProperty("databaseServer")]
        public string DatabaseServer { get; set; }

        [JsonProperty("databasePort")]
        public string DatabasePort { get; set; }

        [JsonProperty("databaseName")]
        public string DatabaseName { get; set; }

        [JsonProperty("databaseUser")]
        public string DatabaseUser { get; set; }

        [JsonProperty("databasePassword")]
        public string DatabasePassword { get; set; }

        [JsonProperty("isUseLocal")]
        public bool IsUseLocal { get; set; }

        [JsonProperty("localDbConnectionString")]
        public string LocalDbConnectionString { get; set; } =
            $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog=mix-cms.db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";

        [JsonProperty("sqliteDbConnectionString")]
        public string SqliteDbConnectionString { get; set; } = $"Data Source=mix-cms.db";

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("databaseProvider")]
        public MixDatabaseProvider DatabaseProvider { get; set; }

        [JsonProperty("culture")]
        public InitCulture Culture { get; set; }

        [JsonProperty("siteName")]
        public string SiteName { get; set; } = "MixCore";

        #endregion Properties

        public InitCmsViewModel()
        {
        }
    }
}