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
using Microsoft.Extensions.Hosting;
using Mix.SignalR.Interfaces;
using Mix.Heart.Helpers;
using Mix.Heart.Extensions;

namespace Mix.Service.Services
{
    public abstract class BaseHubClientService : IHubClientService
    {
        public HubConnection Connection { get; set; }
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

        public async Task SendPrivateMessageAsync(SignalRMessageModel message, string connectionId, bool selfReceive = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(MixEndpointService.Messenger))
                {
                    await StartConnection();
                    await Connection.InvokeAsync(HubMethods.SendPrivateMessage, message, connectionId, selfReceive);
                }
                else
                {
                    Console.WriteLine("Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task SendMessageAsync(SignalRMessageModel message)
        {
            try
            {
                if (!string.IsNullOrEmpty(MixEndpointService.Messenger))
                {
                    await StartConnection();
                    await Connection.InvokeAsync(HubMethods.SendMessage, message);
                }
                else
                {
                    Console.WriteLine("Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task StartConnection()
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
                    if (Connection.State == HubConnectionState.Disconnected)
                    {
                        await Connection.StartAsync();
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
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

                Connection.On(HubMethods.ReceiveMethod, async (string message) =>
                {
                    if (message.IsJsonString())
                    {
                        var obj = ReflectionHelper.ParseStringToObject<SignalRMessageModel>(message);
                        await HandleMessage(obj);
                    }
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

        protected abstract Task HandleMessage(SignalRMessageModel message);

    }
}
