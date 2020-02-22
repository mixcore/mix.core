using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixInit
{
    public class InitCmsViewModel
    {
        #region Properties

        [JsonProperty("connectionString")]
        public string ConnectionString {
            // If use local db  => return local db cnn string
            // Else If use remote db
            // => return: if use mysql => return mysql cnn string
            //              else return remote mssql cnn string
            get {
                switch (DatabaseProvider)
                {
                    case MixEnums.DatabaseProvider.MSSQL:
                        {
                            string dbServer = !string.IsNullOrEmpty(DatabasePort) ? $"{DatabaseServer},{DatabasePort}" : DatabaseServer;
                            return IsUseLocal
                                ? LocalDbConnectionString
                                : $"Server={dbServer};Database={DatabaseName}" +
                                $";UID={DatabaseUser};Pwd={DatabasePassword};MultipleActiveResultSets=true;";
                        }
                    case MixEnums.DatabaseProvider.MySQL:
                        return $"Server={DatabaseServer};port={DatabasePort};Database={DatabaseName}" +
                      $";User={DatabaseUser};Password={DatabasePassword};";

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

        [JsonProperty("isMysql")]
        public bool IsMysql { get { return DatabaseProvider == MixEnums.DatabaseProvider.MySQL; } }

        [JsonProperty("databaseProvider")]
        public MixEnums.DatabaseProvider DatabaseProvider { get; set; }

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