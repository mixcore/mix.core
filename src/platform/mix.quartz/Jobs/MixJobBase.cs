using Mix.Queue.Interfaces;
using Mix.Queue.Models;
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
                return ExecuteHandler(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Task.CompletedTask;
            }
        }

        public string JobName { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Schedule { get; set; }
        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
