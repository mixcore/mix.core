using Newtonsoft.Json.Converters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtension
    {
        #region Routes

        public static void AddMixRoutes(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddSessionStateTempDataProvider()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.Converters.Add(new StringEnumConverter());
              });

            services.AddSingleton<MixSEORouteTransformer>();
        }

        public static void UseMixMVCEndpoints(this IEndpointRouteBuilder routes)
        {
            string notStartWithPattern = "regex(^(?!(mix-app|graph|app|init|page|post|security|portal|api|vue|error|swagger|graphql|ReDoc|OpenAPI|.+Hub))(.+)$)";
            //string urlPathPattern = @"regex((([A-z0-9\-\%]+\/)*[A-z0-9\-\%]+$)?)";
            
            routes.MapDynamicControllerRoute<MixSEORouteTransformer>(
                pattern: "{seoName:" + notStartWithPattern + "}");
            routes.MapDynamicControllerRoute<MixSEORouteTransformer>(
                pattern: "{culture:" + notStartWithPattern + "}/{seoName}");
            routes.MapDynamicControllerRoute<MixSEORouteTransformer>(
                pattern: "{controller:" + notStartWithPattern + "}/{id}/{seoName}");
            routes.MapDynamicControllerRoute<MixSEORouteTransformer>(
                pattern: "{culture:" + notStartWithPattern + "}/{controller}/{id}/{seoName}");
            //routes.MapDynamicControllerRoute<MixPortalRouteTransformer>(
            //    pattern: "portal-apps/{appFolder:" + urlPathPattern + "}/{param1?}/{param2?}/{param3?}/{param4?}");
            routes.MapFallbackToFile("/index.html");
        }

        #endregion
    }
}
