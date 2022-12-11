using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixculture
{
    public int Id { get; set; }

    public string Alias { get; set; }

    public string Icon { get; set; }

    public string Lcid { get; set; }

    public string Specificulture { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public int MixTenantId { get; set; }

    public virtual Mixtenant MixTenant { get; set; }

    public virtual ICollection<Mixconfigurationcontent> Mixconfigurationcontents { get; } = new List<Mixconfigurationcontent>();

    public virtual ICollection<Mixdatacontentassociation> Mixdatacontentassociations { get; } = new List<Mixdatacontentassociation>();

    public virtual ICollection<Mixdatacontent> Mixdatacontents { get; } = new List<Mixdatacontent>();

    public virtual ICollection<Mixdatacontentvalue> Mixdatacontentvalues { get; } = new List<Mixdatacontentvalue>();

    public virtual ICollection<Mixlanguagecontent> Mixlanguagecontents { get; } = new List<Mixlanguagecontent>();

    public virtual ICollection<Mixmodulecontent> Mixmodulecontents { get; } = new List<Mixmodulecontent>();

    public virtual ICollection<Mixmoduledatum> Mixmoduledata { get; } = new List<Mixmoduledatum>();

    public virtual ICollection<Mixpagecontent> Mixpagecontents { get; } = new List<Mixpagecontent>();

    public virtual ICollection<Mixpostcontent> Mixpostcontents { get; } = new List<Mixpostcontent>();
}
