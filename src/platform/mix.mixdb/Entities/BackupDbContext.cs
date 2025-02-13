using Microsoft.EntityFrameworkCore;

namespace Mix.Mixdb.Entities
{
    public class BackupDbContext : DbContext
    {
        private readonly string _cnn;
        public BackupDbContext(string connectionString)
        {
            _cnn = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_cnn);
        }
    }
}
