using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Postgresql.Infrastructures
{
    public static class NpgsqlConfigurationExtension
    {
        public static void AddPostgresDbContext(this IServiceCollection service, string connectionStrings)
        {
            service.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(connectionStrings));
        }
    }
}
