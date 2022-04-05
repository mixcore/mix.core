using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Quartz
{
    public partial class QrtzPausedTriggerGrp
    {
        public string SchedName { get; set; }
        public string TriggerGroup { get; set; }
    }
}
