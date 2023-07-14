using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MixFileHelper.CreateFolderIfNotExist(MixFolders.MixAuditLogFolder);

            string cnn = $"Data Source={MixFolders.MixAuditLogFolder}/auditlog_{DateTime.Now.ToString("dd_MM_yyyy")}.db";
            optionsBuilder.UseSqlite(cnn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<AuditLog>(new AuditLogConfiguration());
        }
        public virtual DbSet<AuditLog> AuditLog { get; set; }

    }
}
