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
        private readonly MixAppSettingService _appSettingService;
        public KeepPoolAliveJob(HttpService httpService, MixAppSettingService appSettingService)
        {
            _httpService = httpService;
            _appSettingService = appSettingService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            var domain = _appSettingService.GetConfig<string>(Shared.Enums.MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.Domain);
            if (!string.IsNullOrEmpty(domain))
            {
                var now = DateTime.UtcNow;
                var ping = await _httpService.GetAsync<DateTime>($"{domain}/api/v1/rest/shared/ping");
                Console.WriteLine($"Ping at {now}: {(ping - now).TotalMilliseconds}");
            }
        }
    }
}