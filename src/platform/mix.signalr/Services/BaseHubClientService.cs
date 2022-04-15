using Microsoft.AspNetCore.SignalR.Client;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using System;
using System.Threading.Tasks;

namespace Mix.SignalR.Services
{
    public abstract class BaseHubClientService
    {
        protected HubConnection connection;
        protected string hubName;
        public bool IsStarted => connection != null;

        public BaseHubClientService(string hub)
        {

        }
        public async Task SendMessageAsync(string message)
        {
            try
            {
                if (connection == null && !string.IsNullOrEmpty(MixEndpointService.Instance.Messenger))
                {
                    Init();
                }

                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                }
                await connection.InvokeAsync(HubMethods.SendMessage, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Init()
        {
            string endpoint = $"{MixEndpointService.Instance.Messenger}{hubName}";
            connection = new HubConnectionBuilder()
               .WithUrl(endpoint)
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

        }

        protected abstract void HandleMessage<T>(SignalRMessageModel<T> message);
    }
}
