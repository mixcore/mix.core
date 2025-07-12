using Microsoft.Extensions.Hosting;
using Mix.Mqtt.Lib.Extensions;
using MQTTnet.Server;
using MQTTnet;

namespace Mix.Mqtt.Lib.Service
{
    public class MqttServerHostedService : BackgroundService
    {
        public MqttServerFactory mqttServerFactory;
        public MqttServer _server;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*
            * This sample starts a simple MQTT server and prints the logs to the output.
            *
            * IMPORTANT! Do not enable logging in live environment. It will decrease performance.
            *
            * See sample "Run_Minimal_Server" for more details.
            */
            try
            {
                var mqttServerFactory = new MqttServerFactory();
                var mqttServerOptions = new MqttServerOptionsBuilder()
                    .WithDefaultEndpoint() // This call disables the default unencrypted endpoint on port 1883
                                           //.WithEncryptedEndpoint()
                                           //.WithEncryptedEndpointPort(1883) // the secured port
                                           //.WithEncryptionCertificate(CertificateHelper.CreateSelfSignedCertificate("localhost", "1.3.6.1.5.5.7.3.1"))
                                           //.WithEncryptionSslProtocol(SslProtocols.Tls12)
                    .Build();
                _server = mqttServerFactory.CreateMqttServer(mqttServerOptions);
                await _server.StartAsync();
                Console.WriteLine("MQTT Server started");
                Console.WriteLine(mqttServerOptions.DefaultEndpointOptions.Port);
                Console.WriteLine(_server.IsStarted);
                //Console.WriteLine(_server.ServerSessionItems.DumpToConsole());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot start mqtt server");
                //ex.DumpToConsole();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _server?.StopAsync();
            return base.StopAsync(cancellationToken);
        }
    }
}
