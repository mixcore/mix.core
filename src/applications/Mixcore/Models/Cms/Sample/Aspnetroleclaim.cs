using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Aspnetroleclaim
{
    public int Id { get; set; }

    public Guid? MixRoleId { get; set; }

    public Guid RoleId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public virtual Mixrole MixRole { get; set; }
}
