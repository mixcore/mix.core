using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.EntityConfigurations.MYSQL.Quartz;
using Mix.Database.Services.MixGlobalSettings;


namespace Mix.Database.Entities.Quartz
{
    public partial class MySQLQuartzDbContext : QuartzDbContext
    {
        public MySQLQuartzDbContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DatabaseService databaseService)
            : base(httpContextAccessor, configuration)
        {
            DbProvider = MixDatabaseProvider.MySQL;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
        }
        public MySQLQuartzDbContext(string connectionString)
                    : base(MixDatabaseProvider.SQLSERVER, connectionString)
        {
            DbProvider = MixDatabaseProvider.MySQL;
            ConnectionString = connectionString;
        }

        public MySQLQuartzDbContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor, configuration)
        {
        }

        protected override void OnConfiguring(
             DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MySqlQuartzConfigurations.Config(modelBuilder);
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
