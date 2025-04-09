using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Mqtt.Lib.Controllers;
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
            builder.Services.AddHostedMqttServer(
               optionsBuilder =>
               {
                   optionsBuilder.WithDefaultEndpoint();
               });

            builder.Services.AddMqttConnectionHandler();
            builder.Services.AddMqttWebSocketServerAdapter();
            builder.Services.AddConnections();

            builder.Services.AddSingleton<MqttController>();
            return builder;
        }

        public static IApplicationBuilder UseAspNetMqttServer(this IApplicationBuilder app, bool isDevelop) 
            //MqttController mqttController)
        {
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapMqtt("/mqtt");
                    endpoints.MapConnectionHandler<MqttConnectionHandler>(
                        "/mqtt",
                        httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
                });
            app.UseMqttServer(
                server =>
                {
                    /*
                     * Attach event handlers etc. if required.
                     */

                    //server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                    //server.ClientConnectedAsync += mqttController.OnClientConnected;
                })
                ;
            return app;
        }
    }
}
