using Microsoft.Extensions.Hosting;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Quartz.Services
{
    // Ref: https://andrewlock.net/creating-a-quartz-net-hosted-service-with-asp-net-core/
    public class QuartzHostedService : IHostedService
    {
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<BaseJob> _jobSchedules;
        private readonly IScheduler _scheduler;

        public QuartzHostedService(
            IJobFactory jobFactory,
            IEnumerable<BaseJob> jobSchedules, IScheduler scheduler)
        {
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
            _scheduler = scheduler;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);

                if (jobSchedule.Trigger != null)
                {
                    var trigger = CreateTrigger(jobSchedule);

                    await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                }
            }

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(BaseJob schedule)
        {
            var jobType = schedule.GetType();
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(BaseJob schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.GetType().FullName}.trigger")
                .StartNowIf(schedule.Trigger.IsStartNow)
                .StartAtIf(schedule.Trigger.StartAt.HasValue, schedule.Trigger.StartAt.Value)
                .WithMixSchedule(schedule.Trigger.Interval, schedule.Trigger.IntervalType, schedule.Trigger.RepeatCount)
                .WithCronScheduleIf(!string.IsNullOrEmpty(schedule.Trigger.CronExpression), schedule.Trigger.CronExpression)
                .WithDescription(schedule.Trigger.CronExpression)
                .Build();
        }
    }
}
