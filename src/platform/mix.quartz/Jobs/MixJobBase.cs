using Microsoft.Extensions.DependencyInjection;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Commands;
using Mix.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public abstract class MixJobBase : IJob
    {
        protected readonly IServiceProvider _provider;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected MixJobBase(
            IServiceProvider provider,
            IQueueService<MessageQueueModel> queueService)
        {
            _provider = provider;
            JobType = GetType();
            JobName = JobType.FullName;
            _queueService = queueService;
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
            var cmd = new LogAuditLogCommand(JobName, request);
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);

        }

        public string JobName { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Schedule { get; set; }
        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
