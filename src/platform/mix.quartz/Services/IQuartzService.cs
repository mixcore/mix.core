using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mix.Quartz.Jobs;
using Mix.Quartz.Models;

namespace Mix.Quartz.Services
{
    public interface IQuartzService
    {
        Task LoadScheduler();

        Task<bool> CheckExist(string triggerName, CancellationToken cancellationToken = default);

        Task PauseTrigger(string id, CancellationToken cancellationToken = default);

        Task ResumeTrigger(string id, CancellationToken cancellationToken = default);

        Task<JobSchedule> GetTrigger(string id, CancellationToken cancellationToken = default);

        Task<bool> DeleteJob(string jobName, CancellationToken cancellationToken = default);

        Task<IJobDetail> GetJob(string id, CancellationToken cancellationToken = default);

        Task<IJobDetail> GetJob<T>(CancellationToken cancellationToken = default);

        Task<IEnumerable<TriggerKey>> GetJobTriggerKeys(CancellationToken cancellationToken = default);

        Task Start(CancellationToken cancellationToken = default);

        Task Shutdown(CancellationToken cancellationToken = default);

        Task ScheduleJob(MixJobBase jobSchedule, CancellationToken cancellationToken = default);

        Task ScheduleJob<T>(JobSchedule schedule, CancellationToken cancellationToken = default) where T : MixJobBase;

        Task ScheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default);

        Task RescheduleJob(JobSchedule schedule, CancellationToken cancellationToken = default);
    }
}