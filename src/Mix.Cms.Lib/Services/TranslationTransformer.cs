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
            string notTransformPattern = @"^(.*)\.(xml|json|html|css|js|map|jpg|png|gif|jpeg|svg|map|ico|webmanifest|woff|woff2|ttf|eot)$";
            Regex reg = new Regex(notTransformPattern);
            if (reg.IsMatch(httpContext.Request.Path.Value))
            {
                return ValueTask.FromResult(values);
            }
            reg = new Regex(@"^\/(init|security|portal|api|vue|error|swagger|graphql|ReDoc|OpenAPI)(\/.+)?");
            if (reg.IsMatch(httpContext.Request.Path.Value.ToLower()))
            {
                return ValueTask.FromResult(values);
            }

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
                values["controller"] = "home";
                values["action"] = "Index";
                values["seoName"] = path;
                return ValueTask.FromResult(values);
            }

            var currentController = GetRouteValue(values, keys, ref keyIndex);
            var controller = MixService.Translate(currentController, language, currentController);
            if (!IsValidController(controller))
            {
                controller = "home";
                keyIndex -= 1;
                values["seoName"] = string.Join('/', values.Values.Skip(keyIndex));
                values["culture"] = language;
                values["action"] = "Index";
                values["controller"] = controller;
            }
            else
            {
                if (keys.Count > 2)
                {
                    values["id"] = GetRouteValue(values, keys, ref keyIndex);
                }
                values["controller"] = controller;
                values["keyword"] = GetRouteValue(values, keys, ref keyIndex);
            }
            values["action"] = "Index";
            
            return ValueTask.FromResult(values);
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
