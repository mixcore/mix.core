using Quartz;
using System;

namespace Mix.Cms.Schedule.Models
{
    public class MixJobModel
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string JobName { get; set; }
    }
}
