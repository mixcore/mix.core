using Mix.Database.Entities.Queue.EntityConfigurations;
using Mix.Heart.Services;

namespace Mix.Database.Entities.Queue
{
    public sealed class MixQueueDbContext : DbContext
    {
        private string _folder = DateTime.Now.ToString("MM_yyyy");
        private string _cnn;
        public MixQueueDbContext()
        {
            _cnn = $"Data Source={MixFolders.MixQueueLogFolder}/{_folder}/queuelog_{DateTime.Now.ToString("dd_MM_yyyy")}.sqlite";
        }
        public MixQueueDbContext(DateTime date)
        {
            _folder = date.ToString("MM_yyyy");
            _cnn = $"Data Source={MixFolders.MixQueueLogFolder}/{_folder}/queuelog_{date.ToString("dd_MM_yyyy")}.sqlite";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MixFileHelper.CreateFolderIfNotExist($"{MixFolders.MixQueueLogFolder}/{_folder}");
            optionsBuilder.UseSqlite(_cnn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MixQueueMessageConfiguration());
        }

        public DbSet<MixQueueMessageLog> MixQueueMessage { get; set; }
    }
}
