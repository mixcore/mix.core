using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Mix.Cms.Lib.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Services
{
    public class TranslationTransformer : DynamicRouteValueTransformer
    {
        public TranslationTransformer()
        {
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (MixService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                return ValueTask.FromResult(values);
            }

            RouteValueDictionary result = values;

            var keys = values.Keys.ToList();

            var language = (string)values[keys[0]];
            var keyIndex = 1;
            if (!MixService.Instance.CheckValidCulture(language))
            {
                language = MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
                keyIndex -= 1;
            }

            var path = string.Join('/', values.Values.Skip(keyIndex));
            if (MixService.Instance.CheckValidAlias(language, path))
            {
                result["controller"] = "home";
                result["action"] = "Index";
                result["seoName"] = path;
                return ValueTask.FromResult(result);
            }

            
            string notTransformPattern = @"^(.*)\.(xml|json|html|css|js|map|jpg|png|gif|jpeg|svg|map|ico|webmanifest|woff|woff2|ttf|eot)$";
            Regex reg = new Regex(notTransformPattern);

            if (reg.IsMatch(httpContext.Request.Path.Value))
            {
                return ValueTask.FromResult(values);
            }

            var currentController = GetRouteValue(values, keys, ref keyIndex);
            var controller = MixService.Translate(currentController, language, currentController);
            if (!IsValidController(controller))
            {
                controller = "home";
                keyIndex -= 1;
                result["keyword"] = keyIndex < keys.Count ? string.Join('/', values.Values.Skip(keyIndex + 1)) : string.Empty;
                result["seoName"] = keys.Count > keyIndex ? (string)values[keys[keyIndex]] : string.Empty;
                result["culture"] = language;
                result["action"] = "Index";
                result["controller"] = controller;
            }
            else
            {
                if (keys.Count > 2)
                {
                    result["id"] = GetRouteValue(values, keys, ref keyIndex);
                }
                result["controller"] = controller;
                result["keyword"] = GetRouteValue(values, keys, ref keyIndex);
            }
            result["action"] = "Index";

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
