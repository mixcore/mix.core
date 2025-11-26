using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Automation.Lib.Entities
{
    public class WorkflowTrigger : WorkflowEntityBase
    {
        public bool IsSuccess { get; set; }
        public int WorkflowId { get; set; }
        public JObject? Input { get; set; }
    }
}
