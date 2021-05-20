using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.MixDatabase.Repositories;
using RepoDb.Interfaces;
using RepoDb;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;

namespace Mix.Cms.Lib.MixDatabase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixDbRepository(this IServiceCollection services)
        {
            InitializeRepoDb();
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<MixDbRepository>();
            return services;
        }

        private static void InitializeRepoDb()
        {
            var provider = MixService.GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            switch (provider)
            {
                case MixDatabaseProvider.MSSQL:
                    SqlServerBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.MySQL:
                    MySqlBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.PostgreSQL:
                    PostgreSqlBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.SQLITE:
                    SqLiteBootstrap.Initialize();
                    break;
                default:
                    SqLiteBootstrap.Initialize();
                    break;
            }
        }
    }
}
