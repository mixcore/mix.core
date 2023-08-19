using Microsoft.Extensions.Options;
using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Entities.Queue.EntityConfigurations;
using Mix.Database.Services;
using Mix.Heart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue
{
    public sealed class MixQueueDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MixFileHelper.CreateFolderIfNotExist($"{MixFolders.MixQueueLogFolder}/{DateTime.Now.ToString("dd_MM")}");
            string cnn = $"Data Source={MixFolders.MixQueueLogFolder}/{DateTime.Now.ToString("dd_MM")}/queuelog_{DateTime.Now.ToString("dd_MM_yyyy")}.sqlite";
            optionsBuilder.UseSqlite(cnn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MixQueueMessageConfiguration());
        }

        public DbSet<MixQueueMessageLog> MixQueueMessage { get; set; }
    }
}
