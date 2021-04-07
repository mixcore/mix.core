using Quartz;
using System;

namespace Mix.Cms.Schedule.Models
{
    public class MixJobModel
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public Type JobType { get; set; }
        public MixTrigger Trigger { get; set; } = new MixTrigger();
    }
}
