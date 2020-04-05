// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Services;
using Mix.Identity.Entities;
using Mix.Identity.Models;
using MySql.Data.MySqlClient;

namespace Mix.Cms.Lib.Models.Account
{
    public class MixDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MixDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixDbContext(DbContextOptions<MixDbContext> options)
                    : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MixDbContext" /> class.
        /// </summary>
        public MixDbContext()
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                if (MixService.GetConfig<int>(MixConstants.CONST_SETTING_DATABASE_PROVIDER) == (int)MixEnums.DatabaseProvider.MySQL)
                {
                    optionsBuilder.UseMySql(cnn);
                }
                else
                {
                    optionsBuilder.UseSqlServer(cnn);
                }
            }
        }
        //Ref https://github.com/dotnet/efcore/issues/10169
        public override void Dispose()
        {
            if (MixService.GetConfig<int>(MixConstants.CONST_SETTING_DATABASE_PROVIDER) == (int)MixEnums.DatabaseProvider.MySQL)
            {
                MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
            }
            else
            {
                SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
            }
            base.Dispose();
        }
    }
}