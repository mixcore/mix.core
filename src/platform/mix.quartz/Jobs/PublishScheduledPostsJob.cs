using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class PublishScheduledPostsJob : MixJobBase
    {
        public PublishScheduledPostsJob(IServiceProvider provider) : base(provider, true)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}