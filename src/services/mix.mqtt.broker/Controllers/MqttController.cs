using Microsoft.AspNetCore.Mvc;
using MQTTnet.Server;

namespace Mix.MQTT.Broker.Controllers
{
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