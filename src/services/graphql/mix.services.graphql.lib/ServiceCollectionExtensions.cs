using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Services;
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
            services.AddSingleton<GraphQLDbContext>();
            services.AddGraphQL(c =>
            {
                c.AddSystemTextJson();
                c.AddScopedSubscriptionExecutionStrategy();
            });

            services.AddSingleton<ITableNameLookup, TableNameLookup>();
            services.AddSingleton<IDatabaseMetadata, DatabaseMetadata>();
            services.AddSingleton<ISchema>((resolver) =>
            {
                var dbContext = resolver.GetRequiredService<GraphQLDbContext>();
                var metaDatabase = resolver.GetRequiredService<IDatabaseMetadata>();
                var tableNameLookup = resolver.GetRequiredService<ITableNameLookup>();
                var schema = new Schema { Query = new GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
                return schema;
            });
            return services;
        }

        public static void UseMixGraphQL(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("graphql");
                endpoints.MapGraphQLVoyager("ui/voyager");
                endpoints.MapGraphQLGraphiQL("ui/graphiql");
                endpoints.MapGraphQLPlayground("ui/playground");
                endpoints.MapGraphQLAltair("ui/altair");
            });
        }
    }
}
