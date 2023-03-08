using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Mixdb.Entities;
using Mix.Services.Graphql.Lib.Interfaces;
using Mix.Services.Graphql.Lib.Models;
using Mix.Services.Graphql.Lib.Queries;

namespace Mix.Services.Graphql.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixGraphQL(this IServiceCollection services)
        {
            services.AddDbContext<MixDbDbContext>();
            services.AddGraphQLServer()
                    .AddQueryType<Query>();


            //services.AddScoped<ITableNameLookup, TableNameLookup>();
            //services.AddScoped<IDatabaseMetadata, DatabaseMetadata>();
            //services.AddScoped((resolver) =>
            //{
            //    var dbContext = resolver.GetRequiredService<MixDbDbContext>();
            //    var metaDatabase = resolver.GetRequiredService<IDatabaseMetadata>();
            //    var tableNameLookup = resolver.GetRequiredService<ITableNameLookup>();
            //    //var schema = new Schema { Query = new GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
            //    var schema = SchemaBuilder.New();
            //    schema.AddQueryType<GraphQLQuery>();
            //    return schema;
            //});
            ////add schema provider so we don't need to create it everytime
            //// Also for this demo we expose all fields on MyDbContext.See below for details on building custom fields etc.
            //services.AddSingleton(SchemaBuilder.New().BindRuntimeType< MixDbDbContext>());
            return services;
        }

        public static void UseMixGraphQL(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
