using Quartz;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class PublishScheduledPostsJob : BaseJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}