using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib
{
    public class MixAutomationConstants
    {
        public static class DatabaseNames
        {
            public const string Workflow = "mix_workflow";
            public const string WorkflowAction = "mix_workflow_action";
            public const string WorkflowTrigger = "mix_workflow_trigger";
            public const string WorkflowActionData = "mix_workflow_action_data";
        }

        public static class Topics
        {
            public const string MixAutomation = "MixAutomation";
        }
    }
}
