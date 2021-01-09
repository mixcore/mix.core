using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Services;
using MySql.Data.MySqlClient;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MySqlMixCmsContext : MixCmsContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MySqlMixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }

        public MySqlMixCmsContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //define the database to use
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                var provider = Enum.Parse<MixEnums.DatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
                switch (provider)
                {
                    case MixEnums.DatabaseProvider.MSSQL:
                        optionsBuilder.UseSqlServer(cnn);
                        break;
                    case MixEnums.DatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn);
                        break;
                    case MixEnums.DatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(cnn);
                        break;
                    default:
                        break;
                }
            }
        }

        //Ref https://github.com/dotnet/efcore/issues/10169
        public override void Dispose()
        {
            var provider = Enum.Parse<MixEnums.DatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
            switch (provider)
            {
                case MixEnums.DatabaseProvider.MSSQL:
                    SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                    break;
                case MixEnums.DatabaseProvider.MySQL:
                    MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                    break;
                case MixEnums.DatabaseProvider.PostgreSQL:
                    Npgsql.NpgsqlConnection.ClearPool((Npgsql.NpgsqlConnection)Database.GetDbConnection());
                    break;

            }
            base.Dispose();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly, 
                "Mix.Cms.Lib.Models.EntityConfigurations.MySQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}