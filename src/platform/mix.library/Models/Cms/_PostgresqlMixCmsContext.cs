using Microsoft.EntityFrameworkCore;
using Mix.Heart.Extensions;

namespace Mix.Lib.Models.Cms
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
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Lib.Models.EntityConfigurations.POSTGRESQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}