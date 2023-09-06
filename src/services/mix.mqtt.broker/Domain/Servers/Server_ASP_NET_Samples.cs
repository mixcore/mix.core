using Mix.MQTT.Broker;
using MQTTnet.AspNetCore;
using MQTTnet.Server;

namespace mix.mqtt.broker.Domain.Servers
{
    public static class Server_ASP_NET_Samples
    {
        public static Task Start_Server_With_WebSockets_Support()
        {
            /*
             * This sample starts a minimal ASP.NET Webserver including a hosted MQTT server.
             */
            var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseKestrel(
                            o =>
                            {
                                // This will allow MQTT connections based on TCP port 1883.
                                o.ListenAnyIP(1883, l => l.UseMqtt());

                                // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
                                // See code below for URI configuration.
                                o.ListenAnyIP(5000); // Default HTTP pipeline
                            });

                        webBuilder.UseStartup<Startup>();
                    });

            return host.RunConsoleAsync();
        }

        sealed class MqttController
        {
            public MqttController()
            {
                // Inject other services via constructor.
            }

            public Task OnClientConnected(ClientConnectedEventArgs eventArgs)
            {
                Console.WriteLine($"Client '{eventArgs.ClientId}' connected.");
                return Task.CompletedTask;
            }


            public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
            {
                Console.WriteLine($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
                return Task.CompletedTask;
            }
        }

        
    }
}
