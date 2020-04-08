using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Cms.Scheduler
{
    public class MixScheduler
    {
        public static async Task Demo()
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
    {
        { "quartz.serializer.type", "binary" }
    };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            IScheduler sched = await factory.GetScheduler();
            await sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .Build();
            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartAt(DateTime.Now.AddSeconds(10))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
            .Build();

            await sched.ScheduleJob(job, trigger);
            
        }
    }
    class JobsAndTriggers
    {
        public string jobIdentityKey { get; set; }
        public string TriggerIdentityKey { get; set; }
        public string jobData_MethodName { get; set; }
        public int ScheduleIntervalInSec { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<JobsAndTriggers> LstobjJobsAndTriggers = new List<JobsAndTriggers>();
            LstobjJobsAndTriggers.Add(new JobsAndTriggers { jobIdentityKey = "JOB1", TriggerIdentityKey = "TRIGGER1", jobData_MethodName = "JOB1_METHOD()", ScheduleIntervalInSec = 5 });
            LstobjJobsAndTriggers.Add(new JobsAndTriggers { jobIdentityKey = "JOB2", TriggerIdentityKey = "TRIGGER2", jobData_MethodName = "JOB2_METHOD()", ScheduleIntervalInSec = 10 });


            TestDemoJob1(LstobjJobsAndTriggers).GetAwaiter().GetResult();

            Console.ReadKey();

        }
        public static async Task TestDemoJob1(List<JobsAndTriggers> lstJobsAndTriggers)

        {

            IScheduler scheduler;
            IJobDetail job = null;
            ITrigger trigger = null;

            var schedulerFactory = new StdSchedulerFactory();

            scheduler = schedulerFactory.GetScheduler().Result;

            scheduler.Start().Wait();

            foreach (var JobandTrigger in lstJobsAndTriggers)
            {

                //  int ScheduleIntervalInSec = 1;//job will run every minute

                JobKey jobKey = JobKey.Create(JobandTrigger.jobIdentityKey);



                job = JobBuilder.Create<DemoJob1>().
                   WithIdentity(jobKey)
                   .UsingJobData("MethodName", JobandTrigger.jobData_MethodName)
                   .Build();



                trigger = TriggerBuilder.Create()

                .WithIdentity(JobandTrigger.TriggerIdentityKey)

                .StartNow()

                .WithSimpleSchedule(x => x.WithIntervalInSeconds(JobandTrigger.ScheduleIntervalInSec).WithRepeatCount(1)
                // .RepeatForever()
                )

                .Build();

                await scheduler.ScheduleJob(job, trigger);

            }



        }
    }
    public class DemoJob1 : IJob

    {

        public Task Execute(IJobExecutionContext context)

        {

            //simple task, the job just prints current datetime in console

            //return Task.Run(() => {

            //    //Console.WriteLine(DateTime.Now.ToString());

            //});
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string MethodName = dataMap.GetString("MethodName");
            Console.WriteLine(DateTime.Now.ToString() + " " + MethodName);

            return Task.FromResult(0);

        }

    }
    public class HelloJob : IJob
    {
        private readonly HubConnection _connection;
        public HelloJob()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/portalHub")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .AddMessagePackProtocol()
                .Build();
           _connection.On<string>("receive_message", (resp) =>
           {
               Console.WriteLine(resp);
           });
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("HelloJob is executing.");
            await AlertAsync();
        }

        protected async Task AlertAsync()
        {
            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                {
                    await _connection.StartAsync();
                }
                await _connection.InvokeAsync("SendMessage", $"{{\"msg\": \"Hello at {DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")}\"}}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
