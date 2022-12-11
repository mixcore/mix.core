using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class QrtzSimpleTrigger
{
    public string SchedName { get; set; }

    public string TriggerName { get; set; }

    public string TriggerGroup { get; set; }

    public long RepeatCount { get; set; }

    public long RepeatInterval { get; set; }

    public long TimesTriggered { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; }
}
