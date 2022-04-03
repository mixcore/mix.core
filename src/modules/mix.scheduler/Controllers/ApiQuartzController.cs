using Microsoft.AspNetCore.Mvc;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Models;
using Mix.Quartz.Services;
using System.Linq;
using System.Reflection;

namespace Mix.Scheduler.Controllers
{
    [Route("api/quartz")]
    [ApiController]
    public class ApiQuartzController : ControllerBase
    {
        private readonly QuartzService _service;
        public ApiQuartzController(QuartzService service)
        {
            _service = service;
        }

        [HttpGet("trigger/{key}")]
        public ActionResult GetTrigger(string key)
        {
            return Ok(_service.GetTrigger(key));
        }

        [HttpPost("reschedule/{key}")]
        public async System.Threading.Tasks.Task<ActionResult> GetJobAsync(string key, [FromBody] JobSchedule schedule)
        {
            var trigger = await _service.GetTrigger(key);
            var job = await _service.GetJob(trigger.JobKey.Name);
            var newTrigger = _service.CreateTrigger(schedule, key);
            //new JobSchedule(typeof(KeepPoolAliveJob))
            //{
            //    CronExpression = "0/5 * * * * ?",
            //    IsStartNow = true,
            //    StartAt = DateTime.Now,
            //    Interval = 1,
            //    IntervalType = MixIntevalType.Second
            //}
            await _service.Scheduler.RescheduleJob(trigger.Key, newTrigger);
            return Ok();
        }

        [HttpGet("jobs")]
        public ActionResult GetJobs()
        {
            var mixJobs = Assembly.GetAssembly(typeof(MixJobBase))
                .GetExportedTypes()
                .Where(m => m.BaseType.Name == typeof(MixJobBase).Name);
            return Ok(mixJobs);
        }

        [HttpGet("trigger-keys")]
        public ActionResult GetTriggerKeys()
        {
            var result = _service.GetJobTriggerKeys();
            return Ok(result);
        }
    }
}
