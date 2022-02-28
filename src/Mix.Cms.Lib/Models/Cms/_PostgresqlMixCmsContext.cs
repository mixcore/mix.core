using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MixCmsContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PostgresqlMixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Cms.Lib.Models.EntityConfigurations.POSTGRESQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}