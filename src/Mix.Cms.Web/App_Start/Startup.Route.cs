// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using RewriteRules;
using System.IO;

namespace Mix.Cms.Web
{
    public static class MixRoutesServiceCollectionExtensions
    {
        public static IApplicationBuilder UseMixRoutes(this IApplicationBuilder app)
        {
            app.UseEndpoints(routes =>
            {
                routes.MapDefaultControllerRoute();
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{controller}/{id}/{seoName}");
                routes.MapDynamicControllerRoute<TranslationTransformer>(
                    pattern: "{culture}/{controller}/{id}/{seoName}");
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