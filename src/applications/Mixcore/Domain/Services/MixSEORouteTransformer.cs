using Microsoft.AspNetCore.Mvc.Routing;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mixcore.Domain.Services
{
    // Ref: https://www.strathweb.com/2019/08/dynamic-controller-routing-in-asp-net-core-3-0/
    public class MixSEORouteTransformer : DynamicRouteValueTransformer
    {
        private readonly DatabaseService _databaseService;
        public MixSEORouteTransformer(
            IHttpContextAccessor httpContextAccessor)
        {
            _databaseService = new(httpContextAccessor);
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(
            HttpContext httpContext, RouteValueDictionary values)
        {
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                return ValueTask.FromResult(values);
            }

            RouteValueDictionary result = values;

            var keys = values.Keys.ToArray();

            var language = (string)values[keys[0]];
            string seoName = string.Empty;
            if (keys.Count() > 1)
            {
                seoName = string.Join('/', values.Values.Skip(1));
            }
            result["controller"] = "home";
            result["culture"] = language;
            result["action"] = "Index";
            result["seoName"] = seoName.TrimStart('/');

            return ValueTask.FromResult(result);
        }
    }
}
