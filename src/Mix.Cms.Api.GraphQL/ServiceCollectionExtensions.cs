//using EntityGraphQL.Schema;
//using GraphQL.Types;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Api.GraphQL.Infrastructure;
using Mix.Cms.Api.GraphQL.Infrastructure.Interfaces;
using Mix.Cms.Api.GraphQL.Infrastructure.Models;
//using Mix.Cms.Api.GraphQL.Infrastructure.Interfaces;
//using Mix.Cms.Api.GraphQL.Infrastructure.Models;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Api.GraphQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            //services.AddDbContext<MixCmsAccountContext>();
            //services.AddScoped<ITableNameLookup, TableNameLookup>();
            //services.AddScoped<IDatabaseMetadata, DatabaseMetadata>();
            //services.AddScoped((resolver) =>
            //{
            //    var dbContext = resolver.GetRequiredService<MixCmsAccountContext>();
            //    var metaDatabase = resolver.GetRequiredService<IDatabaseMetadata>();
            //    var tableNameLookup = resolver.GetRequiredService<ITableNameLookup>();
            //    var schema = new Schema { Query = new GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
            //    schema.Initialize();
            //    return schema;
            //});

            //services.AddDbContext<MixCmsContext>();
            // add schema provider so we don't need to create it everytime
            // Also for this demo we expose all fields on MyDbContext. See below for details on building custom fields etc.
            //services.AddSingleton(SchemaBuilder.FromObject<MixCmsContext>(true));
            return services;
        }
    }
}
