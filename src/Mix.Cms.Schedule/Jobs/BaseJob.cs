using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule.Jobs
{
    public abstract class BaseJob: IJob
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public Type JobType { get; set; }
        public abstract Task Execute(IJobExecutionContext context);
    }
}
