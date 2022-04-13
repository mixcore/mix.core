using Mix.Database.EntityConfigurations.SQLSERVER.Quartz;
using Mix.Database.Services;
using Mix.Shared.Constants;

namespace Mix.Database.Entities.Quartz
{
    public partial class SQLServerQuartzDbContext : QuartzDbContext
    {
        public SQLServerQuartzDbContext(MixDatabaseService databaseService)
        {
            DbProvider = MixDatabaseProvider.SQLSERVER;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
        }
        public SQLServerQuartzDbContext(string connectionString)
                    : base(MixDatabaseProvider.SQLSERVER, connectionString)
        {
            DbProvider = MixDatabaseProvider.SQLSERVER;
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(
             DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SqlServerQuartzConfigurations.Configure(modelBuilder);
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
