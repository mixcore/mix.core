using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Productdetail
{
    public int Id { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public int? MixTenantId { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool? IsDeleted { get; set; }

    public float? Price { get; set; }

    public string Brands { get; set; }

    public int? ParentId { get; set; }

    public string ParentType { get; set; }

    public string DesignBy { get; set; }

    public string Information { get; set; }

    public string InformationImage { get; set; }

    public string Size { get; set; }

    public string SizeImage { get; set; }

    public string Document { get; set; }

    public string MaintenanceDocument { get; set; }
}
