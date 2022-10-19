using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Heart.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Base
{
    public abstract class BaseDbContext : DbContext
    {
        protected static DatabaseService _databaseService;
        protected readonly string _connectionStringName;
        protected Type _dbContextType;

        public BaseDbContext(DatabaseService databaseService, string connectionStringName)
        {
            _databaseService = databaseService;
            _connectionStringName = connectionStringName;
        }

        protected override void OnConfiguring(
           DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = _databaseService.GetConnectionString(_connectionStringName);
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
            modelBuilder.ApplyAllConfigurations(_databaseService, _dbContextType.Assembly, $"{_dbContextType.Namespace}.EntityConfigurations");
        }
    }
}
