using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Client
{
    public string Id { get; set; }

    public bool Active { get; set; }

    public string AllowedOrigin { get; set; }

    public int ApplicationType { get; set; }

    public string Name { get; set; }

    public int RefreshTokenLifeTime { get; set; }

    public string Secret { get; set; }
}
