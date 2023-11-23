using Mix.Database.Extensions;
using Mix.Database.Services;

namespace Mix.Database.Base
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly string _connectionString;
        protected MixDatabaseProvider? DatabaseProvider;
        protected DatabaseService DatabaseService;
        protected readonly string _connectionStringName;
        protected Type _dbContextType;

        public BaseDbContext(DatabaseService databaseService, string connectionStringName)
        {
            DatabaseService = databaseService;
            _connectionStringName = connectionStringName;
            _dbContextType = GetType();
        }
        public BaseDbContext(string connectionString, MixDatabaseProvider databaseProvider)
        {
            _connectionString = connectionString;
            DatabaseProvider = databaseProvider;
            _dbContextType = GetType();
        }

        protected override void OnConfiguring(
           DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _connectionString ?? DatabaseService.GetConnectionString(_connectionStringName);
            var databaseProvider = DatabaseProvider ?? DatabaseService.DatabaseProvider;
            if (!string.IsNullOrEmpty(cnn))
            {
                switch (databaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        optionsBuilder.UseSqlServer(cnn);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn, ServerVersion.AutoDetect(cnn),
                            b => b.UseMicrosoftJson(MySqlCommonJsonChangeTrackingOptions.FullHierarchyOptimizedFast));
                        break;

                    case MixDatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(cnn);
                        break;

                    case MixDatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(cnn);
                        break;

                    case MixDatabaseProvider.INMEMORY:
                        optionsBuilder.UseInMemoryDatabase(cnn);
                        break;

                    default:
                        break;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations(DatabaseService, _dbContextType.Assembly, $"{_dbContextType.Namespace}.EntityConfigurations");
        }
    }
}
