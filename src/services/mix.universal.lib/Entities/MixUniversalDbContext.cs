using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;
using Mix.Heart.EntityFrameworkCore.Extensions;
using Mix.Heart.Enums;
using Mix.Universal.Lib.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.Entities
{
    public class MixUniversalDbContext: DbContext
    {
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<PortalApp> PortalApp { get; set; }

        private static DatabaseService _databaseService;
        public MixUniversalDbContext(
            DatabaseService databaseService)
                    : base()
        {
            _databaseService = databaseService;
        }
        protected override void OnConfiguring(
           DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                switch (_databaseService.DatabaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        optionsBuilder.UseSqlServer(cnn);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn, ServerVersion.AutoDetect(cnn));
                        break;

                    case MixDatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(cnn);
                        break;

                    case MixDatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(cnn);
                        break;

                    default:
                        break;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
