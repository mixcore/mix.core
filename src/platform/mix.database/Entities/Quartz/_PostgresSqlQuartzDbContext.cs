using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.EntityConfigurations.POSTGRES.Quartz;
using Mix.Database.Services.MixGlobalSettings;


namespace Mix.Database.Entities.Quartz
{
    public partial class PostgresSQLQuartzDbContext : QuartzDbContext
    {
        public PostgresSQLQuartzDbContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DatabaseService databaseService)
            : base(httpContextAccessor, configuration)
        {
            DbProvider = MixDatabaseProvider.PostgreSQL;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
        }
        public PostgresSQLQuartzDbContext(string connectionString)
                    : base(MixDatabaseProvider.SQLSERVER, connectionString)
        {
            DbProvider = MixDatabaseProvider.PostgreSQL;
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(
             DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PostgresSqlQuartzConfigurations.Config(modelBuilder);
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
