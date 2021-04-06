using Quartz;
using System.Threading.Tasks;

namespace Mix.Cms.Schedule.Jobs
{
    public abstract class BaseJob: IJob
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public BaseJob(string key, string group = null)
        {
            Key = key;
            Group = group;
        }
        public abstract Task Execute(IJobExecutionContext context);
    }
}
