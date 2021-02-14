using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Services;
using Mix.Cms.Schedule.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Logging;
using System;
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
                    // here's a known job for triggers
                    var jobKey = new JobKey("PublishScheduledPostsJob", "Publish Scheduled Posts Job");
                    q.AddJob<PublishScheduledPostsJob>(jobKey, j => j
                        .WithDescription("Publish Scheduled Posts Job")
                    );

                    q.AddTrigger(t => t
                        .WithIdentity("PublishScheduledPostsTrigger")
                        .ForJob(jobKey)
                        .StartAt(DateTime.UtcNow.AddSeconds(10))
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(1)
                        .RepeatForever())
                        .WithDescription("Publish Scheduled Posts trigger")
                    );
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