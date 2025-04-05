using Mix.Automation.Lib.Enums;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Automation.Lib.Entities
{
    public class WorkflowActionData : WorkflowEntityBase
    {
        public bool? IsSuccess { get; set; }
        public ActionStatus? ActionStatus { get; set; }
        public ActionType ActionType { get; set; }
        public double Duration { get; set; }
        public int TriggerId { get; set; }
        public int ActionId { get; set; }
        public int Index { get; set; }
        public JObject? Request { get; set; }
        public JObject? Body { get; set; }
        public JObject? Response { get; set; }
        public JObject? Exception { get; set; }
    }
}
