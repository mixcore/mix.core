using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.MixQuartz.Extensions;
using Mix.Quartz.Constants;
using Mix.Shared.Constants;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Quartz.Services
{
    public class QuartzService
    {
        public IScheduler Scheduler;

        public QuartzService(IJobFactory jobFactory)
        {
            LoadScheduler().GetAwaiter().GetResult();
            Scheduler.JobFactory = jobFactory;
        }

        public async Task LoadScheduler()
        {
            MixDatabaseService databaseService = new();
            string cnn = databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);

            if (string.IsNullOrEmpty(cnn))
            {
                Scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            }
            else
            {
                await databaseService.InitQuartzContextAsync();
                string dbProvider = GetQuartzDbProvider(databaseService.DatabaseProvider);
                // TODO: use database for quartz
                var config = SchedulerBuilder.Create();
                config.UsePersistentStore(store =>
                {
                    // it's generally recommended to stick with
                    // string property keys and values when serializing
                    store.UseProperties = true;
                    store.UseGenericDatabase(dbProvider, db =>
                        db.ConnectionString = cnn
                    );

                    store.UseJsonSerializer();
                });
                ISchedulerFactory schedulerFactory = config.Build();
                Scheduler = await schedulerFactory.GetScheduler();
            }
        }

        private string GetQuartzDbProvider(MixDatabaseProvider databaseProvider)
        => databaseProvider switch
        {
            MixDatabaseProvider.SQLSERVER => QuartzDbProviders.SqlServer,
            MixDatabaseProvider.MySQL => QuartzDbProviders.MySql,
            MixDatabaseProvider.PostgreSQL => QuartzDbProviders.PostgresSql,
            MixDatabaseProvider.SQLITE => QuartzDbProviders.SQLite,
            _ => throw new NotImplementedException(),
        };

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

        public async Task ScheduleJob(MixJobBase jobSchedule, CancellationToken cancellationToken = default)
        {
            var existed = await Scheduler.CheckExists(new JobKey(jobSchedule.Key));
            if (!existed && jobSchedule.Schedule != null)
            {
                var job = CreateJob(jobSchedule.JobType);
                var trigger = CreateTrigger(jobSchedule.Schedule, $"{jobSchedule.Key}.trigger");
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
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
