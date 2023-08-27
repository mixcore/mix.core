using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Database.Entities.MixDb;
using Mix.Mixdb.Event.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.RepoDb.Interfaces;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.SignalR.Enums;
using Mix.SignalR.Hubs;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Lib.Subscribers
{
    public class MixDbCommandSubscriber : SubscriberBase
    {
        private const string TopicId = MixQueueTopics.MixDbCommand;

        public MixDbCommandSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> mixQueueService,
            IQueueService<MessageQueueModel> queueService)
            : base(TopicId, nameof(MixDbCommandSubscriber), 20, serviceProvider, configuration, mixQueueService, queueService)
        {
        }

        public override async Task Handler(MessageQueueModel model)
        {
            IMixDbDataService mixDbDataService = GetRequiredService<IMixDbDataService>();
            IMixDbCommandHubClientService mixDbCommandHub = GetRequiredService<IMixDbCommandHubClientService>();
            UnitOfWorkInfo<MixDbDbContext> uow = GetRequiredService<UnitOfWorkInfo<MixDbDbContext>>();
            mixDbDataService.SetUOW(uow);
            switch (model.Action)
            {
                case MixDbCommandQueueActions.Create:
                    var cmd = model.ParseData<MixDbCommandModel>();
                    if (cmd != null)
                    {
                        var id = await mixDbDataService.CreateData(cmd.MixDbName, cmd.Body);
                        if (!string.IsNullOrEmpty(cmd.ConnectionId))
                        {
                            await mixDbCommandHub.SendPrivateMessageAsync(
                                new SignalRMessageModel(cmd.Body), cmd.ConnectionId, false);
                        }
                    }
                    break;
            }
            await uow.CompleteAsync();
        }

        public override Task HandleException(MessageQueueModel model, Exception ex)
        {
            return MixLogService.LogExceptionAsync(ex);
        }
    }
}
