using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MixFileHelper.CreateFolderIfNotExist($"{MixFolders.MixAuditLogFolder}/{DateTime.Now.ToString("dd_MM")}");
            string cnn = $"Data Source={MixFolders.MixAuditLogFolder}/{DateTime.Now.ToString("dd_MM")}/auditlog_{DateTime.Now.ToString("dd_MM_yyyy")}.sqlite";
            optionsBuilder.UseSqlite(cnn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        }
        public virtual DbSet<AuditLog> AuditLog { get; set; }

    }
}
