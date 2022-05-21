using Mix.Database.Services;


namespace Mix.Database.Entities.AuditLog
{
    public class AuditLogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = $"Data Source=MixContent/auditlog_{DateTime.Now.ToString("MM_yyyy")}.db";
            optionsBuilder.UseSqlite(cnn);
        }

        public virtual DbSet<AuditLog> AuditLog { get; set; }

    }
}
