﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Database.Entities.MixDb;
using Mix.Mixdb.Event.Services;
using Mix.Mq.Lib.Models;
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
        private readonly string[] _allowActions;
        private const string TopicId = MixQueueTopics.MixDbCommand;

        public MixDbCommandSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<MixDbCommandSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(TopicId, nameof(MixDbCommandSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMqObjectPolicy)
        {
            _allowActions = [.. Enum.GetNames(typeof(MixDbCommandQueueAction))];
        }

        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            if (!_allowActions.Contains(model.Action))
            {
                return;
            }

            IMixDbDataService mixDbDataService = GetRequiredService<IMixDbDataService>();
            IMixDbCommandHubClientService mixDbCommandHub = GetRequiredService<IMixDbCommandHubClientService>();
            UnitOfWorkInfo<MixDbDbContext> uow = GetRequiredService<UnitOfWorkInfo<MixDbDbContext>>();
            mixDbDataService.SetUOW(uow);
            Enum.TryParse(model.Action, out MixDbCommandQueueAction action);
            switch (action)
            {
                case MixDbCommandQueueAction.POST:
                    var cmd = model.ParseData<MixDbCommandModel>();
                    if (cmd != null)
                    {
                        var id = await mixDbDataService.CreateData(cmd.MixDbName, cmd.Body);
                        if (!string.IsNullOrEmpty(cmd.ConnectionId))
                        {
                            await mixDbCommandHub.SendPrivateMessageAsync(
                                new SignalRMessageModel(cmd.Body) { Title = "Success", Type = MessageType.Success }, cmd.ConnectionId, false);
                        }
                    }
                    break;
                case MixDbCommandQueueAction.PUT:
                    var updCmd = model.ParseData<MixDbCommandModel>();
                    if (updCmd != null)
                    {
                        var id = await mixDbDataService.UpdateData(updCmd.MixDbName, updCmd.Body);
                        if (!string.IsNullOrEmpty(updCmd.ConnectionId))
                        {
                            await mixDbCommandHub.SendPrivateMessageAsync(
                                new SignalRMessageModel(updCmd.Body) { Title = "Success", Type = MessageType.Success }, updCmd.ConnectionId, false);
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
