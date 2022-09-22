using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Services;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
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
                IntervalType = Enums.MixIntevalType.Second
            };
        }

        public override async Task ExecuteHandler(IJobExecutionContext context)
        {
            string domain = context.Trigger.JobDataMap.GetString("domain");
            if (!string.IsNullOrEmpty(domain))
            {
                var now = DateTime.UtcNow;
                var ping = await _httpService.GetAsync<DateTime>($"{domain}/api/v2/rest/shared/ping");
                Console.WriteLine($"Ping at {now}: {(ping - now).TotalMilliseconds}");
            }
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}