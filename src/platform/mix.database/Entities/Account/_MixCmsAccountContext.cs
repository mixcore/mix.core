// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.Data.SqlClient;
using Mix.Database.Services;

using MySqlConnector;

namespace Mix.Database.Entities.Account
{
    public partial class MixCmsAccountContext : BaseDbContext
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
        public virtual DbSet<OAuthApplicationEntity> OAuthApplications { get; set; }
        public virtual DbSet<OAuthTokenEntity> OAuthTokens { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MixCmsAccountContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsAccountContext(
            DatabaseService databaseService)
                    : base(databaseService, MixConstants.CONST_ACCOUNT_CONNECTION)
        {
            _databaseService = databaseService;
        }
        public MixCmsAccountContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
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

    }
}