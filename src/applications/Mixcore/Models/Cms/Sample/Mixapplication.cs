using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixapplication
{
    public int Id { get; set; }

    public string BaseHref { get; set; }

    public string BaseRoute { get; set; }

    public string Domain { get; set; }

    public string BaseApiUrl { get; set; }

    public int? TemplateId { get; set; }

    public string Image { get; set; }

    public string MixDatabaseName { get; set; }

    public Guid? MixDataContentId { get; set; }

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

    public virtual Mixdatacontent MixDataContent { get; set; }

    public virtual Mixtenant MixTenant { get; set; }
}
