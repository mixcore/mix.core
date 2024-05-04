using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;

namespace Mix.Lib.Policies
{
    public class OutputCacheWithAuthPolicy : IOutputCachePolicy
    {
        public static readonly OutputCacheWithAuthPolicy Instance = new();
        private OutputCacheWithAuthPolicy() { }

        ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            var attemptOutputCaching = AttemptOutputCaching(context);
            context.EnableOutputCaching = true;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;
            // Vary by any query by default
            context.CacheVaryByRules.QueryKeys = "*";
            return ValueTask.CompletedTask;
        }
        private static bool AttemptOutputCaching(OutputCacheContext context)
        {
            // Check if the current request fulfills the requirements to be cached
            var request = context.HttpContext.Request;

            // Verify the method, we only cache get and head verb
            if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
            {
                return false;
            }
            // we comment out below code to cache authorization response.
            // Verify existence of authorization headers
            //if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) || request.HttpContext.User?.Identity?.IsAuthenticated == true)
            //{
            //    return false;
            //}
            return true;
        }
        ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            return ValueTask.CompletedTask;
        }

        ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            var response = context.HttpContext.Response;
            context.AllowCacheStorage = true;

            return ValueTask.CompletedTask;
        }

    }
}
