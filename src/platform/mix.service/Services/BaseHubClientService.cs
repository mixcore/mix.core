using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Extensions;
using Mix.Service.Models;
using Mix.Shared.Extensions;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace Mix.Service.Services
{
    public abstract class BaseHubClientService : IHubClientService
    {
        public HubConnection Connection { get; set; }
        protected string HubEndpoint;
        protected ILogger _logger;
        protected IConfiguration _configuration;
        protected string _hub;
        protected string _endpoint;
        protected string AccessToken;
        public bool IsStarting = false;
        protected BaseHubClientService(string hub, string endpoint, ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _hub = hub;
            _endpoint = endpoint;

            _configuration = configuration;
            _ = Task.Run(StartConnection);
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

        public Task SendGroupMessageAsync(string groupName, string title, string description, object data, MessageType messageType = MessageType.Info, bool exceptCaller = true)
        {
            var msg = new SignalRMessageModel(data)
            {
                Title = title,
                Message = description,
                Type = messageType
            };
            return SendGroupMessageAsync(msg, groupName, exceptCaller);
        }

        public async Task SendPrivateMessageAsync(SignalRMessageModel message, string connectionId, bool selfReceive = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(HubEndpoint))
                {
                    await StartConnection();
                    await Connection.InvokeAsync(HubMethods.SendPrivateMessage, message, connectionId, selfReceive);
                    _logger.LogInformation("Start SignalR client successfully");
                }
                else
                {
                    _logger.LogWarning("Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
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
                if (!string.IsNullOrEmpty(HubEndpoint))
                {

                    await Connection.InvokeAsync(HubMethods.SendMessage, message);
                }
                else
                {
                    _logger.LogWarning($"{_logger.GetType().FullName}: Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task SendGroupMessageAsync(SignalRMessageModel message, string groupName, bool exceptCaller = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(HubEndpoint))
                {
                    await StartConnection();
                    await Connection.InvokeAsync(HubMethods.SendGroupMessage, message, groupName, exceptCaller);
                }
                else
                {
                    _logger.LogWarning($"{_logger.GetType().FullName}: Cannot Start SignalR Hub: MixEndpointService.Messenger is null or empty");
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task StartConnection()
        {
            await InitEndpoint();
            while (Connection == null)
            {
                await Task.Delay(5000);
                Init();
            }

            while (Connection != null && Connection.State != HubConnectionState.Connected)
            {
                try
                {
                    if (!IsStarting)
                    {
                        IsStarting = true;
                        await Connection.StartAsync();
                    }
                }

                catch (Exception ex)
                {
                    IsStarting = false;
                    await Task.Delay(2000);
                    Console.WriteLine(ex);
                }
            }
        }

        private void Init()
        {

            Connection = new HubConnectionBuilder()
               .WithUrl(HubEndpoint, options =>
               {
                   options.AccessTokenProvider = async () => await Task.FromResult(AccessToken);
               })
               .WithKeepAliveInterval(TimeSpan.FromSeconds(2))
               .WithStatefulReconnect()
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

            Connection.Reconnected += msg =>
            {
                Console.WriteLine(Connection.State);

                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.

                return Task.CompletedTask;
            };
        }
        private async Task InitEndpoint()
        {
            while (string.IsNullOrEmpty(_configuration.BaseUrl()))
            {
                Console.WriteLine("Waiting for init endpoint");
                await Task.Delay(2000);
            }

            if (string.IsNullOrEmpty(_endpoint))
            {
                _endpoint = _configuration.BaseUrl()!;
            }
            HubEndpoint = $"{_endpoint.TrimEnd('/')}/{_hub.TrimStart('/')}";
        }
        protected abstract Task HandleMessage(SignalRMessageModel message);

    }
}
