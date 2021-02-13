// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;
using Mix.Identity.Data;

namespace Mix.Cms.Lib.Models.Account
{
    public partial class SQLAccountContext : MixCmsAccountContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SQLAccountContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options)
        {
        }

        public SQLAccountContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyIddentityConfigurations();
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}