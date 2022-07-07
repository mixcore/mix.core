using Mix.Heart.Helpers;
using System;
using System.Collections.Generic;

namespace Mix.MixQuartz.Extensions
{
    public static class ITriggerConfiguratorExtensions
    {
        public static TriggerBuilder StartNowIf(this TriggerBuilder trigger, bool condition)
        {
            return condition ? trigger.StartNow() : trigger;
        }

        public static TriggerBuilder StartAtIfHaveValue(this TriggerBuilder trigger, bool condition, DateTime? startAt)
        {
            return condition ? trigger.StartAt(startAt.Value) : trigger;
        }

        public static TriggerBuilder UsingJobDataIf(this TriggerBuilder trigger, bool condition, IDictionary<string, object> dicJobData)
        {
            if (dicJobData != null)
            {
                foreach (var key in dicJobData.Keys)
                {
                    if (dicJobData[key] is not string)
                    {
                        dicJobData[key] = ReflectionHelper.ParseObject(dicJobData[key]).ToString();
                    }
                }
            }
            return condition ? trigger.UsingJobData(new(dicJobData)) : trigger;
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
                    case MixIntevalType.Second:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInSeconds(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntevalType.Minute:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInMinutes(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntevalType.Hour:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInHours(schedule.Interval.Value).Repeat(schedule.RepeatCount));
                    case MixIntevalType.Day:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInDays(schedule.Interval.Value));
                    case MixIntevalType.Week:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInWeeks(schedule.Interval.Value));
                    case MixIntevalType.Month:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInMonths(schedule.Interval.Value));
                    case MixIntevalType.Year:
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
