using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Policies;
using Mix.Shared.Models.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddMixResponseCaching(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder => builder.Cache());
                options.AddPolicy("OutputCacheWithAuthPolicy", OutputCacheWithAuthPolicy.Instance);
            });
            builder.Services.AddResponseCaching();
            builder.Services.AddControllers(
                opt =>
                {
                    int responseCache = builder.Configuration.GetGlobalConfiguration<int>(nameof(GlobalSettingsModel.ResponseCache));
                    opt.CacheProfiles.Add("Default",
                        new CacheProfile()
                        {
                            Duration = responseCache > 0
                                        ? responseCache
                                        : 0,
                            VaryByHeader = "User-Agent",
                            Location = responseCache > 0
                                        ? ResponseCacheLocation.Any
                                        : ResponseCacheLocation.None,
                            NoStore = responseCache > 0
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
            //    if (GlobalSettingsService.Instance.ResponseCache > 0)
            //    {
            //        context.Response.GetTypedHeaders().CacheControl =
            //            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //            {
            //                Public = true,
            //                NoCache = false,
            //                SharedMaxAge = TimeSpan.FromSeconds(GlobalSettingsService.Instance.ResponseCache),
            //                MaxAge = TimeSpan.FromSeconds(GlobalSettingsService.Instance.ResponseCache),

            //            };
            //    }
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "User-Agent" };
            //    await next();
            //});
        }
    }
}