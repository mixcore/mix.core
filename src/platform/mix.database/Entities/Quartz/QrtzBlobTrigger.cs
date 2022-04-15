using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Quartz
{
    public partial class QrtzBlobTrigger
    {
        public string SchedName { get; set; }
        public string TriggerName { get; set; }
        public string TriggerGroup { get; set; }
        public byte[] BlobData { get; set; }
    }
}
