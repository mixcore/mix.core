// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.Data.SqlClient;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;

using MySqlConnector;

namespace Mix.Database.Entities.Account
{
    public partial class MixCmsAccountContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<MixUser> MixUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<MixUserTenant> MixUserTenants { get; set; }
        public virtual DbSet<MixRole> MixRoles { get; set; }

        private static DatabaseService _databaseService;

        public MixCmsAccountContext()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MixCmsAccountContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsAccountContext(
            DatabaseService databaseService)
                    : base()
        {
            _databaseService = databaseService;
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _databaseService.GetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION);
            var _databaseProvider = MixDatabaseProvider.PostgreSQL;
            cnn = "Host=localhost;Database=mixtest1234;Username=postgres;Password=1234qwe@";
            if (!string.IsNullOrEmpty(cnn))
            {
                switch (_databaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        optionsBuilder.UseSqlServer(cnn);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn, ServerVersion.AutoDetect(cnn));
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

        //Ref https://github.com/dotnet/efcore/issues/10169
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_databaseService.AppSettings.ClearDbPool)
            {
                switch (_databaseService.DatabaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                        break;

                    case MixDatabaseProvider.MySQL:
                        MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                        break;
                }
            }
            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string ns = _databaseService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER
                     => typeof(SqlServerDatabaseConstants).Namespace,

                MixDatabaseProvider.MySQL
                    => typeof(MySqlDatabaseConstants).Namespace,

                MixDatabaseProvider.SQLITE
                    => typeof(SqliteDatabaseConstants).Namespace,

                MixDatabaseProvider.PostgreSQL
                    => typeof(PostgresDatabaseConstants).Namespace,
                _ => string.Empty
            };
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == $"{ns}.Account");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}