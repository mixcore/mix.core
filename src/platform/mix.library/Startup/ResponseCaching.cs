using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        // Must call after use cors
        public static void UseMixResponseCaching(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                if (GlobalConfigService.Instance.ResponseCache > 0)
                {
                    context.Response.GetTypedHeaders().CacheControl =
                        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                        {
                            Public = true,
                            NoCache = false,
                            SharedMaxAge = TimeSpan.FromSeconds(GlobalConfigService.Instance.ResponseCache),
                            MaxAge = TimeSpan.FromSeconds(GlobalConfigService.Instance.ResponseCache),

                        };
                }
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };
                await next();
            });
        }
    }
}