using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Quartz.Constants;
using Mix.Quartz.Extensions;
using Mix.Quartz.Interfaces;
using Mix.Shared;
using Newtonsoft.Json.Linq;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Quartz.Services
{
    public class QuartzService : IQuartzService
    {
        private readonly DatabaseService _databaseService;
        public IScheduler Scheduler;

        public QuartzService(IJobFactory jobFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _databaseService = new DatabaseService(httpContextAccessor, configuration);
            LoadScheduler().GetAwaiter().GetResult();
            Scheduler.JobFactory = jobFactory;
        }

        public async Task LoadScheduler()
        {
            var connectionString = _databaseService.GetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION);
            if (string.IsNullOrEmpty(connectionString))
            {
                Scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            }
            else
            {
                await _databaseService.InitQuartzContextAsync();
                var provider = FindDbProvider(_databaseService.DatabaseProvider);
                var config = SchedulerBuilder.Create();
                config.UsePersistentStore(store =>
                {
                    // it's generally recommended to stick with
                    // string property keys and values when serializing
                    store.UseProperties = true;
                    store.UseGenericDatabase(provider, db => db.ConnectionString = connectionString);
                    store.UseNewtonsoftJsonSerializer();
                });

                ISchedulerFactory schedulerFactory = config.Build();
                Scheduler = await schedulerFactory.GetScheduler();
            }
        }

        public async Task<bool> CheckExist(string triggerName, CancellationToken cancellationToken = default)
        {
            return await Scheduler.CheckExists(new TriggerKey(triggerName), cancellationToken);
        }

        public Task PauseTrigger(string id, CancellationToken cancellationToken = default)
        {
            var key = new TriggerKey(id);
            return Scheduler.PauseTrigger(key, cancellationToken);
        }

        public async Task TriggerJob(string id, CancellationToken cancellationToken = default)
        {
            var trigger = await GetTrigger(id, cancellationToken);
            if (trigger != null)
            {
                await Scheduler.TriggerJob(trigger.JobKey, trigger.Trigger.JobDataMap, cancellationToken);
            }
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

        public async Task<bool> DeleteJob(string jobName, CancellationToken cancellationToken = default)
        {
            var key = new JobKey(jobName);
            var result = await Scheduler.DeleteJob(key, cancellationToken);
            return result;
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

        public Task Start(CancellationToken cancellationToken = default)
        {
            return Scheduler.Start(cancellationToken);
        }

        public Task Shutdown(CancellationToken cancellationToken = default)
        {
            return Scheduler?.Shutdown(cancellationToken);
        }

        public async Task ScheduleJob(MixJobBase jobSchedule, CancellationToken cancellationToken = default)
        {
            var schedule = new JobSchedule(jobSchedule.JobType);
            await ScheduleJob(schedule, cancellationToken);
        }

        public Task ScheduleJob<T>(JobSchedule schedule, CancellationToken cancellationToken = default) where T : MixJobBase
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
                var existed = await Scheduler.CheckExists(triggerKey, cancellationToken);
                if (existed)
                {
                    throw new Exception($"Trigger: {triggerKey.Name} existed");
                }

                var jobType = FindJobType(schedule.JobName);
                if (jobType == null)
                {
                    throw new Exception($"No type found {schedule.JobName}");
                }

                var job = await GetJob(jobType.FullName, cancellationToken);
                if (job == null)
                {
                    var trigger = CreateTrigger(schedule);
                    job = CreateJob(jobType);
                    await Scheduler.ScheduleJob(job, trigger, cancellationToken);
                }
                else
                {
                    var trigger = CreateTrigger(schedule, job);
                    await Scheduler.ScheduleJob(trigger, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task RescheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default)
        {
            if (schedule != null)
            {
                var newTrigger = CreateTrigger(schedule);
                await Scheduler.RescheduleJob(newTrigger.Key, newTrigger, cancellationToken);
            }
        }

        private string FindDbProvider(MixDatabaseProvider databaseProvider) => databaseProvider switch
        {
            MixDatabaseProvider.SQLSERVER => QuartzDbProviders.SqlServer,
            MixDatabaseProvider.MySQL => QuartzDbProviders.MySql,
            MixDatabaseProvider.PostgreSQL => QuartzDbProviders.PostgresSql,
            MixDatabaseProvider.SQLITE => QuartzDbProviders.SQLite,
            _ => throw new NotImplementedException(),
        };

        private IJobDetail CreateJob(Type jobType)
        {
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private ITrigger CreateTrigger(JobSchedule schedule, IJobDetail job = null)
        {
            schedule.Name ??= $"{schedule.JobName}.trigger";
            return TriggerBuilder
                .Create()
                .ForJobIf(job != null, job)
                .WithIdentity(schedule.Name)
                .WithDescription(schedule.Description)
                .UsingJobDataIf(schedule.JobData != null, schedule.JobData)
                .WithCronScheduleIf(!string.IsNullOrEmpty(schedule.CronExpression), schedule.CronExpression)
                .StartNowIf(schedule.IsStartNow)
                .WithMixSchedule(schedule)
                .StartAtIfHaveValue(!schedule.IsStartNow && schedule.StartAt.HasValue, schedule.StartAt)
                .EndAtIf(schedule)
                .Build();
        }

        private Type FindJobType(string jobName)
        {
            var assemblies = MixAssemblyFinder.GetAssembliesByPrefix("mix");
            Type jobType = null;
            foreach (var assembly in assemblies)
            {
                jobType = assembly.GetExportedTypes().FirstOrDefault(p => p.FullName == jobName);
                if (jobType != null)
                {
                    break;
                }
            }

            return jobType;
        }

        private static void LogException(Exception ex = null, MixErrorStatus? status = null, string message = null)
        {
            var fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            var filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                var file = new FileInfo(filePath);
                var content = "[]";
                if (file.Exists)
                {
                    using (var s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                var arrExceptions = JArray.Parse(content);
                var jex = new JObject()
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
