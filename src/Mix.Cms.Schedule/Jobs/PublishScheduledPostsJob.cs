using Quartz;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule.Jobs
{
    public class PublishScheduledPostsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Lib.ViewModels.MixPosts.Helper.PublishScheduledPosts();
        }
    }
}