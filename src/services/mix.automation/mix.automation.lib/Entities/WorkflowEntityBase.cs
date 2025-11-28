using Mix.Heart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities
{
    public abstract class WorkflowEntityBase : SimpleEntityBase<int>
    {
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedBy { get; set; }
        public int Priority { get; set; }
    }
}
