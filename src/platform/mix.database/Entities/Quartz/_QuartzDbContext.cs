using Mix.Database.EntityConfigurations.MYSQL.Quartz;
using Mix.Database.EntityConfigurations.POSTGRES.Quartz;
using Mix.Database.EntityConfigurations.SQLITE.Quartz;
using Mix.Database.EntityConfigurations.SQLSERVER.Quartz;
using Mix.Database.Services;
using Mix.Shared.Constants;

namespace Mix.Database.Entities.Quartz
{
    public partial class QuartzDbContext : DbContext
    {
        public QuartzDbContext()
        {
            var databaseService = new MixDatabaseService();
            DbProvider = databaseService.DatabaseProvider;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
        }
        public QuartzDbContext(MixDatabaseProvider DbProvider, string connectionString)
                    : base()
        {
            this.DbProvider = DbProvider;
            ConnectionString = connectionString;
        }

        public QuartzDbContext(DbContextOptions<QuartzDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<QrtzBlobTrigger> QrtzBlobTriggers { get; set; }
        public virtual DbSet<QrtzCalendar> QrtzCalendars { get; set; }
        public virtual DbSet<QrtzCronTrigger> QrtzCronTriggers { get; set; }
        public virtual DbSet<QrtzFiredTrigger> QrtzFiredTriggers { get; set; }
        public virtual DbSet<QrtzJobDetail> QrtzJobDetails { get; set; }
        public virtual DbSet<QrtzLock> QrtzLocks { get; set; }
        public virtual DbSet<QrtzPausedTriggerGrp> QrtzPausedTriggerGrps { get; set; }
        public virtual DbSet<QrtzSchedulerState> QrtzSchedulerStates { get; set; }
        public virtual DbSet<QrtzSimpleTrigger> QrtzSimpleTriggers { get; set; }
        public virtual DbSet<QrtzSimpropTrigger> QrtzSimpropTriggers { get; set; }
        public virtual DbSet<QrtzTrigger> QrtzTriggers { get; set; }
        public MixDatabaseProvider DbProvider { get; }
        public string ConnectionString { get; }

        protected override void OnConfiguring(
             DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                switch (DbProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        optionsBuilder.UseSqlServer(ConnectionString);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
                        break;

                    case MixDatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(ConnectionString);
                        break;

                    case MixDatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(ConnectionString);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (DbProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    SqlServerQuartzConfigurations.Configure(modelBuilder);
                    break;
                case MixDatabaseProvider.MySQL:
                    MySqlQuartzConfigurations.Config(modelBuilder);
                    break;
                case MixDatabaseProvider.PostgreSQL:
                    PostgresSqlQuartzConfigurations.Config(modelBuilder);
                    break;
                case MixDatabaseProvider.SQLITE:
                    SQLiteQuartzConfigurations.Configure(modelBuilder);
                    break;
                default:
                    break;
            }
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
