using Mix.Database.Services;
using Mix.Shared.Constants;

namespace Mix.Database.Entities.Quartz
{
    public abstract class QuartzDbContext : DbContext
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
        public MixDatabaseProvider DbProvider { get; protected set; }
        public string ConnectionString { get; protected set; }

    }
}
