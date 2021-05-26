using Microsoft.EntityFrameworkCore;
using Mix.Heart.Extensions;

namespace Mix.Lib.Entities.Cms
{
    public partial class MySqlMixCmsContext : MixCmsContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MySqlMixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }

        public MySqlMixCmsContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Lib.Models.EntityConfigurations.MySQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}