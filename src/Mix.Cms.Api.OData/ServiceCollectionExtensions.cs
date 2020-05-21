using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Cms.Api.OData
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixODataApi(this IServiceCollection services)
        {
            services.AddOData();
            return services;
        }
        public static IApplicationBuilder UseMixODataApi(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                // uncomment the following line to Work-around for #1175 in beta1
                routes.EnableDependencyInjection();

                //and this line to enable OData query option, for example $filter

                routes.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                //routes.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
            });
            return app;
        }
    }
}
