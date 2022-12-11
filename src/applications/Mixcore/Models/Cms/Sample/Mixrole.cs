using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixrole
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string ConcurrencyStamp { get; set; }

    public virtual ICollection<Aspnetroleclaim> Aspnetroleclaims { get; } = new List<Aspnetroleclaim>();

    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; } = new List<Aspnetuserrole>();
}
