using Mix.Database.Extensions;
using Mix.Database.Services;

namespace Mix.Database.Base
{
    public abstract class BaseDbContext : DbContext
    {
        protected static string _connectionString;
        protected static MixDatabaseProvider? _databaseProvider;
        protected static DatabaseService _databaseService;
        protected readonly string _connectionStringName;
        protected Type _dbContextType;

        public BaseDbContext()
        {
            
        }
        public BaseDbContext(DatabaseService databaseService, string connectionStringName)
        {
            _databaseService = databaseService;
            _connectionStringName = connectionStringName;
            _dbContextType = GetType();
        }
        public BaseDbContext(string connectionString, MixDatabaseProvider databaseProvider)
        {
            _connectionString = connectionString;
            _databaseProvider = databaseProvider;
            _dbContextType = GetType();
        }

        protected override void OnConfiguring(
           DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _connectionString ?? _databaseService.GetConnectionString(_connectionStringName);
            var databaseProvider = _databaseProvider ?? _databaseService.DatabaseProvider;
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

                    default:
                        break;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations(_databaseService, _dbContextType.Assembly, $"{_dbContextType.Namespace}.EntityConfigurations");
        }
    }
}
