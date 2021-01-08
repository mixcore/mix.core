﻿using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Clients
    {
        public string Id { get; set; }
        public ulong Active { get; set; }
        public string AllowedOrigin { get; set; }
        public int ApplicationType { get; set; }
        public string Name { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string Secret { get; set; }
    }
}
