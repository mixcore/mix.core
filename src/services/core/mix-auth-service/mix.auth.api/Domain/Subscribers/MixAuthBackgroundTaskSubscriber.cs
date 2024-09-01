using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Auth.Api.Domain.Subscribers
{
    public sealed class MixAuthBackgroundTaskSubscriber : SubscriberBase
    {
        private IPortalHubClientService PortalHub;
        private static string topicId = MixQueueTopics.MixBackgroundTasks;
        private static string[] allowActions = {
        };
        public MixAuthBackgroundTaskSubscriber(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
            IPortalHubClientService portalHub,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<MixAuthBackgroundTaskSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(topicId, nameof(MixAuthBackgroundTaskSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMqObjectPolicy)
        {
            PortalHub = portalHub;
        }
        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            base.StartAsync(cancellationToken);

            return Task.Run(async () =>
            {
                while (PortalHub.Connection == null || PortalHub.Connection.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                {
                    try
                    {
                        await Task.Delay(5000);
                        await PortalHub.StartConnection();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(GetType().Name, ex);
                    }
                }
            });
        }
        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            if (!allowActions.Contains(model.Action))
            {
                return;
            }

            try
            {
                using (ServiceScope = ServicesProvider.CreateScope())
                {
                    switch (model.Action)
                    {
                        default:
                            await SendMessage($"Received Message {model.Action}", true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage(model.Action, false, ex);
            }
        }

        private async Task SendMessage(string message, bool result, Exception? ex = null)
        {
            SignalRMessageModel msg = new()
            {
                Action = MessageAction.NewMessage,
                Type = result ? MessageType.Success : MessageType.Error,
                Title = message,
                From = new(GetType().FullName),
                Message = ex == null ? message : ex!.Message
            };
            await PortalHub.SendMessageAsync(msg);
        }


    }
}
