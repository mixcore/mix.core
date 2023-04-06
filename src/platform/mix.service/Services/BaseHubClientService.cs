using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Mix.Constant.Constants;
using Mix.Service.Models;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using System;
using System.Threading.Tasks;
using Mix.Shared.Extensions;
using System.Drawing.Printing;

namespace Mix.Service.Services
{
    public abstract class BaseHubClientService
    {
        protected HubConnection Connection;
        protected string HubName;
        protected string AccessToken;
        public bool IsStarted => Connection != null;
        protected readonly MixEndpointService MixEndpointService;
        protected BaseHubClientService(string hub, MixEndpointService mixEndpointService)
        {
            HubName = hub;
            MixEndpointService = mixEndpointService;
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
            if (!string.IsNullOrEmpty(MixEndpointService.Messenger))
            {
                while (Connection == null)
                {
                    Init();
                    if (Connection == null)
                    {
                        await Task.Delay(5000);
                    }
                }

                while (Connection != null && Connection.State != HubConnectionState.Connected)
                {
                    try
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        await Connection.StartAsync();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                await Connection.InvokeAsync(HubMethods.SendMessage, message);
            }
            else
            {
                Console.WriteLine("Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
            }

        }
        private void Init()
        {
            if (!string.IsNullOrEmpty(MixEndpointService.Messenger))
            {

                string endpoint = $"{MixEndpointService.Messenger}{HubName}";
                Connection = new HubConnectionBuilder()
                   .WithUrl(endpoint, options =>
                   {
                       options.AccessTokenProvider = () => Task.FromResult(AccessToken);
                   })
                   .WithAutomaticReconnect()
                   .Build();
                Connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await Connection.StartAsync();
                };

                Connection.On(HubMethods.ReceiveMethod, (SignalRMessageModel message) =>
                {
                    HandleMessage(message);
                });

                Connection.Reconnecting += error =>
                {
                    Console.WriteLine(Connection.State);

                    // Notify users the connection was lost and the client is reconnecting.
                    // Start queuing or dropping messages.

                    return Task.CompletedTask;
                };

            }
        }

        protected abstract void HandleMessage(SignalRMessageModel message);
    }
}
