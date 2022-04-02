using System;

namespace Mix.MixQuartz.Models
{
    public class MixJobModel
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public Type JobType { get; set; }
        public JobSchedule Trigger { get; set; }
    }
}
