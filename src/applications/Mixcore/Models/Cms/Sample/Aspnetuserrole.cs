using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Aspnetuserrole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public int MixTenantId { get; set; }

    public Guid? MixUserId1 { get; set; }

    public Guid? MixRoleId { get; set; }

    public Guid? MixUserId { get; set; }

    public virtual Mixrole MixRole { get; set; }

    public virtual Mixuser MixUser { get; set; }

    public virtual Mixuser MixUserId1Navigation { get; set; }
}
