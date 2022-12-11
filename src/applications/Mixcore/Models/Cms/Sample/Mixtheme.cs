using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixtheme
{
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public string PreviewUrl { get; set; }

    public string AssetFolder { get; set; }

    public string TemplateFolder { get; set; }

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

    public string SystemName { get; set; }

    public virtual Mixdatacontent MixDataContent { get; set; }

    public virtual Mixtenant MixTenant { get; set; }

    public virtual ICollection<Mixviewtemplate> Mixviewtemplates { get; } = new List<Mixviewtemplate>();
}
