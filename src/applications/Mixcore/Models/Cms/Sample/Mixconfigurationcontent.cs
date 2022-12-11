using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixconfigurationcontent
{
    public int Id { get; set; }

    public string DefaultContent { get; set; }

    public int? MixConfigurationId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public int MixTenantId { get; set; }

    public string Specificulture { get; set; }

    public string Icon { get; set; }

    public bool IsPublic { get; set; }

    public int ParentId { get; set; }

    public int MixCultureId { get; set; }

    public string DisplayName { get; set; }

    public string SystemName { get; set; }

    public string Description { get; set; }

    public string Content { get; set; }

    public string Category { get; set; }

    public string DataType { get; set; }

    public virtual Mixconfiguration MixConfiguration { get; set; }

    public virtual Mixculture MixCulture { get; set; }
}
