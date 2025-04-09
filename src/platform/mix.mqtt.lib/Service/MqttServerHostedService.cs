using Microsoft.Extensions.Hosting;
using Mix.Mqtt.Lib.Helpers;
using MQTTnet.Samples.Server;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _server.StopAsync();
            return base.StopAsync(cancellationToken);
        }
    }
}
