using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mix.Quartz.Jobs;
using Mix.Quartz.Models;

namespace Mix.Quartz.Services
{
    // Ref: https://andrewlock.net/creating-a-quartz-net-hosted-service-with-asp-net-core/
    public class QuartzHostedService : IHostedService
    {
        private readonly IEnumerable<MixJobBase> _jobSchedules;
        private readonly IQuartzService _service;

        public QuartzHostedService(IEnumerable<MixJobBase> jobSchedules, IQuartzService scheduler)
        {
            _jobSchedules = jobSchedules;
            _service = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            foreach (var jobSchedule in _jobSchedules)
            {
                if (jobSchedule.Schedule != null && !await _service.CheckExist(jobSchedule.Schedule.Name, cancellationToken))
                {
                    jobSchedule.JobName = typeof(JobSchedule).FullName;
                    await _service.ScheduleJob(jobSchedule, cancellationToken);
                }
            }

            await _service.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _service.Shutdown(cancellationToken);
        }
    }
}
