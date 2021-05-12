using Mix.Cms.Schedule.Enums;
using Quartz;
using System;

namespace Mix.Cms.Schedule.Extensions
{
    public static class ITriggerConfiguratorExtensions
    {
        public static ITriggerConfigurator StartNowIf(this ITriggerConfigurator trigger, bool condition)
        {
            return condition ? trigger.StartNow() : trigger;
        }

        public static ITriggerConfigurator StartAtIf(this ITriggerConfigurator trigger, bool condition, DateTime startAt)
        {
            return condition ? trigger.StartAt(startAt) : trigger;
        }

        public static ITriggerConfigurator WithMixSchedule(this ITriggerConfigurator trigger, int? internalValue, MixIntevalType? internalType, int? repeatCount)
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
