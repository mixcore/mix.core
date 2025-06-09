using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.Enums;
using Mix.Automation.Lib.Models;
using Mix.Automation.Lib.Services;
using Mix.Automation.Lib.ViewModels;
using Mix.Constant.Enums;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Subscribers
{
    public sealed class MixAutomationSubscriber : SubscriberBase
    {
        private readonly string[] _allowActions;
        public MixAutomationSubscriber(
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<SubscriberBase> logger,
            DatabaseService databaseService,
            IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null)
            : base(
                  MixAutomationConstants.Topics.MixAutomation,
                  nameof(MixAutomationSubscriber),
                  2000,
                  servicesProvider, configuration, queueService, logger, rabbitMQObjectPolicy)
        {
            _allowActions = GetAllowActions();
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            if (!_allowActions.Any(m => m == model.Action))
            {
                return;
            }
            _ = Enum.TryParse(model.Action, out MixAutomationAction action);
            using (ServiceScope = ServicesProvider.CreateScope())
            {
                var srv = GetRequiredService<WorkflowService>();
                switch (action)
                {
                    case MixAutomationAction.CreateTrigger:
                        var dto = model.ParseData<CreateWorkflowTriggerModel>();
                        if (srv != null)
                        {
                            await srv.CreateTrigger(dto);
                        }
                        break;
                    case MixAutomationAction.ExecuteTrigger:
                        var exeModel = model.ParseData<ExecuteWorkflowTriggerModel>();
                        if (srv != null)
                        {
                            await srv.TriggerWorkflow(exeModel, cancellationToken);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private string[] GetAllowActions()
        {
            return [..Enum.GetNames(typeof(MixAutomationAction))];
        }
    }
}
