using Microsoft.AspNetCore.SignalR.Client;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace Mix.SignalR.Services
{
    public abstract class BaseHubClientService
    {
        protected HubConnection connection;
        protected string hubName;
        protected string _accessToken;
        public bool IsStarted => connection != null;

        public BaseHubClientService(string hub)
        {
            hubName = hub;
        }

        public Task SendMessageAsync(string title, string description, object data, MessageType messageType = MessageType.Info)
        {
            var msg = new SignalRMessageModel(data)
            {
                Title = title,
                Message = description,
                Type = messageType
            };
            return SendMessageAsync(msg);
        }

        public async Task SendMessageAsync(SignalRMessageModel message)
        {
            if (connection == null && !string.IsNullOrEmpty(MixEndpointService.Instance.Messenger))
            {
                Init();
            }
            while (connection.State != HubConnectionState.Connected)
            {
                try
                {
                    await Task.Delay(RandomNumberGenerator.GetInt32(0, 5) * 1000);
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
               .WithUrl(endpoint, options =>
               {
                   options.AccessTokenProvider = () => Task.FromResult(_accessToken);
               })
               .WithAutomaticReconnect()
               .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(RandomNumberGenerator.GetInt32(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On(HubMethods.ReceiveMethod, (SignalRMessageModel message) =>
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

        protected abstract void HandleMessage(SignalRMessageModel message);
    }
}
