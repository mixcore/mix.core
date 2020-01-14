// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using RewriteRules;
using System.IO;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        protected void ConfigRoutes(IApplicationBuilder app)
        {
            if (MixService.GetConfig<bool>("IsRewrite"))
            {
                using (StreamReader apacheModRewriteStreamReader =
            File.OpenText("ApacheModRewrite.txt"))
                using (StreamReader iisUrlRewriteStreamReader =
                    File.OpenText("IISUrlRewrite.xml"))
                {
                    var options = new RewriteOptions()
                        .AddRedirect("redirect-rule/(.*)", "redirected/$1")
                        .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2",
                            skipRemainingRules: true)
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
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "areaRoute",                    
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{area:exists}/{controller=Portal}/{action=Init}");
                routes.MapRoute(
                    name: "Alias",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                routes.MapRoute(
                   name: "Page",
                   template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                routes.MapRoute(
                    name: "File",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/portal/file");
                routes.MapRoute(
                    name: "Post",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/post/{id}/{seoName}");
                routes.MapRoute(
                    name: "Module",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/module/{id}/{seoName}");

                // uncomment the following line to Work-around for #1175 in beta1
                routes.EnableDependencyInjection();

                // and this line to enable OData query option, for example $filter
                routes.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                //routes.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());


            });
        }

    }
}
