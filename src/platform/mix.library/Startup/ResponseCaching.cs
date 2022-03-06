using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        // Must call after use cors
        private static void UseMixResponseCaching(this IApplicationBuilder app)
        {
            int responseCache = GlobalConfigService.Instance.GetConfig(MixConfigurations.REPONSE_CACHE, 5);
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        NoCache = false,
                        SharedMaxAge = TimeSpan.FromSeconds(responseCache),
                        MaxAge = TimeSpan.FromSeconds(responseCache),

                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };
                await next();
            });
        }
    }
}