using Mix.Shared.Constants;
using Mix.Shared.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class KeepPoolAliveJob : BaseJob
    {
        private readonly HttpService _httpService;
        private readonly GlobalConfigService _globalConfigService;
        public KeepPoolAliveJob(HttpService httpService, GlobalConfigService globalConfigService)
        {
            _httpService = httpService;
            _globalConfigService = globalConfigService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            var domain = _globalConfigService.GetConfig<string>(MixAppSettingKeywords.Domain);
            if (!string.IsNullOrEmpty(domain))
            {
                var now = DateTime.UtcNow;
                var ping = await _httpService.GetAsync<DateTime>($"{domain}/api/v1/rest/shared/ping");
                Console.WriteLine($"Ping at {now}: {(ping - now).TotalMilliseconds}");
            }
        }
    }
}