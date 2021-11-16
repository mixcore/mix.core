using Mix.Shared.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class KeepPoolAliveJob : BaseJob
    {
        private readonly HttpService _httpService;
        public KeepPoolAliveJob(HttpService httpService)
        {
            _httpService = httpService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            var domain = GlobalConfigService.Instance.AppSettings.Domain;
            if (!string.IsNullOrEmpty(domain))
            {
                var now = DateTime.UtcNow;
                var ping = await _httpService.GetAsync<DateTime>($"{domain}/api/v1/rest/shared/ping");
                Console.WriteLine($"Ping at {now}: {(ping - now).TotalMilliseconds}");
            }
        }
    }
}