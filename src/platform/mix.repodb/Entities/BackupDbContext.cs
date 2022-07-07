using Microsoft.EntityFrameworkCore;

namespace Mix.RepoDb.Entities
{
    public class BackupDbContext : DbContext
    {
        private string _cnn;
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
