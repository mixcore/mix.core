using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Shared.Models.Configurations;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;

// Ref: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRateLimiter(this IServiceCollection services, IConfiguration configuration)
        {
            var myOptions = configuration.GetSection(MixRateLimitConfigurations.SectionName).Get<MixRateLimitConfigurations>()!;
            var userPolicyName = "user";
            services.AddRateLimiter(limiterOptions =>
            {
                limiterOptions.OnRejected = (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                        .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                        .LogWarning("OnRejected: {GetUserEndPoint}", GetUserEndPoint(context.HttpContext));

                    return new ValueTask();
                };

                limiterOptions.AddPolicy(userPolicyName, context =>
                {
                    var username = "anonymous user";
                    if (context.User.Identity?.IsAuthenticated is true)
                    {
                        username = context.User.ToString()!;
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(username,
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = myOptions.PermitLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = myOptions.QueueLimit,
                            Window = TimeSpan.FromSeconds(myOptions.Window),
                            SegmentsPerWindow = myOptions.SegmentsPerWindow
                        });

                });

                limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                {
                    IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

                    if (!IPAddress.IsLoopback(remoteIpAddress!))
                    {
                        return RateLimitPartition.GetTokenBucketLimiter
                        (remoteIpAddress!, _ =>
                            new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = myOptions.TokenLimit2,
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                QueueLimit = myOptions.QueueLimit,
                                ReplenishmentPeriod = TimeSpan.FromSeconds(myOptions.ReplenishmentPeriod),
                                TokensPerPeriod = myOptions.TokensPerPeriod,
                                AutoReplenishment = myOptions.AutoReplenishment
                            });
                    }

                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                });
            });
            
            return services;
        }
        static string GetUserEndPoint(HttpContext context) =>
   $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
   + $" {context.Connection.RemoteIpAddress}";

        public static IApplicationBuilder UseMixRateLimiter(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
            return app;
        }
    }
}
