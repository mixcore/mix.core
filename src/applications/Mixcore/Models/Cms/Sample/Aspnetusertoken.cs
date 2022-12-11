using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Aspnetusertoken
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public Guid? MixUserId { get; set; }

    public virtual Mixuser MixUser { get; set; }
}
