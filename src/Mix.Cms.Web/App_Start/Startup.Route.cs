// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Web
{
    public static class MixRoutesServiceCollectionExtensions
    {
        public static IApplicationBuilder UseMixRoutes(this IApplicationBuilder app)
        {
            string notStartWithPattern = "regex(^(?!(init|security|portal|api|vue|error|swagger|graphql|ReDoc|OpenAPI|.+Hub))(.+)$)";
            app.UseEndpoints(routes =>
            {
                routes.MapDefaultControllerRoute();
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{seoName:" + notStartWithPattern + "}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture:" + notStartWithPattern + "}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{controller:" + notStartWithPattern + "}/{id}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture:" + notStartWithPattern + "}/{controller}/{id}/{seoName}");
            });
            app.MapWhen(
               context =>
               {
                   var path = context.Request.Path.Value.ToLower();
                   return
                       path.StartsWith("/mix-app") ||
                       path.StartsWith("/mix-content");
               },
               config => config.UseStaticFiles());
            return app;
        }
    }
}