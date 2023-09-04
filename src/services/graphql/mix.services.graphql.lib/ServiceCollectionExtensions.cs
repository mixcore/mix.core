using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.Mixdb.Services;
using Mix.Services.Graphql.Lib.Entities;
using Mix.Services.Graphql.Lib.Interfaces;
using Mix.Services.Graphql.Lib.Models;

namespace Mix.Services.Graphql.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixGraphQL(this IServiceCollection services)
        {
            var dbService = services.GetService<DatabaseService>();
            services.TryAddSingleton<GraphQLDbContext>();
            services.TryAddSingleton<RuntimeDbContextService>();
            services.AddGraphQL(c =>
            {
                c.AddSystemTextJson();
                c.AddScopedSubscriptionExecutionStrategy();
            });

            services.TryAddSingleton<ITableNameLookup, TableNameLookup>();
            services.TryAddSingleton<ISchema>((resolver) =>
            {
                var runtimeDbContextService = resolver.GetRequiredService<RuntimeDbContextService>();
                var dbContext = runtimeDbContextService.GetMixDatabaseDbContext()!;
                var tableNameLookup = resolver.GetRequiredService<ITableNameLookup>();
                DatabaseMetadata metaDatabase = new(dbContext, tableNameLookup);
                var schema = new Schema { Query = new GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
                return schema;
            });
            return services;
        }

        public static void UseMixGraphQL(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGraphQL("graphql");
            endpoints.MapGraphQLVoyager("graph/ui/voyager");
            endpoints.MapGraphQLGraphiQL("graph/ui/graphiql");
            endpoints.MapGraphQLPlayground("graph/ui/playground");
            endpoints.MapGraphQLAltair("graph/ui/altair");
        }
    }
}
