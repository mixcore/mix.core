// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using System.Text;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        protected void ConfigRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{area:exists}/{controller=Portal}/{action=Init}");
                routes.MapRoute(
                    name: "alias",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                routes.MapRoute(
                   name: "page",
                   template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/{seoName}");
                routes.MapRoute(
                    name: "file",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/portal/file");
                routes.MapRoute(
                    name: "article",
                    template: "{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + "}/article/{seoName}");
                routes.MapRoute(
                    name: "product",
                    template: @"{culture=" + MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture) + @"}/product/{seoName}");
            });
        }

    }
}
