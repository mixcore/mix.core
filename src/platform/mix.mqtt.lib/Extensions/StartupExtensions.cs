using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Mqtt.Lib.Controllers;
using Mix.Mqtt.Lib.Service;
using MQTTnet.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mqtt.Lib.Extensions
{
    public static class StartupExtensions
    {
        public static IHostApplicationBuilder AddMqttServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHostedService<MqttServerHostedService>();

            builder.Services.AddHostedMqttServer(
                optionsBuilder =>
                {
                    optionsBuilder.WithDefaultEndpoint();
                });
            builder.Services.AddMqttConnectionHandler();
            builder.Services.AddConnections();
            return builder;
        }

        public static IApplicationBuilder UseAspNetMqttServer(this IApplicationBuilder app, bool isDevelop)
        //MqttController mqttController)
        {
            app.UseMqttServer(
                server =>
                {
                    /*
                     * Attach event handlers etc. if required.
                     */

                });
            return app;
        }

        public static IEndpointRouteBuilder MapMqttEndpoints(this IEndpointRouteBuilder endpoints, bool isDevelop)
        //MqttController mqttController)
        {
            endpoints.MapConnectionHandler<MqttConnectionHandler>(
                      "/mqtt",
                      httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                          protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            return endpoints;
        }
    }
}
