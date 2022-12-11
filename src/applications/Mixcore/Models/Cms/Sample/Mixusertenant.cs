using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixusertenant
{
    public int TenantId { get; set; }

    public Guid MixUserId { get; set; }
}
