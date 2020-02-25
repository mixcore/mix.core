// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//using Messenger.Lib.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        public void ConfigureSignalRServices(IServiceCollection services)
        {
            //services.BuildServiceProvider();
            //services.AddSignalR();
        }

        public void ConfigurationSignalR(IApplicationBuilder app)
        {
            //app.UseSignalR(routes => routes.MapHub<MessengerHub>("Messenger"));
        }
    }
}