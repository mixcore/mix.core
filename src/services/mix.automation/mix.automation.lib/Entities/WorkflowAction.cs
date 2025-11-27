using Mix.Automation.Lib.Enums;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Automation.Lib.Entities
{
    public class WorkflowAction: WorkflowEntityBase
    {
        public ActionType Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Index { get; set; }
        public int? WorkflowId { get; set; }
        public JObject? Request { get; set; }
        public JObject? Body { get; set; }
    }
}
