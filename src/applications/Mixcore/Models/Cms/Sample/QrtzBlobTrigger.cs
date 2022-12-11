using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class QrtzBlobTrigger
{
    public string SchedName { get; set; }

    public string TriggerName { get; set; }

    public string TriggerGroup { get; set; }

    public byte[] BlobData { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; }
}
