using Mix.Cms.Lib.Models.Cms;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
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
