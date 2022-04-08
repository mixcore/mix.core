using System;
using System.Collections.Generic;

namespace Mix.MixQuartz.Models
{
    public class JobSchedule
    {
        public JobSchedule()
        {

        }
        public JobSchedule(Type jobType)
        {
            JobName = jobType.FullName;
            Name ??= $"{JobName}.trigger";
        }

        public JobSchedule(ITrigger trigger, TriggerState state)
        {
            Trigger = trigger;
            Key = trigger.Key;
            Name = trigger.Key.Name;
            State = state;
            JobKey = trigger.JobKey;
            JobName = trigger.JobKey.Name;
        }

        public ITrigger Trigger { get; set; }
        public TriggerKey Key { get; set; }
        public string Name { get; set; }
        public JobKey JobKey { get; set; }
        public TriggerState State { get; set; }

        public Dictionary<string, object> JobData { get; set; }
        public string? CronExpression { get; set; }
        public string? GroupName { get; set; }
        public string? JobName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public bool IsStartNow { get; set; }
        public int? Interval { get; set; }
        public MixIntevalType? IntervalType { get; set; }
        public int? RepeatCount { get; set; }
    }
}
