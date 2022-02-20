using Newtonsoft.Json.Converters;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtension
    {
        #region Ocelot

        public static void AddMixOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
        }

        public static void UseMixOcelot(this IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseOcelot().Wait();
        }
        #endregion

        #region Routes

        public static void AddMixRoutes(this IServiceCollection services)
        {
            services.AddControllersWithViews()
              .AddRazorRuntimeCompilation()
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.Converters.Add(new StringEnumConverter());
              });

            services.AddSingleton<TranslationTransformer>();
        }

        public static IApplicationBuilder UseMixRoutes(this IApplicationBuilder app)
        {
            string notStartWithPattern = "regex(^(?!(init|page|post|security|portal|api|vue|error|swagger|graphql|ReDoc|OpenAPI|.+Hub))(.+)$)";
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                //routes.MapDefaultControllerRoute();
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{seoName:" + notStartWithPattern + "}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture:" + notStartWithPattern + "}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{controller:" + notStartWithPattern + "}/{id}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture:" + notStartWithPattern + "}/{controller}/{id}/{seoName}");
            });
            app.MapWhen(
               context =>
               {
                   var path = context.Request.Path.Value.ToLower();
                   return
                       path.StartsWith("/mix-app") ||
                       path.StartsWith("/mix-content");
               },
               config => config.UseStaticFiles());
            return app;
        }

        #endregion
    }
}
