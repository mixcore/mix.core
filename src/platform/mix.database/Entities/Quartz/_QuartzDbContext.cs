using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using System.Linq;


namespace Mix.Database.Entities.Quartz
{
    public abstract class QuartzDbContext : DbContext
    {
        protected IHttpContextAccessor _httpContextAccessor;
        public QuartzDbContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            using var dbContext = new GlobalSettingContext(configuration);
            var settings = dbContext.MixGlobalSetting.First(m => m.SystemName == "database");
            var databaseService = new DatabaseService(_httpContextAccessor, configuration, settings);
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
