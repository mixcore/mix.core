using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public abstract class MixJobBase : IJob
    {
        private readonly IServiceProvider _provider;
        protected bool _singleton;
        protected MixJobBase(IServiceProvider provider, bool singleton = false)
        {
            _provider = provider;
            _singleton = singleton;
            JobType = GetType();
            JobName = JobType.FullName;
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

        public string JobName { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Schedule { get; set; }

        public abstract Task ExecuteHandler(IJobExecutionContext context);
    }
}
