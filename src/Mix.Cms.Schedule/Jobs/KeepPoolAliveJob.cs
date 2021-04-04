using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule.Jobs
{
    public class KeepPoolAliveJob : IJob
    {
        HttpService _httpService;

        public KeepPoolAliveJob(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var domain = MixService.GetConfig<string>(MixAppSettingKeywords.Domain);
            if (!string.IsNullOrEmpty(domain))
            {

                var now = DateTime.UtcNow;
                var ping = await _httpService.GetAsync<DateTime>($"{domain}/api/v1/rest/shared/ping");
                Console.WriteLine($"Ping at {now}: {(ping - now).TotalMilliseconds}");
            }
        }
    }
}