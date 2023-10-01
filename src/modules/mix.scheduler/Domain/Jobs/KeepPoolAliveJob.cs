using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Services;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Quartz.Enums;
using Mix.Quartz.Jobs;
using Mix.Quartz.Models;
using NuGet.Packaging.Signing;

namespace Mix.Scheduler.Domain.Jobs
{
    public class KeepPoolAliveJob : MixJobBase
    {
        private readonly HttpService _httpService;
        public KeepPoolAliveJob(
            HttpService httpService,
            IServiceProvider serviceProvider,
            IQueueService<MessageQueueModel> queueService)
            : base(serviceProvider, queueService)
        {
            _httpService = httpService;
            Schedule = new JobSchedule(GetType())
            {
                StartAt = DateTime.Now,
                Interval = 5,
                IntervalType = MixIntervalType.Second,
                RepeatCount = 5
            };
        }

        public override async Task ExecuteHandler(IJobExecutionContext context)
        {
            string domain = context.Trigger.JobDataMap.GetString("domain");
            if (!string.IsNullOrEmpty(domain))
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var ping = await _httpService.GetStringAsync($"{domain.TrimEnd('/')}");

                    Console.WriteLine($"Ping at {now}: {(DateTime.Now - now).TotalMilliseconds}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot Ping: " + ex.Message);
                }
            }
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}