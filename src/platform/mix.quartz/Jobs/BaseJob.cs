using Microsoft.Extensions.DependencyInjection;
using Mix.MixQuartz.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public abstract class BaseJob : IJob
    {
        private readonly IServiceProvider _provider;
        protected bool _singleton;
        protected BaseJob(IServiceProvider provider, bool singleton = false)
        {
            _provider = provider;
            _singleton = singleton;
        }

        public Task Execute(IJobExecutionContext context)
        {
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

        public string Key { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Trigger { get; set; }

        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
