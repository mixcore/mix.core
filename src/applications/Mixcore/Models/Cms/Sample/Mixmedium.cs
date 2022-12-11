using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixmedium
{
    public Guid Id { get; set; }

    public string Extension { get; set; }

    public string FileFolder { get; set; }

    public string FileName { get; set; }

    public string FileProperties { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; }

    public string Title { get; set; }

    public string Tags { get; set; }

    public string Source { get; set; }

    public string TargetUrl { get; set; }

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
}
