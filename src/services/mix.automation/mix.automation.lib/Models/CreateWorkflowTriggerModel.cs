using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Models
{
    public sealed class CreateWorkflowTriggerModel
    {
        public int WorkflowId { get; set; }
        public JObject? Input { get; set; }
    }
}
