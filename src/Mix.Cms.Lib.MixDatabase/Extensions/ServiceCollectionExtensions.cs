using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Mix.Cms.Lib.MixDatabase.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.Enums;
using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace Mix.Cms.Lib.MixDatabase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixDbRepository(this IServiceCollection services, Assembly assembly)
        {
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                var provider = MixService.GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
                var connectionType = GetDbConnectionType(provider);
                var repositoryType = typeof(RepositoryReadBase<>);
                services.AddScoped(repositoryType.MakeGenericType(connectionType));
            }
            return services;
        }

        static Type GetDbConnectionType(MixDatabaseProvider dbProvider)
        {
            switch (dbProvider)
            {
                case MixDatabaseProvider.MSSQL:
                    return typeof(SqlConnection);
                case MixDatabaseProvider.MySQL:
                    return typeof(MySqlConnection);
                case MixDatabaseProvider.PostgreSQL:
                    return typeof(NpgsqlConnection);
                case MixDatabaseProvider.SQLITE:
                    return typeof(SqliteConnection);
                default:
                    return typeof(SqliteConnection);
            }
        }
    }
}
