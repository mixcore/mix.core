using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Event.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.SignalR.Enums;
using Mix.SignalR.Hubs;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Lib.Subscribers
{
    public class MixBackgroundTaskSubscriber : SubscriberBase
    {
        protected IPortalHubClientService PortalHub;
        protected MixDbEventService MixDbEventService;
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;
        private static string[] allowActions =
        {
            MixQueueActions.SendMail,
            MixQueueActions.MixDbEvent,
            MixQueueActions.InstallMixApplication
        };
        public MixBackgroundTaskSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IPortalHubClientService portalHub,
            MixDbEventService mixDbEventService,
            IQueueService<MessageQueueModel> queueService)
            : base(TopicId, nameof(MixBackgroundTaskSubscriber), 20, serviceProvider, configuration, queueService)
        {
            PortalHub = portalHub;
            MixDbEventService = mixDbEventService;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Run(async () =>
            {
                while (PortalHub.Connection == null || PortalHub.Connection.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                {
                    await Task.Delay(5000);
                    await PortalHub.StartConnection();
                }
            });
            return base.StartAsync(cancellationToken);
        }
        public override async Task Handler(MessageQueueModel model)
        {
            if (!allowActions.Contains(model.Action))
            {
                return;
            }

            switch (model.Action)
            {
                case MixQueueActions.InstallMixApplication:
                    await InstallMixApplication(model);
                    break;

                case MixQueueActions.SendMail:
                    await SendMail(model);
                    break;

                case MixQueueActions.MixDbEvent:
                    var evtCmd = model.ParseData<MixDbEventCommand>();
                    if (evtCmd != null)
                    {
                        await MixDbEventService.HandleMessage(evtCmd);
                    }
                    break;
            }
        }

        private async Task InstallMixApplication(MessageQueueModel model)
        {
            var cmsUow = GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
            IMixApplicationService _applicationService = GetRequiredService<IMixApplicationService>();
            var app = model.ParseData<MixApplicationViewModel>();
            await _applicationService.Install(app);
            await cmsUow.CompleteAsync();
        }

        public override Task HandleException(MessageQueueModel model, Exception ex)
        {
            return MixLogService.LogExceptionAsync(ex);
        }

        private async Task SendMail(MessageQueueModel model)
        {
            try
            {
                using (ServiceScope = ServicesProvider.CreateScope())
                {
                    var msg = model.ParseData<EmailMessageModel>();
                    var emailService = GetRequiredService<EmailService>();
                    await emailService.SendMail(msg);
                    await SendMessage($"Sent Email {msg.Subject} to {msg.To}", true);
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage($"Error {model.Action}: {model.Data}", false, ex);
            }

        }

        private async Task SendMessage(string message, bool result, Exception ex = null)
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