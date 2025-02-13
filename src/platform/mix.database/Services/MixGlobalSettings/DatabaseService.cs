using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.MixDb;
using Mix.Database.Entities.Quartz;
using Mix.Database.Entities.QueueLog;
using Mix.Database.Entities.Settings;
using Mix.Heart.Constants;
using Mix.Heart.Entities.Cache;
using Mix.Heart.Services;
using Mix.Shared.Models.Configurations;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class DatabaseService : GlobalSettingServiceBase
    {
        public DatabaseConfigurations AppSettings { get; set; }

        public MixDatabaseProvider DatabaseProvider => AppSettings.DatabaseProvider;
        protected IHttpContextAccessor HttpContextAccessor;

        public DatabaseService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixGlobalSetting settings)
            : base(configuration, settings)
        {
            AppSettings = RawSettings.ToObject<DatabaseConfigurations>();
            HttpContextAccessor = httpContextAccessor;
        }

        public string GetConnectionString(string name)
        {
            switch (name)
            {
                case MixConstants.CONST_CMS_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixCmsConnection;
                case MixConstants.CONST_AUDIT_LOG_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixAuditLogConnection;
                case MixConstants.CONST_QUEUE_LOG_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixQueueLogConnection;
                case MixConstants.CONST_ACCOUNT_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixAccountConnection;
                case MixConstants.CONST_MIXDB_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixDbConnection;
                case MixConstants.CONST_QUARTZ_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixQuartzConnection;
                default:
                    return RawSettings.Value<JObject>(MixAppSettingsSection.ConnectionStrings).Value<string>(name)
                        ?? _configuration.GetConnectionString(name);
            }
        }

        public void SetConnectionString(string name, string value, bool isSave = false)
        {
            RawSettings[MixAppSettingsSection.ConnectionStrings][name] = value;
            _configuration.GetSection(MixAppSettingsSection.ConnectionStrings)[name] = value;
            if (isSave)
            {
                SaveSettings();
            }
        }



        public MixCmsContext GetDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerMixCmsContext(this),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this),
                _ => null,
            };
        }

        public MixDbDbContext GetMixDbDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerMixDbDbContext(this),
                MixDatabaseProvider.MySQL => new MySqlMixDbDbContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixDbDbContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSqlMixDbDbContext(this),
                _ => null,
            };
        }

        public MixCmsContext GetMixcmsContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new(this),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this),
                _ => null,
            };
        }

        public MixCmsAccountContext GetAccountDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerAccountContext(this),
                MixDatabaseProvider.MySQL => new MySqlAccountContext(this),
                MixDatabaseProvider.SQLITE => new SqliteAccountContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSqlAccountContext(this),
                _ => null,
            };
        }

        public AuditLogDbContext GetAuditLogDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerAuditLogDbContext(this),
                MixDatabaseProvider.MySQL => new MySqlAuditLogDbContext(this),
                MixDatabaseProvider.SQLITE => new SqliteAuditLogDbContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresAuditLogDbContext(this),
                _ => null,
            };
        }
        public QueueLogDbContext GetQueueLogDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerQueueLogDbContext(this),
                MixDatabaseProvider.MySQL => new MySqlQueueLogDbContext(this),
                MixDatabaseProvider.SQLITE => new SqliteQueueLogDbContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresQueueLogDbContext(this),
                _ => null,
            };
        }

        public QuartzDbContext GetQuartzDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SQLServerQuartzDbContext(HttpContextAccessor, _configuration, this),
                MixDatabaseProvider.MySQL => new MySQLQuartzDbContext(HttpContextAccessor, _configuration, this),
                MixDatabaseProvider.SQLITE => new SQLiteQuartzDbContext(HttpContextAccessor, _configuration, this),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLQuartzDbContext(HttpContextAccessor, _configuration, this),
                _ => null,
            };
        }

        public MixCacheDbContext GetCacheDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerCacheDbContext(_configuration),
                MixDatabaseProvider.MySQL => new MySqlCacheDbContext(_configuration),
                MixDatabaseProvider.SQLITE => new SqliteCacheDbContext(_configuration),
                MixDatabaseProvider.PostgreSQL => new PostgresCacheDbContext(_configuration),
                _ => null,
            };
        }

        public void InitConnectionStrings(string connectionString,
            MixDatabaseProvider databaseProvider)
        {
            AppSettings.DatabaseProvider = databaseProvider;

            SetConfig(nameof(DatabaseProvider), databaseProvider);
            _configuration["DatabaseProvider"] = databaseProvider.ToString();
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            // TODO: Seperate Account db. Current Store account to same database
            if (databaseProvider == MixDatabaseProvider.SQLITE)
            {
                SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, connectionString.Replace(".sqlite", "") + "-quartz.sqlite");
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString.Replace(".sqlite", "") + "-account.sqlite");
                SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, connectionString.Replace(".sqlite", "") + "-mixdb.sqlite");
                SetConnectionString(MixConstants.CONST_AUDIT_LOG_CONNECTION, connectionString.Replace(".sqlite", "") + "-audit-log.sqlite");
                SetConnectionString(MixConstants.CONST_QUEUE_LOG_CONNECTION, connectionString.Replace(".sqlite", "") + "-queue-log.sqlite");
            }
            else
            {
                SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_AUDIT_LOG_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_QUEUE_LOG_CONNECTION, connectionString);
            }
            SaveSettings();
            //ToDo: update init
            //MixHeartConfigService.Instance.AppSettingsModel.DatabaseProvider = databaseProvider;
            //MixHeartConfigService.Instance.SetConnectionString(MixHeartConstants.CACHE_CONNECTION, connectionString);
        }

        public void ResetConnectionStrings()
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, null);
            //ToDo: update init
            //MixHeartConfigService.Instance.SetConnectionString(MixHeartConstants.CACHE_CONNECTION, null);
            SaveSettings();
        }
        public void UpdateMixCmsContext()
        {
            using var ctx = GetDbContext();
            {
                MigrateDbContext(ctx);
            }
            using var settingsCtx = GetSettingDbContext();
            {
                MigrateDbContext(settingsCtx);
            }
            //using var cacheCtx = GetCacheDbContext();
            //if (cacheCtx.Database.GetPendingMigrations().Count() > 0)
            //    cacheCtx.Database.Migrate();
            using var accCtx = GetAccountDbContext();
            {
                MigrateDbContext(accCtx);
            }
            using var mixdbCtx = GetMixDbDbContext();
            {
                MigrateDbContext(mixdbCtx);
            }
            using var auditlogCtx = GetAuditLogDbContext();
            {
                MigrateDbContext(auditlogCtx);
            }
            using var queuelogCtx = GetQueueLogDbContext();
            {
                MigrateDbContext(queuelogCtx);
            }
        }

        private void MigrateDbContext(DbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (ctx.Database.GetPendingMigrations().Count() > 0)
                ctx.Database.Migrate();
        }

        public async Task InitQuartzContextAsync()
        {
            if (!AppSettingsService.AppSettings.IsInit)
            {
                using var ctx = GetQuartzDbContext();
                await ctx.Database.MigrateAsync();
            }
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            AppSettings = RawSettings.ToObject<DatabaseConfigurations>();
        }

        public override void SetConfig<TValue>(string name, TValue value, bool isSave = false)
        {
            base.SetConfig(name, value, isSave);
            AppSettings = RawSettings.ToObject<DatabaseConfigurations>();
        }
    }
}
