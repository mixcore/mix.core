using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Policies;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddMixResponseCaching(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder => builder.Cache());
                options.AddPolicy("OutputCacheWithAuthPolicy", OutputCacheWithAuthPolicy.Instance);
            });
            services.AddResponseCaching();
            services.AddControllers(
                opt =>
                {
                    opt.CacheProfiles.Add("Default",
                        new CacheProfile()
                        {
                            Duration = GlobalConfigService.Instance.ResponseCache > 0
                                        ? GlobalConfigService.Instance.ResponseCache
                                        : 0,
                            VaryByHeader = "User-Agent",
                            Location = GlobalConfigService.Instance.ResponseCache > 0
                                        ? ResponseCacheLocation.Any
                                        : ResponseCacheLocation.None,
                            NoStore = GlobalConfigService.Instance.ResponseCache > 0
                                        ? false : true
                        });
                });
        }
        // Must call after use cors
        public static void UseMixResponseCaching(this IApplicationBuilder app)
        {
            app.UseOutputCache();
            app.UseResponseCaching();
            //app.Use(async (context, next) =>
            //{
            //    if (GlobalConfigService.Instance.ResponseCache > 0)
            //    {
            //        context.Response.GetTypedHeaders().CacheControl =
            //            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //            {
            //                Public = true,
            //                NoCache = false,
            //                SharedMaxAge = TimeSpan.FromSeconds(GlobalConfigService.Instance.ResponseCache),
            //                MaxAge = TimeSpan.FromSeconds(GlobalConfigService.Instance.ResponseCache),

            //            };
            //    }
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "User-Agent" };
            //    await next();
            //});
        }
    }
}