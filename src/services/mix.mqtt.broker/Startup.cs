using mix.mqtt.broker.Controllers;
using MQTTnet.AspNetCore;

namespace Mix.MQTT.Broker
{
    sealed class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, MqttController mqttController)
        {
            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
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

                    server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                    server.ClientConnectedAsync += mqttController.OnClientConnected;
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedMqttServer(
                optionsBuilder =>
                {
                    optionsBuilder.WithDefaultEndpoint();
                });

            services.AddMqttConnectionHandler();
            services.AddConnections();

            services.AddSingleton<MqttController>();
        }
    }
}
