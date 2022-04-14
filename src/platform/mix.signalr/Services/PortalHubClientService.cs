using Microsoft.AspNetCore.SignalR.Client;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using System;
using System.Threading.Tasks;

namespace Mix.SignalR.Services
{
    public class PortalHubClientService
    {
        protected HubConnection connection;
        private const string hubEndpoint = "https://localhost:5010";
        public PortalHubClientService()
        {
            StartAsync().GetAwaiter().GetResult();
        }
        public async Task StartAsync()
        {
            connection = new HubConnectionBuilder()
               .WithUrl($"{hubEndpoint}{HubEndpoints.PortalHub}")
               .WithAutomaticReconnect()
               .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On(HubMethods.ReceiveMethod, (SignalRMessageModel<string> message) =>
            {
                this.HandleMessage(message);
            });

            connection.Reconnecting += error =>
            {
                Console.WriteLine(connection.State);

                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.

                return Task.CompletedTask;
            };
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
            }
        }
        public async Task SendMessageAsync(string message)
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
            }
            await connection.InvokeAsync(HubMethods.SendMessage, message);
        }
        private void HandleMessage<T>(SignalRMessageModel<T> message)
        {
            Console.WriteLine(message);
        }
    }
}
