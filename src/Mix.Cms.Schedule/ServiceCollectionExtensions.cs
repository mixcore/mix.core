using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Services;
using Mix.Cms.Schedule.Jobs;
using Mix.Cms.Schedule.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixScheduler(this IServiceCollection services, IConfiguration configuration)
        {
            // base configuration from appsettings.json
            services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));

            services.AddQuartz(q =>
            {
                // we could leave DI configuration intact and then jobs need
                // to have public no-arg constructor
                // the MS DI is expected to produce transient job instances
                // this WONT'T work with scoped services like EF Core's DbContext
                q.UseMicrosoftDependencyInjectionJobFactory(options =>
                {
                    // if we don't have the job in DI, allow fallback
                    // to configure via default constructor
                    options.AllowDefaultConstructor = true;
                });

                // or for scoped service support like EF Core DbContext
                // q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });

                if (!MixService.GetConfig<bool>("IsInit"))
                {
                    q.AddMixJobs();
                }
            });
            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
            return services;
        }


        private static IServiceCollectionQuartzConfigurator AddMixJobs(this IServiceCollectionQuartzConfigurator quartzConfiguration)
        {
            List<MixJobModel> jobConfiguraions = LoadJobConfiguraions();
            var assembly = Assembly.GetExecutingAssembly();
            var mixJobs = assembly
                .GetExportedTypes()
                .Where(m => m.BaseType.Name == typeof(BaseJob).Name);

            foreach (var job in mixJobs)
            {
                var jobConfig = jobConfiguraions.FirstOrDefault(j => j.JobName == job.Name);
                var jobKey = new JobKey(jobConfig.Key, jobConfig.Group);
                var applyGenericMethod = typeof(IServiceCollectionQuartzConfigurator)
            .GetMethods()
            .Single(m => m.Name == "AddJobListener");
                var addJobMethod = applyGenericMethod.MakeGenericMethod(job);
                addJobMethod.Invoke(quartzConfiguration, new object[] { Activator.CreateInstance(job), jobKey });
            }

            //// here's a known job for triggers

            //var jobKey1 = new JobKey("KeepPoolAliveJob", "Keep Pool Alive Job");
            //quartzConfiguration.AddJob<PublishScheduledPostsJob>(jobKey, j => j
            //    .WithDescription("Publish Scheduled Posts Job")
            //).AddJob<KeepPoolAliveJob>(jobKey1, j => j
            //    .WithDescription("Ping Server")
            //);

            //quartzConfiguration.AddTrigger(t => t
            //    .WithIdentity("PublishScheduledPostsTrigger")
            //    .ForJob(jobKey)
            //    .StartAt(DateTime.UtcNow.AddSeconds(10))
            //    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1)
            //    .RepeatForever())
            //    .WithDescription("Publish Scheduled Posts trigger")
            //);

            //quartzConfiguration.AddTrigger(t => t
            //    .WithIdentity("KeepPoolAliveTrigger")
            //    .ForJob(jobKey1)
            //    .StartAt(DateTime.UtcNow.AddSeconds(5))
            //    .WithSimpleSchedule(x => x.WithIntervalInMinutes(5)
            //    .RepeatForever())
            //    .WithDescription("Keep Pool Alive trigger")
            //);
            return quartzConfiguration;
        }

        private static List<MixJobModel> LoadJobConfiguraions()
        {


            return new List<MixJobModel>()
            {
                new MixJobModel()
                {
                    Key = "PublishScheduledPostsJob",
                    Group = null,
                    Description = "Publish Scheduled Posts Job",
                    JobName = typeof(KeepPoolAliveJob).Name
                }
            };
        }

        public static IApplicationBuilder UseMixScheduler(this IApplicationBuilder app)
        {
            return app;
        }

        public static async Task StartScheduler()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<PublishScheduledPostsJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();
            HolidayCalendar holidayCalendar = new HolidayCalendar();
            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }

        // simple log provider to get something to the console
        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
        }
    }
}