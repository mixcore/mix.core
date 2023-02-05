using Mix.MixQuartz.Enums;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Jobs
{
    public class KeepPoolAliveJob : MixJobBase
    {
        private readonly HttpService _httpService;
        public KeepPoolAliveJob(
            HttpService httpService,
            IServiceProvider provider,
            IQueueService<MessageQueueModel> queueService)
            : base(provider, queueService)
        {
            _httpService = httpService;
            Schedule = new JobSchedule(GetType())
            {
                StartAt = DateTime.Now,
                Interval = 5,
                IntervalType = MixIntevalType.Second
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
                    var ping = await _httpService.GetStringAsync($"{domain.TrimEnd('/')}/api/v2/rest/shared/ping");

                    Console.WriteLine($"Ping at {now}: {(DateTime.Parse(ping) - now).TotalMilliseconds}");
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