using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Commands;
using Mix.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Mix.Quartz.Jobs
{
    public abstract class MixJobBase : IJob
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IQueueService<MessageQueueModel> QueueService;
        protected MixJobBase(
            IServiceProvider serviceProvider,
            IQueueService<MessageQueueModel> queueService)
        {
            ServiceProvider = serviceProvider;
            JobType = GetType();
            JobName = JobType.FullName;
            QueueService = queueService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                LogJobData(context);
                return ExecuteHandler(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Task.CompletedTask;
            }
        }

        private void LogJobData(IJobExecutionContext context)
        {
            var request = new ParsedRequestModel("localhost", JobName, "Quartz", context.Trigger.JobDataMap.GetString("data"));
            var cmd = new LogAuditLogCommand(Guid.NewGuid(), JobName, request);
            QueueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);

        }

        public string JobName { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Schedule { get; set; }
        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
