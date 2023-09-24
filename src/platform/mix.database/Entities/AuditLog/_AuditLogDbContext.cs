using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLogDbContext : DbContext
    {
        private string _folder = DateTime.UtcNow.ToString("MM_yyyy");
        private string _cnn;
        public AuditLogDbContext()
        {
            _cnn = $"Data Source={MixFolders.MixAuditLogFolder}/{_folder}/auditlog_{DateTime.Now.ToString("dd_MM_yyyy")}.sqlite";
        }
        public AuditLogDbContext(DateTime date)
        {
            _folder = date.ToString("MM_yyyy");
            _cnn = $"Data Source={MixFolders.MixAuditLogFolder}/{_folder}/auditlog_{date.ToString("dd_MM_yyyy")}.sqlite";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MixFileHelper.CreateFolderIfNotExist($"{MixFolders.MixAuditLogFolder}/{_folder}");
            optionsBuilder.UseSqlite(_cnn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        }
        public virtual DbSet<AuditLog> AuditLog { get; set; }

    }
}
