using Mix.Database.EntityConfigurations.SQLITE.Quartz;
using Mix.Database.Services;


namespace Mix.Database.Entities.Quartz
{
    public partial class SQLiteQuartzDbContext : QuartzDbContext
    {
        public SQLiteQuartzDbContext(DatabaseService databaseService)
        {
            DbProvider = MixDatabaseProvider.SQLITE;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
        }
        public SQLiteQuartzDbContext(string connectionString)
                    : base(MixDatabaseProvider.SQLITE, connectionString)
        {
            DbProvider = MixDatabaseProvider.SQLITE;
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(
             DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SQLiteQuartzConfigurations.Configure(modelBuilder);
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
