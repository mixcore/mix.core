using Microsoft.AspNetCore.Mvc;
using Mix.MixQuartz.Jobs;
using Quartz;
using System.Linq;
using System.Reflection;

namespace Mix.Scheduler.Controllers
{
    [Route("api/quartz")]
    [ApiController]
    public class ApiQuartzController : ControllerBase
    {
        private readonly IScheduler _scheduler;
        public ApiQuartzController(IScheduler scheduler)
        {
            _scheduler = scheduler;

            var jobType = typeof(KeepPoolAliveJob);
            IJobDetail job = JobBuilder.Create(jobType)
                .WithDescription("Job to rescan jobs from SQL db")
                .WithIdentity(jobType.FullName)
                //.UsingJobData(jobData)
                .Build();

            TriggerKey triggerKey = new TriggerKey($"{jobType.FullName}.trigger");

            ITrigger trigger = TriggerBuilder.Create()
                .WithCronSchedule("0/5 * * * * ?")
                .StartNow()
                .WithDescription("trigger for sql job loader")
                .WithIdentity(triggerKey)
                .WithPriority(1)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        [HttpGet]
        public ActionResult GetJobs()
        {
            var mixJobs = Assembly.GetAssembly(typeof(BaseJob))
                .GetExportedTypes()
                .Where(m => m.BaseType.Name == typeof(BaseJob).Name);
            return Ok(mixJobs);
        }
    }
}
