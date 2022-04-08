using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
using Mix.MixQuartz.Jobs;
using Mix.MixQuartz.Models;
using Mix.Quartz.Services;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Scheduler.Controllers
{
    [Route("/api/v2/scheduler")]
    [ApiController]
    public class ApiQuartzController : ControllerBase
    {
        private readonly QuartzService _service;
        public ApiQuartzController(QuartzService service)
        {
            _service = service;
        }

        [HttpGet("job-schedule/default")]
        public ActionResult GetDefaultJobSchedule()
        {
            var obj = new JobSchedule();
            return Ok(ReflectionHelper.ParseObject(obj).ToString());
        }

        [HttpGet("trigger/{name}")]
        public async Task<ActionResult> GetTrigger(string name)
        {
            var trigger = await _service.GetTrigger(name);
            return Ok(trigger);
        }

        [HttpGet("trigger/pause/{name}")]
        public async Task<ActionResult> PauseTrigger(string name)
        {
            await _service.PauseTrigger(name);
            return Ok();
        }


        [HttpGet("trigger/resume/{name}")]
        public async Task<ActionResult> ResumeTrigger(string name)
        {
            await _service.ResumeTrigger(name);
            return Ok();
        }

        [HttpPost("trigger/create")]
        public async Task<ActionResult> CreateTriggerAsync([FromBody] JobSchedule schedule)
        {
            await _service.ScheduleJob(schedule);
            return Ok();
        }

        [HttpPost("schedule")]
        public async Task<ActionResult> Schedule([FromBody] JobSchedule schedule)
        {
            await _service.ScheduleJob(schedule);
            return Ok();
        }

        [HttpPost("reschedule")]
        public async Task<ActionResult> Reschedule([FromBody] JobSchedule schedule)
        {
            await _service.ResheduleJob(schedule);
            return Ok();
        }

        [HttpGet("job")]
        public ActionResult GetJobs()
        {
            var mixJobs = Assembly.GetAssembly(typeof(MixJobBase))
                .GetExportedTypes()
                .Where(m => m.BaseType.Name == typeof(MixJobBase).Name);
            return Ok(mixJobs.Select(m => m.FullName));
        }

        [HttpGet("trigger")]
        public async Task<ActionResult> GetTriggerKeysAsync()
        {
            var keys = await _service.GetJobTriggerKeys();
            List<JobSchedule> result = new();
            foreach (var key in keys)
            {
                var trigger = await _service.GetTrigger(key.Name);
                if (trigger != null)
                {
                    result.Add(trigger);
                }
            }
            return Ok(result);
        }
    }
}
