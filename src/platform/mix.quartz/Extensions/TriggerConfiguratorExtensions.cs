using Mix.Heart.Helpers;
using System;
using System.Collections.Generic;

namespace Mix.Quartz.Extensions
{
    public static class TriggerConfiguratorExtensions
    {
        public static TriggerBuilder StartNowIf(this TriggerBuilder trigger, bool condition)
        {
            return condition ? trigger.StartNow() : trigger;
        }

        public static TriggerBuilder StartAtIfHaveValue(this TriggerBuilder trigger, bool condition, DateTime? startAt)
        {
            return condition && startAt != null ? trigger.StartAt(startAt.Value) : trigger;
        }

        public static TriggerBuilder UsingJobDataIf(this TriggerBuilder trigger, bool condition, IDictionary<string, object> dictJobData)
        {
            if (dictJobData != null)
            {
                foreach (var key in dictJobData.Keys)
                {
                    if (dictJobData[key] is not string)
                    {
                        dictJobData[key] = ReflectionHelper.ParseObject(dictJobData[key]).ToString();
                    }
                }
            }
            return condition && dictJobData != null ? trigger.UsingJobData(new JobDataMap(dictJobData)) : trigger;
        }

        public static TriggerBuilder ForJobIf(this TriggerBuilder trigger, bool condition, IJobDetail job)
        {
            return condition ? trigger.ForJob(job) : trigger;
        }

        public static TriggerBuilder WithCronScheduleIf(this TriggerBuilder trigger, bool condition, string cron)
        {
            return condition ? trigger.WithCronSchedule(cron) : trigger;
        }

        public static TriggerBuilder EndAtIf(this TriggerBuilder trigger, JobSchedule schedule)
        {
            if (schedule.EndAt.HasValue)
            {
                return trigger.EndAt(schedule.EndAt.Value);
            }
            return trigger;
        }

        public static TriggerBuilder WithMixSchedule(this TriggerBuilder trigger, JobSchedule schedule)
        {
            if (!schedule.Interval.HasValue)
            {
                return trigger;
            }
            else
            {
                var calendarSchedule = CalendarIntervalScheduleBuilder.Create();
                switch (schedule.IntervalType)
                {
                    case MixIntervalType.Second:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInSeconds(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntervalType.Minute:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInMinutes(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntervalType.Hour:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInHours(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntervalType.Day:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInDays(schedule.Interval.Value));
                    case MixIntervalType.Week:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInWeeks(schedule.Interval.Value));
                    case MixIntervalType.Month:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInMonths(schedule.Interval.Value));
                    case MixIntervalType.Year:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInYears(schedule.Interval.Value));
                    default:
                        return trigger;
                }
            }
        }

        private static SimpleScheduleBuilder Repeat(this SimpleScheduleBuilder builder, int? repeatCount)
        {
            return repeatCount.HasValue ? builder.WithRepeatCount(repeatCount.Value)
                : builder.RepeatForever();
        }
    }
}
