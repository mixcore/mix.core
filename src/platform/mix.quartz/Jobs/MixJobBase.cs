using Microsoft.Extensions.DependencyInjection;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Commands;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public abstract class MixJobBase : IJob
    {
        protected readonly IServiceProvider _provider;
        protected bool _singleton;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected MixJobBase(
            IServiceProvider provider,
            IQueueService<MessageQueueModel> queueService,
            bool singleton = false)
        {
            _provider = provider;
            _singleton = singleton;
            JobType = GetType();
            JobName = JobType.FullName;
            _queueService = queueService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                LogJobData(context);
                if (_singleton)
                {
                    return ExecuteHandler(context);
                }
                else
                {
                    // Create a new scope
                    using (var scope = _provider.CreateScope())
                    {
                        // Resolve the Scoped service
                        ExecuteHandler(context);
                    }

                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Task.CompletedTask;
            }
        }

        private void LogJobData(IJobExecutionContext context)
        {
            var cmd = new LogAuditLogCommand(
                    JobName,
                    new("localhost", JobName, "Quartz", context.Trigger.JobDataMap.GetString("data")));
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);

        }

        public string JobName { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Schedule { get; set; }

        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
