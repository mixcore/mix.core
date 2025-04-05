using Mix.Heart.Entities;
using Mix.Heart.Enums;

namespace Mix.Automation.Lib.Entities
{
    public class Workflow: WorkflowEntityBase
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
