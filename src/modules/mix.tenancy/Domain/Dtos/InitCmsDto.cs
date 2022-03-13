using Mix.Tenancy.Domain.Models;

namespace Mix.Tenancy.Domain.Dtos
{
    public class InitCmsDto
    {
        #region Properties

        public string PrimaryDomain { get; set; }
        public string ConnectionString
        {
            get
            {
                switch (DatabaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        {
                            string dbServer = !string.IsNullOrEmpty(DatabasePort) ? $"{DatabaseServer},{DatabasePort}" : DatabaseServer;
                            return $"Server={dbServer};Database={DatabaseName}" +
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

        public string DatabaseServer { get; set; }

        public string DatabasePort { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

        public string SqliteDbConnectionString { get; set; } = $"Data Source=mix-cms.db";

        public string Lang { get; set; }

        public MixDatabaseProvider DatabaseProvider { get; set; }

        public InitCultureModel Culture { get; set; }

        public string SiteName { get; set; } = "MixCore";

        #endregion Properties

        public InitCmsDto()
        {
        }
    }
}
