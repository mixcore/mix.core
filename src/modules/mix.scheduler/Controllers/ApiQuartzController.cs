using Microsoft.AspNetCore.Mvc;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Models;
using Mix.Quartz.Services;
using Quartz;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
        
        [HttpPost("trigger/{jobKey}")]
        public async Task<ActionResult> CreateTriggerAsync(string jobKey, [FromBody] JobSchedule schedule)
        {
            var jobType = Assembly.GetAssembly(typeof(MixJobBase)).GetType(jobKey);
            var job = await _service.GetJob(jobKey);
            if (job == null)
            {
                job = _service.CreateJob(jobType);
                var trigger = _service.CreateTrigger(schedule, DateTime.Now.Ticks.ToString());
                await _service.Scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                var trigger = _service.CreateTrigger(schedule, DateTime.Now.Ticks.ToString(), job);
                await _service.Scheduler.ScheduleJob(trigger);
            }
            return Ok();
        }

        [HttpPost("reschedule/{key}")]
        public async Task<ActionResult> GetJobAsync(string key, [FromBody] JobSchedule schedule)
        {
            var trigger = await _service.GetTrigger(key);
            var job = await _service.GetJob(trigger.JobKey.Name);
            var newTrigger = _service.CreateTrigger(schedule, key);
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
