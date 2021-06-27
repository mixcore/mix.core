// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.Account
{
    public partial class PostgresSQLAccountContext : MixCmsAccountContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PostgresSQLAccountContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options)
        {
        }

        public PostgresSQLAccountContext()
        {
        }

        public PostgresSQLAccountContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) 
            : base(databaseService, appSettingService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyPostgresIddentityConfigurations();
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}