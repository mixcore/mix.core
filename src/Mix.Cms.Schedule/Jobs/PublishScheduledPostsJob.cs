using Quartz;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule.Jobs
{
    public class PublishScheduledPostsJob : BaseJob
    {
        public PublishScheduledPostsJob(string key, string group = null) : base(key, group)
        {
        }

        public override Task Execute(IJobExecutionContext context)
        {
            return Lib.ViewModels.MixPosts.Helper.PublishScheduledPosts();
        }
    }
}