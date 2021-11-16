using Microsoft.AspNetCore.Mvc.Routing;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Lib.Services;
using Mix.Shared.Services;


namespace Mixcore.Domain.Services
{
    // Ref: https://www.strathweb.com/2019/08/dynamic-controller-routing-in-asp-net-core-3-0/
    public class TranslationTransformer : DynamicRouteValueTransformer
    {
        private readonly MixDatabaseService _databaseService;
        private readonly CultureService _cultureService;

        public TranslationTransformer(
            IConfiguration configuration)
        {
            _databaseService = new(configuration);
            MixCmsContext ctx = new MixCmsContext(_databaseService);
            _cultureService = new(ctx);
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(
            HttpContext httpContext, RouteValueDictionary values)
        {
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                return ValueTask.FromResult(values);
            }

            RouteValueDictionary result = values;

            var keys = values.Keys.ToList();

            var language = (string)values[keys[0]];
            var keyIndex = 1;
            if (_cultureService.CheckValidCulture(language))
            {
                language = GlobalConfigService.Instance.AppSettings.DefaultCulture;
                keyIndex -= 1;
                result["controller"] = "home";
                result["culture"] = language;
                result["action"] = "Index";
                result["keyword"] = "aaa";
                result["seoName"] = "test";
            }

            return ValueTask.FromResult(result);
        }

        private string GetRouteValue(RouteValueDictionary values, List<string> keys, ref int keyIndex)
        {
            string value = keys.Count > keyIndex
               ? values[keys[keyIndex]]?.ToString()
               : string.Empty;
            keyIndex += 1;
            return value;
        }

        private bool IsValidController(string controller)
        {
            string[] controllers = { "post", "page", "module", "data" };
            return controllers.Contains(controller?.ToLower());
        }
    }
}
