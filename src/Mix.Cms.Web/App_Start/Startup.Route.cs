// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Mix.Cms.Hub;
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
                    app.UseRewriter(options);
                }

                /* Uncomment for debug rewrite route url 
                 * 
                app.Run(context => context.Response.WriteAsync(
                $"Rewritten or Redirected Url: " +
                $"{context.Request.Path + context.Request.QueryString}"));

                */
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{area:exists}/{controller=Portal}/{action=Init}");
                endpoints.MapControllerRoute(
                    name: "alias",
                    pattern: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                endpoints.MapControllerRoute(
                   name: "page",
                   pattern: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                endpoints.MapControllerRoute(
                    name: "file",
                    pattern: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/portal/file");
                endpoints.MapControllerRoute(
                    name: "post",
                    pattern: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/post/{id}/{seoName}");
                
                endpoints.MapHub<PortalHub>("/portalhub");

                endpoints.MapHub<ServiceHub>("/servicehub");

            });
            //app.UseMvc(routes =>
            //{
            //    // uncomment the following line to Work-around for #1175 in beta1
            //    routes.EnableDependencyInjection();

            //    // and this line to enable OData query option, for example $filter
            //    routes.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
            //    //routes.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());

            //});
        }

    }
}
