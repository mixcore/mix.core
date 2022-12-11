using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Aspnetuserclaim
{
    public int Id { get; set; }

    public Guid? MixUserId1 { get; set; }

    public Guid UserId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public Guid? MixUserId { get; set; }

    public virtual Mixuser MixUser { get; set; }

    public virtual Mixuser MixUserId1Navigation { get; set; }
}
