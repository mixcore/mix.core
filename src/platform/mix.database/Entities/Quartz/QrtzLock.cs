using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Quartz
{
    public partial class QrtzLock
    {
        public string SchedName { get; set; }
        public string LockName { get; set; }
    }
}
