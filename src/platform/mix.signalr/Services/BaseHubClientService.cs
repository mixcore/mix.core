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
            hubName = hub;
        }

        public Task SendMessageAsync<T>(SignalRMessageModel<T> message)
        {
            return SendMessageAsync(message.ToString());
        }

            public async Task SendMessageAsync(string message)
        {
            if (connection == null && !string.IsNullOrEmpty(MixEndpointService.Instance.Messenger))
            {
                Init();
            }
            while (connection.State != HubConnectionState.Connected)
            {
                try
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            await connection.InvokeAsync(HubMethods.SendMessage, message);
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
