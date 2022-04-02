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
