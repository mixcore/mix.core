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
                    if (dicJobData[key].GetType() != typeof(string))
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

            if (schedule.StartAt.HasValue && schedule.RepeatCount.HasValue && schedule.Interval.HasValue)
            {
                schedule.StartAt ??= DateTime.UtcNow;

                switch (schedule.IntervalType)
                {
                    case MixIntevalType.Second:
                        schedule.StartAt = !schedule.IsStartNow 
                                                ? schedule.StartAt.Value.AddSeconds(schedule.Interval.Value) 
                                                : schedule.StartAt;
                        schedule.EndAt = schedule.StartAt.Value.AddSeconds(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                    case MixIntevalType.Minute:
                        schedule.StartAt =  schedule.StartAt.Value.AddMinutes(schedule.Interval.Value);
                        schedule.EndAt = schedule.StartAt.Value.AddMinutes(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                    case MixIntevalType.Hour:
                        schedule.StartAt =  schedule.StartAt.Value.AddHours(schedule.Interval.Value);
                        schedule.EndAt = schedule.StartAt.Value.AddHours(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                    case MixIntevalType.Day:
                        schedule.StartAt =  schedule.StartAt.Value.AddDays(schedule.Interval.Value);
                        schedule.EndAt = schedule.StartAt.Value.AddDays(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                    case MixIntevalType.Week:
                        schedule.StartAt =  schedule.StartAt.Value.AddDays(schedule.Interval.Value * 7);
                        schedule.EndAt = schedule.StartAt.Value.AddDays(schedule.Interval.Value * schedule.RepeatCount.Value * 7);
                        break;
                    case MixIntevalType.Month:
                        schedule.StartAt =  schedule.StartAt.Value.AddMonths(schedule.Interval.Value);
                        schedule.EndAt = schedule.StartAt.Value.AddMonths(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                    case MixIntevalType.Year:
                        schedule.StartAt =  schedule.StartAt.Value.AddYears(schedule.Interval.Value);
                        schedule.EndAt = schedule.StartAt.Value.AddYears(schedule.Interval.Value * schedule.RepeatCount.Value);
                        break;
                }
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
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInSeconds(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Minute:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInMinutes(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Hour:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInHours(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Day:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInDays(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Week:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInWeeks(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Month:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInMonths(schedule.Interval.Value)).EndAtIf(schedule);
                    case MixIntevalType.Year:
                        return trigger.WithSchedule(calendarSchedule.WithIntervalInYears(schedule.Interval.Value)).EndAtIf(schedule);
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
