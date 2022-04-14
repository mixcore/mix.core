using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.MixQuartz.Extensions;
using Mix.Quartz.Constants;
using Newtonsoft.Json.Linq;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (trigger != null)
            {
                var state = await Scheduler.GetTriggerState(key, cancellationToken);
                return new JobSchedule(trigger, state);
            }
            return default;
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
                .StartAtIfHaveValue(!schedule.IsStartNow && schedule.StartAt.HasValue, schedule.StartAt)
                .WithMixSchedule(schedule)
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
                    LogException(message: $"Trigger: {triggerKey.Name} existed");
                }

                var jobType = Assembly.GetAssembly(typeof(MixJobBase)).GetType(schedule.JobName);
                var job = CreateJob(jobType);
                var trigger = CreateTrigger(schedule);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }


        public async Task ResheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default)
        {
            if (schedule != null)
            {
                var newTrigger = CreateTrigger(schedule);
                await Scheduler.RescheduleJob(newTrigger.Key, newTrigger, cancellationToken);
            }
        }

        public static void LogException(Exception ex = null, MixErrorStatus? status = null, string message = null)
        {
            string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                FileInfo file = new(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new()
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Status", status?.ToString()),
                    new JProperty("Message", message),
                    new JProperty("Details", ex == null ? null : JObject.FromObject(ex))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using var writer = File.CreateText(filePath);
                writer.WriteLine(content);
            }
            catch
            {
                Console.Write($"Cannot write log file {filePath}");
                // File invalid
            }
        }
    }
}
