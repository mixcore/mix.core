using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Services.Compliance;
using Mix.Constant.Constants;

namespace Mix.Lib.Middlewares.Compliance
{
    public class ConsentCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ConsentCheckMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip consent checking for certain paths
            if (ShouldSkipConsentCheck(context.Request.Path))
            {
                await _next(context);
                return;
            }

            // Skip for non-authenticated users
            if (!context.User.Identity?.IsAuthenticated == true)
            {
                await _next(context);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var consentService = scope.ServiceProvider.GetService<IConsentService>();
                
                if (consentService != null)
                {
                    var tenantId = context.Session.GetInt32(MixRequestQueryKeywords.TenantId) ?? 1;
                    var userIdClaim = context.User.FindFirst("user_id")?.Value;
                    
                    if (Guid.TryParse(userIdClaim, out var userId))
                    {
                        // Check consent based on the endpoint being accessed
                        var requiredPurpose = GetRequiredPurposeForEndpoint(context.Request.Path);
                        
                        if (requiredPurpose.HasValue)
                        {
                            var hasConsent = await consentService.HasActiveConsent(tenantId, userId, requiredPurpose.Value);
                            
                            if (!hasConsent)
                            {
                                context.Response.StatusCode = 403;
                                context.Response.Headers.Add("X-Consent-Required", requiredPurpose.Value.ToString());
                                await context.Response.WriteAsync($"Consent required for purpose: {requiredPurpose.Value}");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Log error but don't block request - fail open for now
                // In production, you might want to fail closed for sensitive operations
            }

            await _next(context);
        }

        private bool ShouldSkipConsentCheck(string path)
        {
            var skipPaths = new[]
            {
                "/api/privacy/consent",
                "/api/privacy/dsr",
                "/api/auth",
                "/api/health",
                "/swagger",
                "/favicon.ico"
            };

            return skipPaths.Any(skipPath => path.StartsWith(skipPath, StringComparison.OrdinalIgnoreCase));
        }

        private int? GetRequiredPurposeForEndpoint(string path)
        {
            // This is a basic mapping - in practice, you'd want a more sophisticated
            // configuration system to map endpoints to required consent purposes
            
            var endpointPurposeMap = new Dictionary<string, int>
            {
                { "/api/marketing", 2 }, // Marketing Communications purpose
                { "/api/analytics", 3 }, // Analytics and Improvement purpose
                { "/api/user/profile", 1 }, // Account Management purpose
            };

            foreach (var mapping in endpointPurposeMap)
            {
                if (path.StartsWith(mapping.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return mapping.Value;
                }
            }

            return null; // No specific consent required
        }
    }

    public static class ConsentCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseConsentCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConsentCheckMiddleware>();
        }
    }
}