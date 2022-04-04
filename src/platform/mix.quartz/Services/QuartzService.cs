using Mix.MixQuartz.Extensions;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Quartz.Services
{
    public class QuartzService
    {
        public readonly IScheduler Scheduler;

        public QuartzService(IScheduler scheduler, IJobFactory jobFactory)
        {
            Scheduler = scheduler;
            Scheduler.JobFactory = jobFactory;
        }

        public Task PauseTrigger(string id, CancellationToken cancellationToken = default)
        {
            var key = new TriggerKey(id);
            return Scheduler.PauseTrigger(key, cancellationToken);
        }

        public async Task<ITrigger> GetTrigger(string id, CancellationToken cancellationToken = default)
        {
            var key = new TriggerKey(id);
            return await Scheduler.GetTrigger(key, cancellationToken);
        }

        public async Task<IJobDetail> GetJob(string id, CancellationToken cancellationToken = default)
        {
            var key = new JobKey(id);
            return await Scheduler.GetJobDetail(key, cancellationToken);
        }

        public async Task<IJobDetail> GetJob<T>(CancellationToken cancellationToken = default)
        {
            var key = new JobKey(typeof(T).FullName);
            return await Scheduler.GetJobDetail(key, cancellationToken);
        }

        public async Task<IEnumerable<TriggerKey>> GetJobTriggerKeys(CancellationToken cancellationToken = default)
        {
            return await Scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup(), cancellationToken);

        }

        public IJobDetail CreateJob(Type jobType)
        {
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        public ITrigger CreateTrigger(JobSchedule schedule, string identity, IJobDetail job = null)
        {
            return TriggerBuilder
                .Create()
                .ForJobIf(job != null, job)
                .WithIdentity(identity)
                .UsingJobDataIf(schedule.JobData != null, schedule.JobData)
                .StartNowIf(schedule.IsStartNow)
                .StartAtIfHaveValue(schedule.StartAt)
                .WithMixSchedule(schedule.Interval, schedule.IntervalType, schedule.RepeatCount)
                .WithCronScheduleIf(!string.IsNullOrEmpty(schedule.CronExpression), schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        public Task Start(CancellationToken cancellationToken)
        {
            return Scheduler.Start(cancellationToken);
        }

        public Task Shutdown(CancellationToken cancellationToken)
        {
            return Scheduler?.Shutdown(cancellationToken);
        }

        public Task ScheduleJob(MixJobBase jobSchedule, CancellationToken cancellationToken = default)
        {

            if (jobSchedule.Schedule != null)
            {
                var job = CreateJob(jobSchedule.JobType);
                var trigger = CreateTrigger(jobSchedule.Schedule, $"{jobSchedule.Key}.trigger");
                return Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task ScheduleJob<T>(JobSchedule schedule, CancellationToken cancellationToken = default)
            where T : MixJobBase
        {
            var job = CreateJob(typeof(T));

            if (schedule != null)
            {
                var trigger = CreateTrigger(schedule, $"{job.Key}.trigger");

                return Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}
