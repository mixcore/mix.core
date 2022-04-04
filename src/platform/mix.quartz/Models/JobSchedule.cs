using System;
using System.Collections.Generic;

namespace Mix.MixQuartz.Models
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType)
        {
            JobName = jobType.FullName;
        }

        public JobSchedule()
        {

        }

        public Dictionary<string, object> JobData { get; set; }
        public string? CronExpression { get; set; }
        public string? Name { get; set; }
        public string? GroupName { get; set; }
        public string? JobName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartAt { get; set; }
        public bool IsStartNow { get; set; }
        public int? Interval { get; set; }
        public MixIntevalType? IntervalType { get; set; }
        public int? RepeatCount { get; set; }
    }
}
