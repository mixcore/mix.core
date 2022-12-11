using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Aspnetuserlogin
{
    public string LoginProvider { get; set; }

    public string ProviderKey { get; set; }

    public string ProviderDisplayName { get; set; }

    public Guid UserId { get; set; }

    public Guid? MixUserId { get; set; }

    public Guid? MixUserId1 { get; set; }

    public virtual Mixuser MixUser { get; set; }

    public virtual Mixuser MixUserId1Navigation { get; set; }
}
