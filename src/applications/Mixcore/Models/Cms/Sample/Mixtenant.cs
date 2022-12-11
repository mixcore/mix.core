using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixtenant
{
    public int Id { get; set; }

    public string PrimaryDomain { get; set; }

    public string SystemName { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Mixapplication> Mixapplications { get; } = new List<Mixapplication>();

    public virtual ICollection<Mixconfiguration> Mixconfigurations { get; } = new List<Mixconfiguration>();

    public virtual ICollection<Mixculture> Mixcultures { get; } = new List<Mixculture>();

    public virtual ICollection<Mixdatabasecontext> Mixdatabasecontexts { get; } = new List<Mixdatabasecontext>();

    public virtual ICollection<Mixdatabase> Mixdatabases { get; } = new List<Mixdatabase>();

    public virtual ICollection<Mixdomain> Mixdomains { get; } = new List<Mixdomain>();

    public virtual ICollection<Mixlanguage> Mixlanguages { get; } = new List<Mixlanguage>();

    public virtual ICollection<Mixmedium> Mixmedia { get; } = new List<Mixmedium>();

    public virtual ICollection<Mixmodule> Mixmodules { get; } = new List<Mixmodule>();

    public virtual ICollection<Mixpage> Mixpages { get; } = new List<Mixpage>();

    public virtual ICollection<Mixpost> Mixposts { get; } = new List<Mixpost>();

    public virtual ICollection<Mixtheme> Mixthemes { get; } = new List<Mixtheme>();

    public virtual ICollection<Mixurlalias> Mixurlaliases { get; } = new List<Mixurlalias>();
}
