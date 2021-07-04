using Mix.MixQuartz.Enums;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Models;
using System;
using System.Collections.Generic;

namespace Mix.MixQuartz.Helpers
{
    public class MixQuartzHelper
    {
        public static List<MixJobModel> LoadJobConfiguraions()
        {
            return new List<MixJobModel>()
            {
                new MixJobModel()
                {
                    Key = "PublishScheduledPostsJob",
                    Group = null,
                    Description = "Publish Scheduled Posts Job",
                    JobType = typeof(PublishScheduledPostsJob),
                    Trigger = new MixTrigger()
                    {
                        StartAt = DateTime.UtcNow.AddSeconds(10),
                        Interval = 1,
                        IntervalType = MixIntevalType.Minute
                    }
                },
                new MixJobModel()
                {
                    Key = "KeepPoolAliveJob",
                    Group = null,
                    Description = "Keep Pool Alive Job",
                    JobType = typeof(KeepPoolAliveJob),
                    Trigger = new MixTrigger()
                    {
                        StartAt = DateTime.UtcNow.AddSeconds(10),
                        Interval = 5,
                        IntervalType = MixIntevalType.Minute
                    }
                }
            };
        }
    }
}
