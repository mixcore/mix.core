using Mix.MixQuartz.Enums;
using Quartz;
using System;

namespace Mix.MixQuartz.Extensions
{
    public static class ITriggerConfiguratorExtensions
    {
        public static TriggerBuilder StartNowIf(this TriggerBuilder trigger, bool condition)
        {
            return condition ? trigger.StartNow() : trigger;
        }

        public static TriggerBuilder StartAtIfHaveValue(this TriggerBuilder trigger, DateTime? startAt)
        {
            return startAt.HasValue ? trigger.StartAt(startAt.Value) : trigger;
        }
        
        public static TriggerBuilder WithCronScheduleIf(this TriggerBuilder trigger, bool condition, string cron)
        {
            return condition ? trigger.WithCronSchedule(cron) : trigger;
        }

        public static TriggerBuilder WithMixSchedule(this TriggerBuilder trigger, int? internalValue, MixIntevalType? internalType, int? repeatCount)
        {
            if (!internalValue.HasValue)
            {
                return trigger;
            }
            else
            {
                switch (internalType)
                {
                    case MixIntevalType.Second:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInSeconds(internalValue.Value).Repeat(repeatCount));
                    case MixIntevalType.Minute:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInMinutes(internalValue.Value).Repeat(repeatCount));
                    case MixIntevalType.Hour:
                        return trigger.WithSimpleSchedule(x => x.WithIntervalInHours(internalValue.Value).Repeat(repeatCount));
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
