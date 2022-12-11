using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class QrtzCalendar
{
    public string SchedName { get; set; }

    public string CalendarName { get; set; }

    public byte[] Calendar { get; set; }
}
