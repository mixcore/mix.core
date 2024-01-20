using Mix.Queue.Interfaces;
using Mix.Shared.Services;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Quartz.Enums;
using Mix.Quartz.Jobs;
using Mix.Quartz.Models;
using NuGet.Packaging.Signing;
using Mix.Mq.Lib.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Mix.Scheduler.Domain.Jobs
{
    public class KeepPoolAliveJob : MixJobBase
    {
        private readonly HttpService _httpService;
        public KeepPoolAliveJob(
            HttpService httpService,
            IServiceProvider serviceProvider,
            IMemoryQueueService<MessageQueueModel> queueService)
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
            if (context.Trigger.JobDataMap.ContainsKey("data"))
            {
                var objData = JObject.Parse(context.Trigger.JobDataMap.GetString("data") ?? "{}");
                var domains = objData.Value<JArray>("domains");
                foreach (var domain in domains)
                {
                    try
                    {
                        var now = DateTime.UtcNow;
                        var ping = await _httpService.GetStringAsync($"{domain.ToString().TrimEnd('/')}");

                        Console.WriteLine($"Ping {domain} at {now}: {(DateTime.Now - now).TotalMilliseconds}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot Ping: {domain}" + ex.Message);
                    }
                }
            }
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}