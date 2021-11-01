// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Services;
using MySqlConnector;
using Mix.Database.Extensions;
using Mix.Database.Services;
using System;
using Mix.Shared.Enums;

namespace Mix.Database.Entities.Account
{
    public partial class MixCmsAccountContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

        private static MixDatabaseService _databaseService;

        public MixCmsAccountContext()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsAccountContext(
            MixDatabaseService databaseService)
                    : base()
        {
            _databaseService = databaseService;
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                switch (_databaseService.DatabaseProvider)
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
            switch (_databaseService.DatabaseProvider)
            {
                case MixDatabaseProvider.PostgreSQL:
                    modelBuilder.ApplyPostgresIddentityConfigurations();
                    break;

                case MixDatabaseProvider.SQLSERVER:
                case MixDatabaseProvider.MySQL:
                case MixDatabaseProvider.SQLITE:
                default:
                    modelBuilder.ApplyIddentityConfigurations();
                    break;
            }
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}