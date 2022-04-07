using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.MixQuartz.Extensions;
using Mix.Quartz.Constants;
using Mix.Shared.Constants;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        public async Task<bool> CheckExist(string triggerName)
        {
            return await Scheduler.CheckExists(new TriggerKey(triggerName));
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

        public Task ResumeTrigger(string id, CancellationToken cancellationToken = default)
        {
            var key = new TriggerKey(id);
            return Scheduler.ResumeTrigger(key, cancellationToken);
        }

        public async Task<JobSchedule> GetTrigger(string id, CancellationToken cancellationToken = default)
        {
            var key = new TriggerKey(id);
            var trigger = await Scheduler.GetTrigger(key, cancellationToken);
            var state = await Scheduler.GetTriggerState(key, cancellationToken);
            return new JobSchedule(trigger, state);
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

        public ITrigger CreateTrigger(JobSchedule schedule, IJobDetail job = null)
        {
            schedule.Name ??= $"{schedule.JobName}.trigger";
            return TriggerBuilder
                .Create()
                .ForJobIf(job != null, job)
                .WithIdentity(schedule.Name)
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
            var schedule = new JobSchedule(jobSchedule.JobType);
            await ScheduleJob(schedule);
        }
        public Task ScheduleJob<T>(JobSchedule schedule, CancellationToken cancellationToken = default)
            where T : MixJobBase
        {
            if (schedule != null)
            {
                schedule.JobName = typeof(T).FullName;
                return ScheduleJob(schedule, cancellationToken);
            }
            return Task.CompletedTask;
        }

        public async Task ScheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default)
        {
            try
            {
                schedule.Name ??= $"{schedule.JobName}.trigger";
                var triggerKey = new TriggerKey(schedule.Name);
                var existed = await Scheduler.CheckExists(triggerKey);
                if (existed)
                {
                    throw new MixException(MixErrorStatus.Badrequest, $"Trigger: {triggerKey.Name} existed");
                }

                var jobType = Assembly.GetAssembly(typeof(MixJobBase)).GetType(schedule.JobName);
                var job = CreateJob(jobType);
                var trigger = CreateTrigger(schedule);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);

            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }


        public async Task ReScheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default)
        {
            if (schedule != null)
            {
                var newTrigger = CreateTrigger(schedule);
                await Scheduler.RescheduleJob(newTrigger.Key, newTrigger, cancellationToken);
            }
        }
    }
}
