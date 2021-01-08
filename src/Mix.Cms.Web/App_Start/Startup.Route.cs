﻿// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using RewriteRules;
using System.IO;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Web
{
    public static class MixRoutesServiceCollectionExtensions
    {
        public static IApplicationBuilder UseMixRoutes(this IApplicationBuilder app)
        {
            if (MixService.GetConfig<bool>("IsRewrite"))
            {
                using (StreamReader apacheModRewriteStreamReader =
            File.OpenText("ApacheModRewrite.txt"))
                using (StreamReader iisUrlRewriteStreamReader =
                    File.OpenText("IISUrlRewrite.xml"))
                {
                    var options = new RewriteOptions()
                        .AddApacheModRewrite(apacheModRewriteStreamReader)
                        .AddIISUrlRewrite(iisUrlRewriteStreamReader)
                        .Add(MethodRules.RedirectXMLRequests);
                    //.Add(new RedirectImageRequests(".png", "/png-images"))
                    //.Add(new RedirectImageRequests(".jpg", "/jpg-images"));

                    app.UseRewriter(options);
                }
                //    app.Run(context => context.Response.WriteAsync(
                //$"Rewritten or Redirected Url: " +
                //$"{context.Request.Path + context.Request.QueryString}"));
            }
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{alias}");
                routes.MapControllerRoute(
                   name: "page",
                   pattern: "{controller=Page}/{culture=" + MixService.GetConfig<string>(AppSettingKeywords.DefaultCulture) + "}/{seoName}"); routes.MapControllerRoute(
                    name: "vue",
                    pattern: "{controller=Vue}/{culture=" + MixService.GetConfig<string>(AppSettingKeywords.DefaultCulture) + "}/{seoName}");
                routes.MapControllerRoute(
                    name: "file",
                    pattern: "{culture=" + MixService.GetConfig<string>(AppSettingKeywords.DefaultCulture) + "}/portal/file");
                routes.MapControllerRoute(
                    name: "post",
                    pattern: "{culture=" + MixService.GetConfig<string>(AppSettingKeywords.DefaultCulture) + "}/post/{id}/{seoName}");
            });
            return app;
        }
    }
}